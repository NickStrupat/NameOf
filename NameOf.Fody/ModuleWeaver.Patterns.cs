using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace NameOf.Fody {
    partial class ModuleWeaver {
        private static PatternInstruction[][] nameOfCallPatterns = {
            #region Instance
		    new [] {
                new PatternInstruction(LoadOpCodes),
                new PatternInstruction(OpCodes.Ldfld),
                new PatternInstruction(OpCodes.Ldfld, (i, p) => ((FieldReference)i.Operand).Name),
                new OptionalPatternInstruction(OpCodes.Box),
                new NameOfPatternInstruction(),
            },
		    new [] {
                new PatternInstruction(LoadOpCodes),
                new PatternInstruction(OpCodes.Ldfld),
                new PatternInstruction(CallOpCodes, (i, p) => ((MethodDefinition)i.Operand).Name.Substring(4), (i, p) => ((MethodDefinition)i.Operand).IsGetter),
                new OptionalPatternInstruction(OpCodes.Box),
                new NameOfPatternInstruction(),
            },
		    new [] {
                new PatternInstruction(LoadOpCodes),
                new PatternInstruction(OpCodes.Ldfld),
                new PatternInstruction(OpCodes.Ldftn, (i, p) => ((MethodReference)i.Operand).Name),
                new OptionalPatternInstruction(OpCodes.Newobj),
                new NameOfPatternInstruction(),
            },
		    new [] {
                new PatternInstruction(LoadOpCodes),
                new PatternInstruction(OpCodes.Ldftn, (i, p) => GetNameFromAnonymousMethod(i, p, CallOpCodes), (i, p) => ContainsOpCodes(i, p, CallOpCodes)),
                new OptionalPatternInstruction(OpCodes.Newobj),
                new NameOfPatternInstruction(),
            },
            #endregion
            #region Enum Value
		    new [] {
                new PatternInstruction(new []{OpCodes.Ldc_I4, OpCodes.Ldc_I4_S, OpCodes.Ldc_I8}),
                new PatternInstruction(OpCodes.Box, (i, p) => ((TypeDefinition)i.Operand).Fields.Single(x => (x.Constant ?? String.Empty).ToString() == i.Previous.Operand.ToString()).Name, (i, p) => ((TypeDefinition)i.Operand).IsEnum),
                new NameOfPatternInstruction(),
            },
		    new [] {
                new PatternInstruction(new []{OpCodes.Ldc_I4_M1, OpCodes.Ldc_I4_0, OpCodes.Ldc_I4_1, OpCodes.Ldc_I4_2, OpCodes.Ldc_I4_3, OpCodes.Ldc_I4_4, OpCodes.Ldc_I4_5, OpCodes.Ldc_I4_6, OpCodes.Ldc_I4_7, OpCodes.Ldc_I4_8}),
                new PatternInstruction(OpCodes.Box, (i, p) => ((TypeDefinition)i.Operand).Fields.Single(x => (Int32?)x.Constant == GetNumberAtEndOf(i.Previous.OpCode)).Name, (i, p) => ((TypeDefinition)i.Operand).IsEnum),
                new NameOfPatternInstruction(),
            },
            #endregion
            #region Type
		    new [] {
                new PatternInstruction(OpCodes.Ldtoken, (i, p) => ((TypeDefinition)i.Operand).Name),
                new PatternInstruction(OpCodes.Call, null, (i, p) => ((MethodReference)i.Operand).FullName.StartsWith(GetTypeFromHandleMethodSignature)),
                new NameOfPatternInstruction(),
            },
		    new PatternInstruction[] {
                new NameOfPatternInstruction(terminal: (i, p) => ((GenericInstanceMethod)i.Operand).GenericArguments[0].Name, predicate: (i, p) => i.Operand is GenericInstanceMethod),
            },
            #endregion
            #region Local
		    new [] {
                new PatternInstruction(OpCodes.Ldloc_0, (i, p) => p.Body.Variables[0].Name),
                new OptionalPatternInstruction(OpCodes.Box),
                new NameOfPatternInstruction(),
            },
		    new [] {
                new PatternInstruction(OpCodes.Ldloc_1, (i, p) => p.Body.Variables[1].Name),
                new OptionalPatternInstruction(OpCodes.Box),
                new NameOfPatternInstruction(),
            },
		    new [] {
                new PatternInstruction(OpCodes.Ldloc_2, (i, p) => p.Body.Variables[2].Name),
                new OptionalPatternInstruction(OpCodes.Box),
                new NameOfPatternInstruction(),
            },
		    new [] {
                new PatternInstruction(OpCodes.Ldloc_3, (i, p) => p.Body.Variables[3].Name),
                new OptionalPatternInstruction(OpCodes.Box),
                new NameOfPatternInstruction(),
            },
		    new [] {
                new PatternInstruction(new[] {OpCodes.Ldloc, OpCodes.Ldloc_S}, (i, p) => ((VariableReference)i.Operand).Name),
                new OptionalPatternInstruction(OpCodes.Box),
                new NameOfPatternInstruction(),
            },
            #endregion
            #region Member
		    new [] {
                new PatternInstruction(OpCodes.Ldsfld, i => CheckIfFieldIsUnused(i)),
                new PatternInstruction(OpCodes.Brtrue_S),
                new PatternInstruction(OpCodes.Ldnull),
                new PatternInstruction(OpCodes.Ldftn, (i, p) => GetNameFromAnonymousMethod(i, p, LambdaOpCodes), (i, p) => ContainsOpCodes(i, p, LambdaOpCodes)),
                new PatternInstruction(OpCodes.Newobj),
                new PatternInstruction(OpCodes.Stsfld),
                new PatternInstruction(OpCodes.Br_S),
                new PatternInstruction(OpCodes.Ldsfld),
                new NameOfPatternInstruction(),
            },
		    new [] {
                new PatternInstruction(OpCodes.Ldnull), 
                new PatternInstruction(OpCodes.Ldftn, (i, p) => ((MethodReference)i.Operand).Name),
                new PatternInstruction(OpCodes.Newobj),
                new NameOfPatternInstruction(),
            },
		    new [] {
                new PatternInstruction(OpCodes.Ldsfld, (i, p) => ((FieldReference)i.Operand).Name),
                new OptionalPatternInstruction(OpCodes.Box),
                new NameOfPatternInstruction(),
            },
		    new [] {
                new PatternInstruction(OpCodes.Call, (i, p) => ((MethodDefinition)i.Operand).Name.Substring(4), (i, p) => ((MethodDefinition)i.Operand).IsGetter),
                new OptionalPatternInstruction(OpCodes.Box),
                new NameOfPatternInstruction(),
            },
            #endregion
            #region Arguments
		    new [] {
                new PatternInstruction(OpCodes.Ldarg_0, (i, p) => p.Body.Method.Parameters[0].Name),
                new OptionalPatternInstruction(OpCodes.Box),
                new NameOfPatternInstruction(),
            },
            new [] {
                new PatternInstruction(OpCodes.Ldarg_1, (i, p) => p.Body.Method.Parameters[1].Name),
                new OptionalPatternInstruction(OpCodes.Box),
                new NameOfPatternInstruction(),
            },
            new [] {
                new PatternInstruction(OpCodes.Ldarg_2, (i, p) => p.Body.Method.Parameters[2].Name),
                new OptionalPatternInstruction(OpCodes.Box),
                new NameOfPatternInstruction(),
            },
            new [] {
                new PatternInstruction(OpCodes.Ldarg_3, (i, p) => p.Body.Method.Parameters[3].Name),
                new OptionalPatternInstruction(OpCodes.Box),
                new NameOfPatternInstruction(),
            },
            new [] {
                new PatternInstruction(new [] {OpCodes.Ldarg, OpCodes.Ldarg_S}, (i, p) => ((ParameterReference)i.Operand).Name),
                new OptionalPatternInstruction(OpCodes.Box),
                new NameOfPatternInstruction(),
            }, 
            #endregion
        };
        private static PatternInstruction[][] lambdaPatterns = {
            new [] {
                new PatternInstruction(OpCodes.Ldarg_0),
                new PatternInstruction(OpCodes.Ldarg_1),
                new PatternInstruction(OpCodes.Callvirt, (i, p) => String.Empty),
                new OptionalPatternInstruction(OpCodes.Nop), // This gets emitted into debug builds
                new PatternInstruction(OpCodes.Ret),
            }, 
        };
    }
}
