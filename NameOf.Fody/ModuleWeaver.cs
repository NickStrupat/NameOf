using System;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace NameOf.Fody {
    public partial class ModuleWeaver {
        public ModuleDefinition ModuleDefinition { get; set; }
        public void Execute() {
            foreach (var typeDefinition in ModuleDefinition.GetTypes()) {
                foreach (var methodDefinition in typeDefinition.Methods.Where(x => x.HasBody))
                    ProcessMethod(methodDefinition);
            }
            // TODO: Remove Name.Of assembly and all references to it
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
        private static readonly OpCode[] LoadOpCodes = { OpCodes.Ldfld, OpCodes.Ldsfld, OpCodes.Ldflda, OpCodes.Ldsflda, OpCodes.Ldloc, OpCodes.Ldloc_S, OpCodes.Ldloca, OpCodes.Ldloca_S, OpCodes.Ldloc_0, OpCodes.Ldloc_1, OpCodes.Ldloc_2, OpCodes.Ldloc_3 };
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
        private static void ProcessNameOfCallInstruction(Instruction instruction, ILProcessor ilProcessor) {
            Boolean patternNotMatched = true;
            PatternInstruction[] patternMatched = null;
            Terminal terminal = null;
            Instruction terminalInstruction = null;
            Instruction iterator;
            var patternsOrdered = nameOfCallPatterns.OrderByDescending(x => x.Count()); // Ordered by length of pattern to ensure longest possible patterns (of which shorter patterns might be a subset) are checked first
            foreach (var pattern in patternsOrdered) {
                iterator = instruction;
                patternNotMatched = false;
                patternMatched = null;
                terminal = null;
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
                    iterator = iterator.Previous;
                }
                if (!patternNotMatched) {
                    patternMatched = pattern;
                    break;
                }
            }
            if (patternNotMatched) {
                // The usage of Name.Of is not supported
                var i = instruction;
                while (i.SequencePoint == null && i.Previous != null) // Look for last sequence point
                    i = i.Previous;
                String exceptionMessage = String.Format("This usage of '{0}.{1}' is not supported. Source: {2} - line {3}",
                    NameOfMethodInfo.DeclaringType.Name,
                    NameOfMethodInfo.Name,
                    i.SequencePoint.Document.Url,
                    i.SequencePoint.StartLine);
                throw new NotSupportedException(exceptionMessage);
            }
            if (terminal == null)
                throw new NotImplementedException("There is no terminal expression implemented for the matched pattern.");
            String name = terminal(terminalInstruction, ilProcessor);
            // TODO: Remove the anonymous methods generated by lamdba expressions in some uses of Name.Of...
            ilProcessor.InsertAfter(instruction, Instruction.Create(OpCodes.Ldstr, name));
            iterator = instruction;
            foreach (var patternMatchedInstruction in patternMatched.Reverse()) {
                if (patternMatchedInstruction is OptionalPatternInstruction && !patternMatchedInstruction.EligibleOpCodes.Contains(iterator.OpCode))
                    continue;
                var temp = iterator.Previous;
                ilProcessor.Remove(iterator);
                iterator = temp;
            }
        }
    }
}