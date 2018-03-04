using System;
using System.Configuration;
using System.Threading;
using Messages = FileSorter.Resources.Messages;
using FileSorter.Configuration;

namespace FileSorter
{
    class Program
    {
        public static void Main()
        {
            SimpleConfigurationSection сonfig = (SimpleConfigurationSection)
                ConfigurationManager.GetSection("simpleSection");

            //Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU");
            Thread.CurrentThread.CurrentCulture = сonfig.Culture.cultureInfo;
            Thread.CurrentThread.CurrentUICulture = сonfig.Culture.cultureInfo;

            Console.WriteLine(Messages.LoadSettingsMsg);

            Console.WriteLine(Messages.UploadedRulesMsg);
            foreach (RuleElement rule in сonfig.Rules)
                Console.WriteLine(Messages.RuleDirMsg, rule.Rule, rule.TargetFolder);

            Console.WriteLine(Messages.DirsWatchMsg);
            foreach (ViewedDirElement viewedDir in сonfig.ViewedDirs)
                Console.WriteLine(viewedDir.Dir);

            Console.WriteLine(Messages.DefaultDirMoveMsg, сonfig.DefaultDestinationDir.Dir);

            Sorter sorter = new Sorter(сonfig);

            sorter.Start += EventStartMessages;
            sorter.ActivateDirWatcher += EventActivateDirWatcherMessages;
            sorter.FileDetected += EventFileDetectedMessages;
            sorter.RuleFound += EventRuleFoundMessages;
            sorter.FileMoved += EventFileMovedMessages;

            sorter.StartProcess();

            Console.WriteLine(Messages.ForExitMsg, DateTime.Now.ToString());
            while (true) ;

        }

        private static void EventStartMessages()
        {
            Console.WriteLine(Messages.StartMsg, DateTime.Now.ToString());
        }

        private static void EventActivateDirWatcherMessages(string sourceDir)
        {
            Console.WriteLine(Messages.ActivateDirWatcherMsg, DateTime.Now.ToString(), sourceDir);
        }

        private static void EventFileDetectedMessages(string fileFullPath)
        {
            Console.WriteLine(Messages.FileDetectedMsg, DateTime.Now.ToString(), fileFullPath);
        }

        private static void EventRuleFoundMessages(bool isFinded, string fileName)
        {
            if (isFinded)
                Console.WriteLine(Messages.RuleFoundMsg, DateTime.Now.ToString(), fileName);
            else
                Console.WriteLine(Messages.RuleNotFoundMsg, DateTime.Now.ToString(), fileName);
        }

        private static void EventFileMovedMessages(bool isMoved, string sourceFileFullPath, string targetFileFullPath)
        {
            if (isMoved)
                Console.WriteLine(Messages.FileMovedMsg, DateTime.Now.ToString(), sourceFileFullPath, targetFileFullPath);
            else
                Console.WriteLine(Messages.FileNotMovedMsg, DateTime.Now.ToString(), sourceFileFullPath);
        }
    }
}
