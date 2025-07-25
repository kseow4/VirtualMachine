using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualMachine.Enumerations
{
    [Description("Operation Code")]
    public enum OPCODE
    {
        NONE,
        // Jump Instructions
        JMP,
        JMR,
        BNZ,
        BGT,
        BLT,
        BRZ,
        // Move Instructions
        MOV,
        LDA,
        STR,
        LDR,
        STB,
        LDB,
        // Arithmetic Instructions
        ADD,
        ADI,
        SUB,
        MUL,
        DIV,
        // Logical Instructions
        AND,
        OR,
        // Compare Instructions
        CMP,
        // Traps
        TRP
    }
}
