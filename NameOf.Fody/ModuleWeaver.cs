using System;
using System.Linq;
using System.Reflection;
using System.Text;
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
        private delegate Boolean NameOfCallInstructionProcessor(Instruction instruction, ILProcessor ilProcessor);
        private static void ProcessNameOfCallInstruction(Instruction instruction, ILProcessor ilProcessor) {
            var processNameOfCallInstructionDelegates = new NameOfCallInstructionProcessor[] {
                                                                  ProcessNameOfTypeCallInstruction,
                                                                  ProcessNameOfLocalCallInstruction
                                                              };
            foreach (var processNameOfCallInstructionDelegate in processNameOfCallInstructionDelegates)
                if (processNameOfCallInstructionDelegate(instruction, ilProcessor))
                    return;
            return; // TODO: Remove this so the exception works!
            // The usage of Name.Of is not supported
            var i = instruction;
            while (i.SequencePoint == null) // Look for last sequence point
                i = i.Previous;
            throw new NotSupportedException("This usage of Name.Of is not supported. Source: " + i.SequencePoint.Document.Url + " - line " + i.SequencePoint.StartLine);
        }
        private static readonly MethodInfo GetTypeFromHandleMethodInfo = new Func<RuntimeTypeHandle, Object>(Type.GetTypeFromHandle).Method;
        private static readonly String GetTypeFromHandleMethodSignature = String.Format("{0} {1}::{2}(", GetTypeFromHandleMethodInfo.ReturnType, GetTypeFromHandleMethodInfo.DeclaringType, GetTypeFromHandleMethodInfo.Name);
        private static Boolean ProcessNameOfTypeCallInstruction(Instruction instruction, ILProcessor ilProcessor) {
            var genericInstanceMethod = instruction.Operand as GenericInstanceMethod;
            if (genericInstanceMethod == null) {
                var previous = instruction.Previous;
                if (previous != null && previous.OpCode == OpCodes.Call && previous.Operand is MethodReference && ((MethodReference) previous.Operand).FullName.StartsWith(GetTypeFromHandleMethodSignature)) {
                    if (previous.Previous != null && previous.Previous.OpCode == OpCodes.Ldtoken) {
                        String name = ((TypeDefinition)previous.Previous.Operand).Name;
                        ilProcessor.InsertAfter(instruction, Instruction.Create(OpCodes.Ldstr, name));
                        ilProcessor.Remove(instruction.Previous.Previous);
                        ilProcessor.Remove(instruction.Previous);
                        ilProcessor.Remove(instruction);
                        return true;
                    }
                }
            }
            else if (genericInstanceMethod.Parameters.Count == 0 && genericInstanceMethod.GenericArguments.Count == 1) {
                String name = genericInstanceMethod.GenericArguments[0].Name;
                ilProcessor.InsertAfter(instruction, Instruction.Create(OpCodes.Ldstr, name));
                ilProcessor.Remove(instruction);
                return true;
            }
            return false;
        }
        private static Boolean ProcessNameOfLocalCallInstruction(Instruction instruction, ILProcessor ilProcessor) {
            if (instruction.Previous == null)
                return false;
            var isBoxed = instruction.Previous.OpCode == OpCodes.Box;
            var potentialLoadLocalInstruction = isBoxed ? instruction.Previous.Previous : instruction.Previous;
            String name;
            if (new[] {OpCodes.Ldloc, OpCodes.Ldloc_S, OpCodes.Ldloca, OpCodes.Ldloca_S}.Contains(potentialLoadLocalInstruction.OpCode))
                name = ((VariableReference) potentialLoadLocalInstruction.Operand).Name;
            else if (potentialLoadLocalInstruction.OpCode == OpCodes.Ldloc_0)
                name = ilProcessor.Body.Variables[0].Name;
            else if (potentialLoadLocalInstruction.OpCode == OpCodes.Ldloc_1)
                name = ilProcessor.Body.Variables[1].Name;
            else if (potentialLoadLocalInstruction.OpCode == OpCodes.Ldloc_2)
                name = ilProcessor.Body.Variables[2].Name;
            else if (potentialLoadLocalInstruction.OpCode == OpCodes.Ldloc_3)
                name = ilProcessor.Body.Variables[3].Name;
            else
                return false;
            ilProcessor.InsertAfter(instruction, Instruction.Create(OpCodes.Ldstr, name));
            if (isBoxed)
                ilProcessor.Remove(instruction.Previous.Previous);
            ilProcessor.Remove(instruction.Previous);
            ilProcessor.Remove(instruction);
            return true;
        }
    }
}