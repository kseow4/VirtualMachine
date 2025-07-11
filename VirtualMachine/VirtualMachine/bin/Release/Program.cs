using System;
using System.IO;

namespace VirtualMachine
{
    public static class Program
    {
        public static void Main(params string[] args)
        {
            try
            {
                VirtualMachine vm = new VirtualMachine(args);
                //  vm.Assembler.Run(args.Length > 1 ? args[1] : "Resources/test_asm.txt");

                vm.Run();
                vm.GetLogs();
            }
            catch (Exception e)
            {
                using (StreamWriter ofile = new StreamWriter(@".\error.txt")) { ofile.WriteLine(e.Message.ToString()); }
            }
        }
    }
}
