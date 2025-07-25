using System;
using System.IO;

namespace VM_Runner
{
    public static class Program
    {
        public static void Main(params string[] args)
        {
            try
            {
                VirtualMachine.VirtualMachine vm = new VirtualMachine.VirtualMachine(args);
              //  vm.Assembler.Run(args.Length > 1 ? args[1] : "Resources/test_asm.txt");

             //   vm.Run();
            }
            catch (Exception e)
            {
                using (StreamWriter ofile = new StreamWriter(@".\error.txt")) { ofile.Write(e.Message.ToString()); }
            }
        }
    }
}
