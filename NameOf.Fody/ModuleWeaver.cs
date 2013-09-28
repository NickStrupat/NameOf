using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace NameOf.Fody {
    public partial class ModuleWeaver {
        public ModuleDefinition ModuleDefinition {
            get { return moduleDefinition; }
            set { moduleDefinition = value; }
        }
        private static ModuleDefinition moduleDefinition;

        private static readonly List<MethodDefinition> UnusedMethodDefinitions = new List<MethodDefinition>();
        private static readonly List<FieldDefinition> UnusedFieldDefinitions = new List<FieldDefinition>();
        public void Execute() {
            foreach (var typeDefinition in ModuleDefinition.GetTypes()) {
                foreach (var methodDefinition in typeDefinition.Methods.Where(x => x.HasBody))
                    ProcessMethod(methodDefinition);
            }
            var nameOfAssemblyReference = ModuleDefinition.AssemblyReferences.SingleOrDefault(x => x.FullName == typeof(Name).Assembly.FullName);
            ModuleDefinition.AssemblyReferences.Remove(nameOfAssemblyReference);
            //foreach (var unusedMethodDefinition in UnusedMethodDefinitions)
            //    unusedMethodDefinition.DeclaringType.Methods.Remove(unusedMethodDefinition);
            //foreach (var unusedFieldDefinition in UnusedFieldDefinitions)
            //    unusedFieldDefinition.DeclaringType.Fields.Remove(unusedFieldDefinition);
            // TODO: Make sure Name.Of.dll isn't placed in the build directory
        }
        static Boolean IsUsed(MethodDefinition methodDefinition) {
            return moduleDefinition.GetTypes().SelectMany(x => x.Methods).SelectMany(x => x.Body.Instructions).Any(x => CallOpCodes.Contains(x.OpCode) && x.Operand == methodDefinition);
        }
        static Boolean IsUsed(FieldDefinition fieldDefinition) {
            return moduleDefinition.GetTypes().SelectMany(x => x.Methods).SelectMany(x => x.Body.Instructions).Any(x => LoadFieldOpCodes.Contains(x.OpCode) && x.Operand == fieldDefinition);
        }
        private static readonly MethodInfo NameOfMethodInfo = new Func<Object, String>(Name.Of).Method;
        private static readonly String NameOfMethodSignature = String.Format("{0} {1}::{2}", NameOfMethodInfo.ReturnType, NameOfMethodInfo.DeclaringType, NameOfMethodInfo.Name);
        private static Boolean IsNameOfCallInstruction(Instruction instruction) {
            return instruction.OpCode == OpCodes.Call && ((MethodReference) instruction.Operand).FullName.StartsWith(NameOfMethodSignature);
        }
        private static void ProcessMethod(MethodDefinition methodDefinition) {
            var nameOfCallInstructions = methodDefinition.Body.Instructions.Where(IsNameOfCallInstruction).ToList();
            var methodBodyIlProcessor = methodDefinition.Body.GetILProcessor();
            foreach (var nameOfCallInstruction in nameOfCallInstructions)
                ProcessNameOfCallInstruction(nameOfCallInstruction, methodBodyIlProcessor);
        }
        private static Int32 GetNumberAtEndOf(OpCode opCode) {
            Int32 number = Int32.Parse(opCode.ToString().Last().ToString());
            if (opCode.ToString().Reverse().Skip(1).First() == 'm')
                return number * -1;
            return number;
        }
        private static readonly OpCode[] CallOpCodes = { OpCodes.Call, OpCodes.Calli, OpCodes.Callvirt };
        private static readonly OpCode[] LambdaOpCodes = CallOpCodes.Concat(new[] { OpCodes.Ldftn, OpCodes.Ldfld }).ToArray();
        private static readonly OpCode[] LoadFieldOpCodes = {OpCodes.Ldfld, OpCodes.Ldsfld, OpCodes.Ldflda, OpCodes.Ldsflda};
        private static readonly OpCode[] LoadOpCodes = LoadFieldOpCodes.Concat(new[] { OpCodes.Ldloc, OpCodes.Ldloc_S, OpCodes.Ldloca, OpCodes.Ldloca_S, OpCodes.Ldloc_0, OpCodes.Ldloc_1, OpCodes.Ldloc_2, OpCodes.Ldloc_3 }).ToArray();
        private static readonly MethodInfo GetTypeFromHandleMethodInfo = new Func<RuntimeTypeHandle, Object>(Type.GetTypeFromHandle).Method;
        private static readonly String GetTypeFromHandleMethodSignature = String.Format("{0} {1}::{2}(", GetTypeFromHandleMethodInfo.ReturnType, GetTypeFromHandleMethodInfo.DeclaringType, GetTypeFromHandleMethodInfo.Name);

        private static Boolean ContainsOpCodes(Instruction instruction, ILProcessor ilProcessor, OpCode[] validOpCodes) {
            return ((MethodDefinition)instruction.Operand).Body.Instructions.SingleOrDefault(x => validOpCodes.Contains(x.OpCode)) != null;
        }
        private static String GetNameFromAnonymousMethod(Instruction instruction, ILProcessor ilProcessor, OpCode[] validOpCodes) {
            var memberReference = (MemberReference)((MethodDefinition)instruction.Operand).Body.Instructions.Single(x => validOpCodes.Contains(x.OpCode)).Operand;
            var name = memberReference.Name;
            var methodDefinition = memberReference as MethodDefinition;
            if (methodDefinition != null) {
                if (methodDefinition.IsGetter || methodDefinition.IsAddOn)
                    name = name.Substring(4); // remove leading "get_" or "add_"
                else if (methodDefinition.IsRemoveOn)
                    name = name.Substring(7); // remove leading "remove_"
            }
            return name;
        }
        private static String GetNameFrom(MethodDefinition methodDefinition) {
            String name = methodDefinition.Name;
            if (methodDefinition.IsGetter || methodDefinition.IsAddOn)
                return name.Substring(4); // remove leading "get_" or "add_"
            if (methodDefinition.IsRemoveOn)
                return name.Substring(7); // remove leading "remove_"
            return name;
        }
        private static void ProcessNameOfCallInstruction(Instruction instruction, ILProcessor ilProcessor) {
            Boolean patternNotMatched = true;
            PatternInstruction[] patternMatched = null;
            Action action = null;
            Terminal terminal = null;
            Instruction terminalInstruction = null;
            Instruction iterator;
            var patternsOrdered = nameOfCallPatterns.OrderByDescending(x => x.Count()); // Ordered by length of pattern to ensure longer patterns (of which shorter patterns might match) are checked first
            foreach (var pattern in patternsOrdered) {
                iterator = instruction;
                patternNotMatched = false;
                patternMatched = null;
                action = null;
                terminal = null;
                terminalInstruction = null;
                foreach (var patternInstruction in pattern.Reverse()) {
                    if (patternInstruction is OptionalPatternInstruction && !patternInstruction.EligibleOpCodes.Contains(iterator.OpCode))
                        continue;
                    if (!patternInstruction.EligibleOpCodes.Contains(iterator.OpCode) || !patternInstruction.IsPredicated(iterator, ilProcessor) || iterator.Previous == null) {
                        patternNotMatched = true;
                        break;
                    }
                    if (patternInstruction.Terminal != null) {
                        terminalInstruction = iterator;
                        terminal = patternInstruction.Terminal;
                    }
                    if (patternInstruction.Action != null) {
                        var actionInstruction = iterator;
                        PatternInstruction instruction1 = patternInstruction;
                        action = () => instruction1.Action(actionInstruction);
                    }
                    iterator = iterator.Previous;
                }
                if (!patternNotMatched) {
                    patternMatched = pattern;
                    break;
                }
            }
            if (patternNotMatched) {
                // The usage of Name.Of is not supported
                String exceptionMessage = String.Format("This usage of '{0}.{1}' is not supported. {2}", NameOfMethodInfo.DeclaringType.Name,
                                                                                                         NameOfMethodInfo.Name,
                                                                                                         GetSequencePointText(instruction));
                throw new NotSupportedException(exceptionMessage);
            }
            if (terminal == null)
                throw new NotImplementedException("There is no terminal expression implemented for the matched pattern.");
            String name = terminal(terminalInstruction, ilProcessor);
            if (String.IsNullOrWhiteSpace(name))
                throw new Exception(String.Format("No name found for this variable. Debug symbols might not have been loaded. {0}", GetSequencePointText(instruction)));
            ilProcessor.InsertAfter(instruction, Instruction.Create(OpCodes.Ldstr, name));
            iterator = instruction;
            foreach (var patternMatchedInstruction in patternMatched.Reverse()) {
                if (patternMatchedInstruction is OptionalPatternInstruction && !patternMatchedInstruction.EligibleOpCodes.Contains(iterator.OpCode))
                    continue;
                var temp = iterator.Previous;
                ilProcessor.Remove(iterator);
                iterator = temp;
            }
            // TODO: Remove the anonymous methods generated by lamdba expressions in some uses of Name.Of...
            var methodDefinition = terminalInstruction.Operand as MethodDefinition;
            if (methodDefinition != null && !IsUsed(methodDefinition))
                UnusedMethodDefinitions.Add(methodDefinition);
            //if (action != null)
            //    action();
        }
        private static String GetSequencePointText(Instruction instruction) {
            var i = instruction;
            while (i.SequencePoint == null && i.Previous != null) // Look for last sequence point
                i = i.Previous;
            if (i.SequencePoint == null)
                return "No source line information available.";
            return String.Format("Source: {0} - line {1}", i.SequencePoint.Document.Url, i.SequencePoint.StartLine);
        }
        private static void CheckIfFieldIsUnused(Instruction instruction) {
            var fieldDefinition = instruction.Operand as FieldDefinition;
            if (fieldDefinition != null && !IsUsed(fieldDefinition))
                UnusedFieldDefinitions.Add(fieldDefinition);
        }
    }
}