using System;
using Mono.Cecil.Cil;

namespace NameOf.Fody {
    delegate String Terminal(Instruction instruction, ILProcessor ilProcessor);
    delegate Boolean Predicate(Instruction instruction, ILProcessor ilProcessor);
    class PatternInstruction {
        public OpCode[] EligibleOpCodes { get; private set; }
        public Terminal Terminal { get; private set; }
        private readonly Predicate predicate;
        public Action<Instruction> Action { get; private set; }
        public Boolean IsPredicated(Instruction instruction, ILProcessor ilProcessor) {
            try {
                return predicate(instruction, ilProcessor);
            }
            catch (Exception) {
                return false;
            }
        }
        private static Boolean PredicateDummy(Instruction instruction, ILProcessor ilProcessor) { return true; }
        public PatternInstruction(OpCode[] eligibleOpCodes, Terminal terminal = null, Predicate predicate = null) {
            if (eligibleOpCodes == null)
                throw new ArgumentNullException(Name.Of(eligibleOpCodes));
            if (eligibleOpCodes.Length == 0)
                throw new ArgumentException("Array length must be greater than zero", Name.Of(eligibleOpCodes));
            EligibleOpCodes = eligibleOpCodes;
            Terminal = terminal;
            this.predicate = predicate ?? PredicateDummy;
        }
        public PatternInstruction(OpCode opCode, Terminal terminal = null, Predicate predicate = null) : this(new[] { opCode }, terminal, predicate) { }
        public PatternInstruction(OpCode opCode, Action<Instruction> action) : this(opCode, null, null) {
            Action = action;
        }
    }
    class OptionalPatternInstruction : PatternInstruction {
        public OptionalPatternInstruction(OpCode[] eligibleOpCodes, Terminal terminal = null, Predicate predicate = null) : base(eligibleOpCodes, terminal, predicate) { }
        public OptionalPatternInstruction(OpCode opCode, Terminal terminal = null, Predicate predicate = null) : base(opCode, terminal, predicate) { }
    }
    class NameOfPatternInstruction : PatternInstruction {
        public NameOfPatternInstruction(Terminal terminal = null, Predicate predicate = null) : base(OpCodes.Call, terminal, predicate) { }
    }
}
;