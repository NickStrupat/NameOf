using System;
using System.Linq;
using Mono.Cecil.Cil;

namespace NameOf.Fody {
    public delegate String Terminal(Instruction instruction, ILProcessor ilProcessor);
    public delegate Boolean Predicate(Instruction instruction, ILProcessor ilProcessor);
    class PatternInstruction {
        public OpCode[] EligibleOpCodes { get; private set; }
        public Terminal Terminal { get; private set; }
        private readonly Predicate predicate;
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
                throw new ArgumentNullException();
            if (eligibleOpCodes.Count() == 0)
                throw new ArgumentException();
            EligibleOpCodes = eligibleOpCodes;
            Terminal = terminal;
            this.predicate = predicate ?? PredicateDummy;
        }
        public PatternInstruction(OpCode opCode, Terminal terminal = null, Predicate predicate = null) : this(new[] { opCode }, terminal, predicate) { }
    }
    class OptionalPatternInstruction : PatternInstruction {
        public OptionalPatternInstruction(OpCode[] eligibleOpCodes, Terminal terminal = null, Predicate predicate = null) : base(eligibleOpCodes, terminal, predicate) { }
        public OptionalPatternInstruction(OpCode opCode, Terminal terminal = null, Predicate predicate = null) : base(opCode, terminal, predicate) { }
    }
    class NameOfPatternInstruction : PatternInstruction {
        public NameOfPatternInstruction(OpCode[] eligibleOpCodes, Terminal terminal = null, Predicate predicate = null) : base(eligibleOpCodes, terminal, predicate) { }
        public NameOfPatternInstruction(OpCode opCode, Terminal terminal = null, Predicate predicate = null) : base(opCode, terminal, predicate) { }
    }
}
;