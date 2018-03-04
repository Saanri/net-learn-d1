using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSorter
{
    public static class MyLogger
    {
        public static void WriteLog(ILogger logger, LogLevel logLevel, string msg)
        {
            Console.WriteLine(msg);
            if (logLevel == LogLevel.Debug) logger.Debug(msg);
            else if (logLevel == LogLevel.Info) logger.Info(msg);
            else if (logLevel == LogLevel.Warn) logger.Warn(msg);
        }
    }
}
