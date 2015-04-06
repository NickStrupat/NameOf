using System.Collections.Generic;
using Mono.Cecil.Cil;

namespace NameOf.Fody
{
    public static class InstructionExtensions
    {
        public static IEnumerable<Instruction> AsReverseEnumerable(this Instruction instruction)
        {
            yield return instruction;

            while (instruction.Previous != null)
            {
                yield return instruction = instruction.Previous;
            }
        }
    }
}