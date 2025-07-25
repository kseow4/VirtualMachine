using System;
using System.IO;
using VirtualMachine.VMTools;

namespace VirtualMachine
{
    public static class Program
    {
        public static void Main(params string[] args)
        {
            try
            {
                VirtualMachine vm = new VirtualMachine(args.Length > 1 ? args[1] : "Resources/proj4.asm");
                //  vm.Assembler.Run(args.Length > 1 ? args[1] : "Resources/test_asm.txt");

                vm.Run();
                LoggingService.WriteLogs();
            }
            catch (Exception)
            {
                LoggingService.WriteLogs();
               //using (StreamWriter ofile = new StreamWriter(@".\error.txt")) { ofile.WriteLine(e.Message.ToString()); }
            }
        }
    }
}
