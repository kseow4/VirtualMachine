using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualMachine.Enumerations;

namespace VirtualMachine.Interfaces
{
    public interface ILogger
    {
        List<string> Exceptions { get; }

        List<string> Warnings { get; }

        List<string> Messages { get; }

        void LogToFile(string file);
        void Log(string msg, LOG log);
        int Count(params LOG[] logtypes);
    }
}
