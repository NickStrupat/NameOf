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
        private static readonly OpCode[] LoadOpCodes = { OpCodes.Ldfld, OpCodes.Ldsfld, OpCodes.Ldflda, OpCodes.Ldsflda, OpCodes.Ldloc, OpCodes.Ldloc_S, OpCodes.Ldloca, OpCodes.Ldloca_S, OpCodes.Ldloc_0, OpCodes.Ldloc_1, OpCodes.Ldloc_2, OpCodes.Ldloc_3 };
        private static readonly MethodInfo GetTypeFromHandleMethodInfo = new Func<RuntimeTypeHandle, Object>(Type.GetTypeFromHandle).Method;
        private static readonly String GetTypeFromHandleMethodSignature = String.Format("{0} {1}::{2}(", GetTypeFromHandleMethodInfo.ReturnType, GetTypeFromHandleMethodInfo.DeclaringType, GetTypeFromHandleMethodInfo.Name);

        private static String GetNameFromAnonymousMethod(Instruction anonymousMethodCallInstruction, ILProcessor ilProcessor) {
            var instruction = ((MethodDefinition)anonymousMethodCallInstruction.Operand).Body.Instructions.Last();
            Boolean patternNotMatched = true;
            PatternInstruction[] patternMatched = null;
            Terminal terminal = null;
            Instruction terminalInstruction = null;
            var patternsOrdered = lambdaPatterns.OrderByDescending(x => x.Count()); // Ordered by length of pattern to ensure longest possible patterns (of which shorter patterns might be a subset) are checked first
            foreach (var pattern in patternsOrdered) {
                Instruction iterator = instruction;
                patternNotMatched = false;
                patternMatched = null;
                terminal = null;
                foreach (var patternInstruction in pattern.Reverse()) {
                    if (patternInstruction is OptionalPatternInstruction && !patternInstruction.EligibleOpCodes.Contains(iterator.OpCode))
                        continue;
                    if (!patternInstruction.EligibleOpCodes.Contains(iterator.OpCode) || !patternInstruction.IsPredicated(iterator, ilProcessor)) {
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
            if (patternNotMatched)
                throw GetInvalidTerminalYieldException();
            if (terminal == null)
                throw new NotImplementedException("There is no terminal expression implemented for the matched pattern.");
            String name = terminal(terminalInstruction, ilProcessor);
            if (name == null) {
                throw GetInvalidTerminalYieldException();
            }
            return name;
        }
        private static InvalidOperationException GetInvalidTerminalYieldException() {
            return new InvalidOperationException("Terminal didn't yield a valid name.");
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
            if (patternNotMatched)
                throw GetNotSupportedException(instruction); // The usage of Name.Of is not supported
            if (terminal == null)
                throw new NotImplementedException("There is no terminal expression implemented for the matched pattern.");
            String name;
            try {
                name = terminal(terminalInstruction, ilProcessor);
            }
            catch {
                throw GetNotSupportedException(instruction);
            }
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
        private static NotSupportedException GetNotSupportedException(Instruction instruction) {
            String exceptionMessage = String.Format("This usage of '{0}.{1}' is not supported. {2}",
                                                    NameOfMethodInfo.DeclaringType.Name,
                                                    NameOfMethodInfo.Name,
                                                    GetSequencePointText(instruction));
            return new NotSupportedException(exceptionMessage);
        }
        private static String GetSequencePointText(Instruction instruction) {
            var i = instruction;
            while (i.SequencePoint == null && i.Previous != null) // Look for last sequence point
                i = i.Previous;
            if (i.SequencePoint == null)
                return "No source line information available.";
            return String.Format("Source: {0} - line {1}", i.SequencePoint.Document.Url, i.SequencePoint.StartLine);
        }
    }
}