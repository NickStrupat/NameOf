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
            ProcessNameOfLocalCallInstruction(instruction, ilProcessor);
        }
        private static void ProcessNameOfTypeCallInstruction(Instruction instruction, ILProcessor ilProcessor) {
            
        }
        private static void ProcessNameOfLocalCallInstruction(Instruction instruction, ILProcessor ilProcessor) {
            try {
                String name;
                if (new[] {OpCodes.Ldloc, OpCodes.Ldloc_S, OpCodes.Ldloca, OpCodes.Ldloca_S}.Contains(instruction.Previous.OpCode))
                    name = ((VariableReference) instruction.Previous.Operand).Name;
                else if (instruction.Previous.OpCode == OpCodes.Ldloc_0)
                    name = ilProcessor.Body.Variables[0].Name;
                else if (instruction.Previous.OpCode == OpCodes.Ldloc_1)
                    name = ilProcessor.Body.Variables[1].Name;
                else if (instruction.Previous.OpCode == OpCodes.Ldloc_2)
                    name = ilProcessor.Body.Variables[2].Name;
                else if (instruction.Previous.OpCode == OpCodes.Ldloc_3)
                    name = ilProcessor.Body.Variables[3].Name;
                else
                    throw new InvalidOperationException("No valid opcode found.");
                ilProcessor.InsertAfter(instruction, Instruction.Create(OpCodes.Ldstr, name));
                ilProcessor.Remove(instruction.Previous);
                ilProcessor.Remove(instruction);
            }
            catch (InvalidOperationException exception) {
            }
        }
    }
}
