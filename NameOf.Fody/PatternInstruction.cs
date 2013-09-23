using System;
using Mono.Cecil.Cil;

namespace NameOf.Fody {
    class PatternInstruction {
        public OpCode[] EligibleOpCodes { get; private set; }
        public Func<Instruction, ILProcessor, String> TerminalFunc { get; private set; }
        public PatternInstruction(OpCode[] eligibleOpCodes, Func<Instruction, ILProcessor, String> terminalFunc = null, Func<Instruction, ILProcessor, Boolean> condition = null) {
            EligibleOpCodes = eligibleOpCodes;
            TerminalFunc = terminalFunc;
        }
        public PatternInstruction(OpCode opCode, Func<Instruction, ILProcessor, String> terminalFunc = null, Func<Instruction, ILProcessor, Boolean> condition = null) : this(new[] { opCode }, terminalFunc, condition) { }
    }
    class OptionalPatternInstruction : PatternInstruction {
        public OptionalPatternInstruction(OpCode[] eligibleOpCodes, Func<Instruction, ILProcessor, String> terminalFunc = null) : base(eligibleOpCodes, terminalFunc) { }
        public OptionalPatternInstruction(OpCode opCode, Func<Instruction, ILProcessor, String> terminalFunc = null) : base(opCode, terminalFunc) { }
    }
    class NameOfPatternInstruction : PatternInstruction {
        public NameOfPatternInstruction(OpCode[] eligibleOpCodes, Func<Instruction, ILProcessor, String> terminalFunc = null) : base(eligibleOpCodes, terminalFunc) {}
        public NameOfPatternInstruction(OpCode opCode, Func<Instruction, ILProcessor, String> terminalFunc = null) : base(opCode, terminalFunc) {}
    }
}
