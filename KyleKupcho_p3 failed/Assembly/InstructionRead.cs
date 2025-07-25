using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualMachine.Enumerations;

namespace VirtualMachine.Assembly
{
    //    public class Operation
    //    {
    //        public OPCODE Opcode { get; }
    //        public Int32 OpA { get; set; }
    //        public Int32 OpB { get; set; }
    //        public string Label { get; set; }
    //        public Operation(OPCODE opcode, Int32 opA, Int32 opB = -1, string label = null) => (Opcode, OpA, OpB, Label) = (opcode, opA, opB, label);
    //    }
    public class InstructionRead
    {
        public void InstructionSelector(byte[] byteset)
        {
            
        }

        private static VirtualMachine VirtualMachine;
        public InstructionRead(VirtualMachine vm) => VirtualMachine = vm;

        //public void InstructionSelector(Int32 opcode, Int32 operand1, Int32 operand2)
        //{
        //    if (((OPCODE)opcode) == OPCODE.ADD) { }
        //    switch((OPCODE)opcode)
        //    {
        //        case OPCODE.JMP: JMP(operand1); break;
        //        case OPCODE.JMR: JMR(); break;
        //    }


        //}

        public static void JMP(byte label) { /*VirtualMachine.PC.Value = BitConverter.ToInt32((byte[])label, 0);*/ }
        public static void JMR() { }
        public static void BNZ() { }
        public static void BGT() { }
        public static void BLT() { }
        public static void BRZ() { }
        public static void MOV() { }
        public static void LDA() { }
        public static void STR() { }
        public static void LDR() { }
        public static void STB() { }
        public static void LDB() { }
        public static void ADD() { }
        public static void ADI() { }
        public static void SUB() { }
        public static void MUL() { }
        public static void DIV() { }
        public static void AND() { }
        public static void OR() { }
        public static void CMP() { }
        public static void TRP() { }

        //public static Action ASM_Instruction(OPCODE opcode)
        //{
        //    switch (opcode)
        //    {
        //        case OPCODE.JMP:
        //            break;

        //            case OPCODE.
        //    }
        //}

    }
}
