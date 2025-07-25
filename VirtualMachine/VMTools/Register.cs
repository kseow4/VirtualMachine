using System;
using VirtualMachine.Enumerations;
using VirtualMachine.Interfaces;

namespace VirtualMachine.VMTools
{
    public class Register
    { 
        public REGISTER Name { get; }

        public Int32 Data { get; set; }


        public Register(int id) => Name = (REGISTER)id;
        public Register(REGISTER id) => Name = id;
    }
}
