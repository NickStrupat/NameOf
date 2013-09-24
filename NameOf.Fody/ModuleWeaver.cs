using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace NameOf.Fody {
    public class ModuleWeaver {
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
        private static PatternInstruction[][] patterns = {
#region Enum Value
		                                             new [] {
                                                         new PatternInstruction(new []{OpCodes.Ldc_I4, OpCodes.Ldc_I4_S, OpCodes.Ldc_I8}),
                                                         new PatternInstruction(OpCodes.Box, (i, p) => ((TypeDefinition)i.Operand).Fields.Single(x => (x.Constant ?? String.Empty).ToString() == i.Previous.Operand.ToString()).Name, (i, p) => ((TypeDefinition)i.Operand).IsEnum),
                                                         new NameOfPatternInstruction(OpCodes.Call),
                                                     },
		                                             new [] {
                                                         new PatternInstruction(new []{OpCodes.Ldc_I4_M1, OpCodes.Ldc_I4_0, OpCodes.Ldc_I4_1, OpCodes.Ldc_I4_2, OpCodes.Ldc_I4_3, OpCodes.Ldc_I4_4, OpCodes.Ldc_I4_5, OpCodes.Ldc_I4_6, OpCodes.Ldc_I4_7, OpCodes.Ldc_I4_8}),
                                                         new PatternInstruction(OpCodes.Box, (i, p) => ((TypeDefinition)i.Operand).Fields.Single(x => (Int32?)x.Constant == GetNumberAtEndOf(i.Previous.OpCode)).Name, (i, p) => ((TypeDefinition)i.Operand).IsEnum),
                                                         new NameOfPatternInstruction(OpCodes.Call),
                                                     },
#endregion
#region Type
		                                             new [] {
                                                         new PatternInstruction(OpCodes.Ldtoken, (i, p) => ((TypeDefinition)i.Operand).Name),
                                                         new PatternInstruction(OpCodes.Call, null, (i, p) => ((MethodReference)i.Operand).FullName.StartsWith(GetTypeFromHandleMethodSignature)),
                                                         new NameOfPatternInstruction(OpCodes.Call),
                                                     },
		                                             new PatternInstruction[] {
                                                         new NameOfPatternInstruction(OpCodes.Call, (i, p) => ((GenericInstanceMethod)i.Operand).GenericArguments[0].Name, (i, p) => i.Operand is GenericInstanceMethod),
                                                     },
#endregion
#region Local
		                                             new [] {
                                                         new PatternInstruction(OpCodes.Ldloc_0, (i, p) => p.Body.Variables[0].Name),
                                                         new OptionalPatternInstruction(OpCodes.Box),
                                                         new NameOfPatternInstruction(OpCodes.Call),
                                                     },
		                                             new [] {
                                                         new PatternInstruction(OpCodes.Ldloc_1, (i, p) => p.Body.Variables[1].Name),
                                                         new OptionalPatternInstruction(OpCodes.Box),
                                                         new NameOfPatternInstruction(OpCodes.Call),
                                                     },
		                                             new [] {
                                                         new PatternInstruction(OpCodes.Ldloc_2, (i, p) => p.Body.Variables[2].Name),
                                                         new OptionalPatternInstruction(OpCodes.Box),
                                                         new NameOfPatternInstruction(OpCodes.Call),
                                                     },
		                                             new [] {
                                                         new PatternInstruction(OpCodes.Ldloc_3, (i, p) => p.Body.Variables[3].Name),
                                                         new OptionalPatternInstruction(OpCodes.Box),
                                                         new NameOfPatternInstruction(OpCodes.Call),
                                                     },
		                                             new [] {
                                                         new PatternInstruction(new[] {OpCodes.Ldloc, OpCodes.Ldloc_S}, (i, p) => ((VariableReference)i.Operand).Name),
                                                         new OptionalPatternInstruction(OpCodes.Box),
                                                         new NameOfPatternInstruction(OpCodes.Call),
                                                     },
#endregion
#region Member
		                                             new [] {
                                                         new PatternInstruction(OpCodes.Ldsfld),
                                                         new PatternInstruction(OpCodes.Brtrue_S),
                                                         new PatternInstruction(OpCodes.Ldnull),
                                                         new PatternInstruction(OpCodes.Ldftn, GetNameFromAnonymousMethod, (i, p) => ((MethodDefinition)i.Operand).Body.Instructions.SingleOrDefault(x => LambdaOpCodes.Contains(x.OpCode)) != null),
                                                         new PatternInstruction(OpCodes.Newobj),
                                                         new PatternInstruction(OpCodes.Stsfld),
                                                         new PatternInstruction(OpCodes.Br_S),
                                                         new PatternInstruction(OpCodes.Ldsfld),
                                                         new NameOfPatternInstruction(OpCodes.Call),
                                                     },
		                                             new [] {
                                                         new PatternInstruction(OpCodes.Ldnull), 
                                                         new PatternInstruction(OpCodes.Ldftn, (i, p) => ((MethodReference)i.Operand).Name),
                                                         new PatternInstruction(OpCodes.Newobj),
                                                         new NameOfPatternInstruction(OpCodes.Call),
                                                     },
		                                             new [] {
                                                         new PatternInstruction(OpCodes.Ldsfld, (i, p) => ((FieldReference)i.Operand).Name),
                                                         new OptionalPatternInstruction(OpCodes.Box),
                                                         new NameOfPatternInstruction(OpCodes.Call),
                                                     },
		                                             new [] {
                                                         new PatternInstruction(OpCodes.Call, (i, p) => ((MethodDefinition)i.Operand).Name.Substring(4), (i, p) => ((MethodDefinition)i.Operand).IsGetter),
                                                         new OptionalPatternInstruction(OpCodes.Box),
                                                         new NameOfPatternInstruction(OpCodes.Call),
                                                     },
#endregion
#region Arguments
		                                             new [] {
                                                         new PatternInstruction(OpCodes.Ldarg_0, (i, p) => p.Body.Method.Parameters[0].Name),
                                                         new OptionalPatternInstruction(OpCodes.Box),
                                                         new NameOfPatternInstruction(OpCodes.Call),
                                                     },
                                                     new [] {
                                                         new PatternInstruction(OpCodes.Ldarg_1, (i, p) => p.Body.Method.Parameters[1].Name),
                                                         new OptionalPatternInstruction(OpCodes.Box),
                                                         new NameOfPatternInstruction(OpCodes.Call),
                                                     },
                                                     new [] {
                                                         new PatternInstruction(OpCodes.Ldarg_2, (i, p) => p.Body.Method.Parameters[2].Name),
                                                         new OptionalPatternInstruction(OpCodes.Box),
                                                         new NameOfPatternInstruction(OpCodes.Call),
                                                     },
                                                     new [] {
                                                         new PatternInstruction(OpCodes.Ldarg_3, (i, p) => p.Body.Method.Parameters[3].Name),
                                                         new OptionalPatternInstruction(OpCodes.Box),
                                                         new NameOfPatternInstruction(OpCodes.Call),
                                                     },
                                                     new [] {
                                                         new PatternInstruction(new [] {OpCodes.Ldarg, OpCodes.Ldarg_S}, (i, p) => ((ParameterReference)i.Operand).Name),
                                                         new OptionalPatternInstruction(OpCodes.Box),
                                                         new NameOfPatternInstruction(OpCodes.Call),
                                                     }, 
#endregion
                                                 };
        private static String GetNameFromAnonymousMethod(Instruction instruction, ILProcessor ilProcessor) {
            var memberReference = (MemberReference)((MethodDefinition) instruction.Operand).Body.Instructions.Single(x => LambdaOpCodes.Contains(x.OpCode)).Operand;
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
        private delegate Boolean NameOfCallInstructionProcessor(Instruction instruction, ILProcessor ilProcessor);
        private static void ProcessNameOfCallInstruction(Instruction instruction, ILProcessor ilProcessor) {
            Boolean patternNotMatched = true;
            PatternInstruction[] patternMatched = null;
            Terminal terminal = null;
            Instruction terminalInstruction = null;
            Instruction iterator;
            var patternsOrdered = patterns.OrderByDescending(x => x.Count()); // Ordered by length of pattern to ensure longest possible patterns (of which shorter patterns might be a subset) are checked first
            foreach (var pattern in patternsOrdered) {
                iterator = instruction;
                patternNotMatched = false;
                patternMatched = null;
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
            try {
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
                String name = terminal(terminalInstruction, ilProcessor);
                ilProcessor.InsertAfter(instruction, Instruction.Create(OpCodes.Ldstr, name));
                iterator = instruction;
                for (var i = 0; i != patternMatched.Count(); ++i) {
                    var temp = iterator.Previous;
                    if (patternMatched[i] is OptionalPatternInstruction && !patternMatched[i].EligibleOpCodes.Contains(iterator.OpCode))
                        continue;
                    ilProcessor.Remove(iterator);
                    iterator = temp;
                }
            }
            catch (NotSupportedException) {
                // Fall through while developing list of instruction patterns
#if !DEBUG
                throw;
#endif
            }
        }
        private static readonly OpCode[] CallOpCodes = {OpCodes.Call, OpCodes.Calli, OpCodes.Callvirt};
        private static readonly OpCode[] LambdaOpCodes = CallOpCodes.Concat(new[] { OpCodes.Ldftn, OpCodes.Ldfld }).ToArray();
        private static readonly MethodInfo GetTypeFromHandleMethodInfo = new Func<RuntimeTypeHandle, Object>(Type.GetTypeFromHandle).Method;
        private static readonly String GetTypeFromHandleMethodSignature = String.Format("{0} {1}::{2}(", GetTypeFromHandleMethodInfo.ReturnType, GetTypeFromHandleMethodInfo.DeclaringType, GetTypeFromHandleMethodInfo.Name);
        
        private static readonly MethodInfo NameOfEventMethodInfo = new Func<Action<Object, EventHandler>, String>(Name.OfEvent).Method;
        private static readonly String NameOfEventMethodSignature = String.Format("{0} {1}::{2}", NameOfEventMethodInfo.ReturnType, NameOfEventMethodInfo.DeclaringType, NameOfEventMethodInfo.Name);
    }
}