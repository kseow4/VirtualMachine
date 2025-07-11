using System;
using VirtualMachine.Enumerations;

namespace VirtualMachine.Assembly
{
    public class InstructionSet
    {
        //string Label { get; set; } = null;
        //object Instruction { get; set; } = null;
        //object Operand1 { get; set; } = null;
        //object Operand2 { get; set; } = null;

        //public InstructionSet(string label = null, object instruction = null, object op1 = null, object op2 = null) =>
        //    (Label, Instruction, Operand1, Operand2) = (label, instruction, op1, op2);

        //public InstructionSet(OPCODE opcode, object operand1 = null, object operand2 = null)
        //{

        //}

        public Enum InstructionType { get; }
        public object Operand1 { get; }
        public object Operand2 { get; }

        public InstructionSet(Enum instructionType, object operand1 = null, object operand2 = null) => (InstructionType, Operand1, Operand2) = (instructionType, operand1, operand2);



        //public bool IsSyntactic()
        //{
        //    if (Label is string)
        //    {

        //    }



        //    switch ((Label, Instruction, Operand1, Operand2))
        //    {
        //        case (null, OPCODE a, REGISTER r, REGISTER b) when
        //            break;


        //    }
        //}



        //protected object[] Tokens { get; }

        //public string Label { get; set; }

        //public InstructionSet this[object[] tokens]
        //{
        //    get { return GetInstructionSet(tokens); }
        //}

        //public InstructionSet(params object[] tokens)
        //{
        //    Tokens = tokens;
        //}

        //public InstructionSet GetInstructionSet(object[] tokens)
        //{ 
        //    switch (tokens.Length)
        //    {
        //        case 1:
        //            if ((DIRECTIVE)tokens[0] == DIRECTIVE.ALN)
        //            {
        //                // I don't know what ALN does, but checking case since it can be solo.
        //            }
        //            break;

        //        case 2:
        //            if (IsOperation(tokens[0]))
        //            {
        //                if (IsRegister(tokens[1])) { return new OperationInstruction((OPCODE)tokens[0], (REGISTER)tokens[1]); }
        //                else if (IsImmediate(tokens[1])) { return new OperationInstruction((OPCODE)tokens[0], (Int32)tokens[1]); }
        //                else if (IsLabel(tokens[1])) { return new OperationInstruction((OPCODE)tokens[0], (String)tokens[1]); }
        //            }
        //            else if (IsDirective(tokens[0]))
        //            {

        //              //  return new DirectiveInstruction(Tokens);
        //            }
        //            break;
        //            throw new InvalidOperationException();

        //        case 3:
        //            if (IsLabel(tokens[0]))
        //            {

        //            }
        //            else if (IsOperation(tokens[0]))
        //            {

        //            }
        //            break;

        //        default:
        //            throw new InvalidOperationException();

        //    }
        //    throw new InvalidCastException();
        //}

        private bool IsOperation(object o) => Enum.IsDefined(typeof(OPCODE), o);
        private bool IsDirective(object o) => Enum.IsDefined(typeof(DIRECTIVE), o);
        private bool IsRegister(object o) => Enum.IsDefined(typeof(REGISTER), o);
        private bool IsImmediate(object o) => o.GetType() == typeof(byte) || o.GetType() == typeof(Int32);
        private bool IsLabel(object o) => o.GetType() == typeof(string);

        //internal class OperationInstruction : InstructionSet
        //{
        //    private OPCODE Opcode;
        //    private REGISTER D_Register;
        //    private REGISTER S_Register;

        //    public OperationInstruction(OPCODE opcode, REGISTER register)
        //    {
        //        Opcode = opcode;
        //        D_Register = register;
        //    }

        //    public OperationInstruction(OPCODE opcode, object immediate)
        //    {
        //        switch (immediate.GetType().Name)
        //        {
        //            case "char":
        //                break;


        //        }
        //    }

        //    public OperationInstruction(OPCODE opcode, byte immediate)
        //    {

        //    }



        //    public OperationInstruction(OPCODE opcode, REGISTER sourceRegister, REGISTER destinationRegister)
        //    {

        //    }

        //    public OperationInstruction(string label, OPCODE opcode, REGISTER register)
        //    {

        //    }


        //}

        //internal class DirectiveInstruction : InstructionSet
        //{
        //    public DirectiveInstruction(object[] tokens)// : base(tokens)
        //    {

        //    }
        //}


    }
}
