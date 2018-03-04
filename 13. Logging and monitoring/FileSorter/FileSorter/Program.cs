using System;
using System.Configuration;
using System.Threading;
using Messages = FileSorter.Resources.Messages;
using FileSorter.Configuration;
using NLog;
using System.Diagnostics;

namespace FileSorter
{
    class Program
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public static void Main()
        {
            CounterInitialize(); 

            SimpleConfigurationSection сonfig = (SimpleConfigurationSection)
            ConfigurationManager.GetSection("simpleSection");

            //Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU");
            Thread.CurrentThread.CurrentCulture = сonfig.Culture.cultureInfo;
            Thread.CurrentThread.CurrentUICulture = сonfig.Culture.cultureInfo;

            MyLogger.WriteLog(_logger, LogLevel.Info, string.Format(Messages.LoadSettingsMsg));

            MyLogger.WriteLog(_logger, LogLevel.Info, Messages.UploadedRulesMsg);
            foreach (RuleElement rule in сonfig.Rules)
                MyLogger.WriteLog(_logger, LogLevel.Info, string.Format(Messages.RuleDirMsg, rule.Rule, rule.TargetFolder));

            MyLogger.WriteLog(_logger, LogLevel.Info, Messages.DirsWatchMsg);
            foreach (ViewedDirElement viewedDir in сonfig.ViewedDirs)
                MyLogger.WriteLog(_logger, LogLevel.Info, viewedDir.Dir);

            MyLogger.WriteLog(_logger, LogLevel.Info, string.Format(Messages.DefaultDirMoveMsg, сonfig.DefaultDestinationDir.Dir));

            Sorter sorter = new Sorter(сonfig);

            sorter.Start += EventStartMessages;
            sorter.ActivateDirWatcher += EventActivateDirWatcherMessages;
            sorter.FileDetected += EventFileDetectedMessages;
            sorter.RuleFound += EventRuleFoundMessages;
            sorter.FileMoved += EventFileMovedMessages;

            sorter.StartProcess();

            MyLogger.WriteLog(_logger, LogLevel.Info, Messages.ForExitMsg);
            while (true) ;
        }

        private static void CounterInitialize()
        {
            try { PerformanceCounterCategory.Delete("FileSorter"); } catch (Exception) { }

            CounterCreationDataCollection counters = new CounterCreationDataCollection();

            CounterCreationData fileDetectedCount = new CounterCreationData()
            {
                CounterName = "FileDetectedCount",
                CounterType = PerformanceCounterType.NumberOfItems32
            };
            CounterCreationData ruleFoundCount = new CounterCreationData()
            {
                CounterName = "RuleFoundCount",
                CounterType = PerformanceCounterType.NumberOfItems32
            };
            CounterCreationData fileMovedCount = new CounterCreationData()
            {
                CounterName = "FileMovedCount",
                CounterType = PerformanceCounterType.NumberOfItems32
            };

            counters.Add(fileDetectedCount);
            counters.Add(ruleFoundCount);
            counters.Add(fileMovedCount);

            try
            {
                PerformanceCounterCategory.Create(
                    "FileSorter", "...",
                    PerformanceCounterCategoryType.MultiInstance, counters);
            }
            catch (Exception) { }

            using (var counter = new PerformanceCounter("FileSorter", "FileDetectedCount", "FileSorter project", false))
            {
                counter.RawValue = 0;
            }
            using (var counter = new PerformanceCounter("FileSorter", "RuleFoundCount", "FileSorter project", false))
            {
                counter.RawValue = 0;
            }
            using (var counter = new PerformanceCounter("FileSorter", "FileMovedCount", "FileSorter project", false))
            {
                counter.RawValue = 0;
            }
        }

        private static void EventStartMessages()
        {
            MyLogger.WriteLog(_logger, LogLevel.Info, Messages.StartMsg);
        }

        private static void EventActivateDirWatcherMessages(string sourceDir)
        {
            MyLogger.WriteLog(_logger, LogLevel.Info, string.Format(Messages.ActivateDirWatcherMsg, sourceDir));
        }

        private static void EventFileDetectedMessages(string fileFullPath)
        {
            MyLogger.WriteLog(_logger, LogLevel.Debug, string.Format(Messages.FileDetectedMsg, fileFullPath));
            using (var logInCounter = new PerformanceCounter("FileSorter", "FileDetectedCount", "FileSorter project", false))
            {
                logInCounter.Increment();
            }
        }

        private static void EventRuleFoundMessages(bool isFinded, string fileName)
        {
            if (isFinded)
            {
                MyLogger.WriteLog(_logger, LogLevel.Debug, string.Format(Messages.RuleFoundMsg, fileName));
                using (var counter = new PerformanceCounter("FileSorter", "RuleFoundCount", "FileSorter project", false))
                {
                    counter.Increment();
                }
            }
            else
                MyLogger.WriteLog(_logger, LogLevel.Warn, string.Format(Messages.RuleNotFoundMsg, fileName));
        }

        private static void EventFileMovedMessages(bool isMoved, string sourceFileFullPath, string targetFileFullPath)
        {
            if (isMoved)
            {
                MyLogger.WriteLog(_logger, LogLevel.Debug, string.Format(Messages.FileMovedMsg, sourceFileFullPath, targetFileFullPath));
                using (var counter = new PerformanceCounter("FileSorter", "FileMovedCount", "FileSorter project", false))
                {
                    counter.Increment();
                }
            }
            else
                MyLogger.WriteLog(_logger, LogLevel.Warn, string.Format(Messages.FileNotMovedMsg, sourceFileFullPath));
        }
    }
}
