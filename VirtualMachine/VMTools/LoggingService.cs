using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualMachine.Interfaces;

namespace VirtualMachine.VMTools
{
    public sealed class LoggingService
    {
        public readonly List<ILogger> Loggers;
        private LoggingService() => Loggers = new List<ILogger>();
        public static LoggingService Logging { get { lock (loglock) { return LoggingServiceInstance.loggingService.Value; } } }

        private static readonly object loglock = new object();
        private class LoggingServiceInstance
        {
            static LoggingServiceInstance() { }
            internal static readonly Lazy<LoggingService> loggingService = new Lazy<LoggingService>(() => new LoggingService());
        }
        public static void WriteLogs(string file = null) => Logging.Loggers.ForEach(logger => logger.LogToFile(file));
    }
}
