using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualMachine.Enumerations
{
    public enum TOKEN
    {
        Null,
        Opcode,
        Register,
        Label,
        Immediate,
        Char,
        Int,
        Byte,
        DirInt,
        DirChar,
        DirByte,
        Directive,
    }
}
