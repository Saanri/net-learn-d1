using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TreeView
{
	class Program
	{
		static void Main(string[] args)
		{
			LogService logService = new LogService();
			FileSystemVisitor fileSystemVisitor0 = new FileSystemVisitor(@"C:\Users\Public\Desktop", new WorkTree(), logService);
			fileSystemVisitor0.Start += EventStartMessages;
			fileSystemVisitor0.FileFinded += EventObjectFindedMessages;
			fileSystemVisitor0.DirectoryFinded += EventObjectFindedMessages;
			fileSystemVisitor0.Finish += EventFinishMessages;

			var a = fileSystemVisitor0.ScanDir(fileSystemVisitor0.ValRootDirectory).ToList();

			logService.Print(string.Format("Нажмите любую клавишу!"));
			Console.ReadKey();

			logService.Print(string.Format("---"));
			logService.Print(string.Format("Процесс с фильтрацией"));
			logService.Print(string.Format("---"));

			FileSystemVisitor fileSystemVisitor1 = new FileSystemVisitor(@"C:\Users\Public\Desktop", new WorkTree(), logService, Filtering, false);
			fileSystemVisitor1.Start += EventStartMessages;
			fileSystemVisitor1.FileFinded += EventObjectFindedMessages;
			fileSystemVisitor1.FilteredFileFinded += EventObjectFilteredMessages;
			fileSystemVisitor1.DirectoryFinded += EventObjectFindedMessages;
			fileSystemVisitor1.FilteredDirectoryFinded += EventObjectFilteredMessages;
			fileSystemVisitor1.Finish += EventFinishMessages;

			foreach (var s in fileSystemVisitor1.ScanDir(fileSystemVisitor1.ValRootDirectory))
				System.Threading.Thread.Sleep(50);

			logService.Print(string.Format("Нажмите любую клавишу!"));
			Console.ReadKey();
		}

		private static bool Filtering(string name)
		{
			return Regex.IsMatch(name, @".*3\.lnk$");
		}

		private static void EventStartMessages(ILogService logService)
		{
			logService.Print("Процесс запущен...");
		}

		private static void EventObjectFindedMessages(FileInfo fileInfo, ILogService logService)
		{
			logService.Print(string.Format("Найден файл        : {0}", fileInfo.FullName));
		}

		private static bool EventObjectFilteredMessages(FileInfo fileInfo, ILogService logService)
		{
			logService.Print(string.Format("Файл \"{0}\" соответствует заданной фильтрации. Поиск остановлен.", fileInfo.Name));
			return true;
		}

		private static void EventObjectFindedMessages(DirectoryInfo directoryInfo, ILogService logService)
		{
			logService.Print(string.Format("Найдена директория : {0}", directoryInfo.FullName));
		}

		private static bool EventObjectFilteredMessages(DirectoryInfo directoryInfo, ILogService logService)
		{
			logService.Print(string.Format("Директория \"{0}\" соответствует заданной фильтрации. Продолжаем поиск...", directoryInfo.Name));
			return false;
		}

		private static void EventFinishMessages(ILogService logService)
		{
			logService.Print(string.Format("Процесс завершил свою работу."));
		}

	}
}
