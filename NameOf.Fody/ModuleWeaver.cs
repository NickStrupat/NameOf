using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace NameOf.Fody
{
    public class ModuleWeaver
    {
        public ModuleDefinition ModuleDefinition { get; set; }
        public void Execute() {
            foreach (var typeDefinition in ModuleDefinition.GetTypes())
                foreach (var methodDefinition in typeDefinition.Methods.Where(x => x.HasBody))
                    ProcessMethod(methodDefinition);
            // TODO: Remove Name.Of assembly and all references to it
        }
        private static readonly MethodInfo NameOfMethodInfo = new Func<Object, String>(Name.Of).Method;
        private static readonly String NameOfMethodSignature = String.Format("{0} {1}::{2}", NameOfMethodInfo.ReturnType, NameOfMethodInfo.DeclaringType, NameOfMethodInfo.Name);
        private static Boolean IsNameOfCallInstruction(Instruction instruction) {
            return instruction.OpCode == OpCodes.Call && ((MethodReference)instruction.Operand).FullName.StartsWith(NameOfMethodSignature);
        }
        private static void ProcessMethod(MethodDefinition methodDefinition) {
            var nameOfCallInstructions = methodDefinition.Body.Instructions.Where(IsNameOfCallInstruction).ToList();
            var methodBodyIlProcessor = methodDefinition.Body.GetILProcessor();
            foreach (var nameOfCallInstruction in nameOfCallInstructions)
                ProcessNameOfCallInstruction(nameOfCallInstruction, methodBodyIlProcessor);
        }
        private static void ProcessNameOfCallInstruction(Instruction instruction, ILProcessor ilProcessor) {
            ProcessNameOfTypeCallInstruction(instruction, ilProcessor);
        }
        private static void ProcessNameOfTypeCallInstruction(Instruction instruction, ILProcessor ilProcessor) {
            
        }
    }
}
