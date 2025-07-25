using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using VirtualMachine.Enumerations;
using VirtualMachine.Interfaces;
using static VirtualMachine.VMTools.LoggingService;
using static VirtualMachine.VMTools.VM_Extensions;

namespace VirtualMachine.VMTools
{
    public class Logger : ILogger
    {
        public Logger() => Logging.Loggers.Add(this);

        public List<string> Exceptions { get; } = new List<string>();

        public List<string> Warnings { get; } = new List<string>();

        public List<string> Messages { get; } = new List<string>();

        public void Log(string msg, LOG log)
        {
            switch (log)
            {
                case LOG.EXCEPTION: Exceptions.Add(msg); break;
                case LOG.WARNING: Warnings.Add(msg); break;
                case LOG.MESSAGE: Messages.Add(msg); break;
            }
        }

        public void LogException(string msg) => Exceptions.Add(msg);
        public void LogWarning(string msg) => Warnings.Add(msg);
        public void LogMessage(string msg) => Messages.Add(msg);
        public int Count(params LOG[] logtypes)
        {
            int count = 0;
            List<LOG> types = new List<LOG>();
            foreach (LOG item in logtypes)
            {
                if (types.Contains(item)) { continue; }
                types.Add(item);
                switch (item)
                {
                    case LOG.EXCEPTION: count += Exceptions.Count; break;
                    case LOG.WARNING: count += Warnings.Count; break;
                    case LOG.MESSAGE: count += Messages.Count; break;
                    default: count += Exceptions.Count + Warnings.Count + Messages.Count; break;
                }
            }
            return count;
        }

        public void LogToFile(string file = null)
        {
            System.IO.Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"/Logs/");
            using (StreamWriter logWriter = new StreamWriter(file
                ?? $@".\Logs\Log_{DateTime.Now:dddd-dd-MMMM-yyyy}_{Guid.NewGuid()}.log"))
            {
                logWriter.Flush();
                foreach (string exception in Exceptions) logWriter.WriteLineAsync(exception);
                foreach (string warning in Warnings) logWriter.WriteLineAsync(warning);
                foreach (string message in Messages) logWriter.WriteLineAsync(message);
            }
        }
    }
}

