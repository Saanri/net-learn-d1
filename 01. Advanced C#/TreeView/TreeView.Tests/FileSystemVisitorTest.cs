using NSubstitute;
using NUnit.Framework;
using System.IO;
using System.Text.RegularExpressions;

namespace TreeView.Tests
{
	[TestFixture]
	public class FileSystemVisitorTest
	{
		private IWorkTree _workTree;
		private ILogService _logService;
		private FileSystemVisitor _fileSystemVisitor;

		private DirectoryInfo _subDir1;
		private DirectoryInfo _subDir2;
		private DirectoryInfo[] _subDirs;

		private FileInfo _fileInRoot1;
		private FileInfo _fileInRoot2;
		private FileInfo _fileInRoot3;
		private FileInfo[] _filesInRoot;

		private FileInfo _fileInSubDir1_1;
		private FileInfo[] _fileInSubDir1;

		private FileInfo _fileInSubDir2_1;
		private FileInfo _fileInSubDir2_2;
		private FileInfo[] _fileInSubDir2;

		private bool _eventStartChecked = false;
		private bool _eventFileFindedChecked = false;
		private bool _eventFilteredFileFindedChecked = false;
		private bool _eventDirectoryFindedChecked = false;
		private bool _eventFilteredDirectoryFindedChecked = false;
		private bool _eventFinishChecked = false;

		[SetUp]
		public void SetUp()
		{
			_workTree = Substitute.For<IWorkTree>();
			_logService = Substitute.For<ILogService>();

			_subDir1 = new DirectoryInfo(@"C:\Test\subDir1");
			_subDir2 = new DirectoryInfo(@"C:\Test\subDir2");
			_subDirs = new DirectoryInfo[] { _subDir1, _subDir2 };

			_fileInRoot1 = new FileInfo(@"C:\Test\fileInRoot1.tst");
			_fileInRoot2 = new FileInfo(@"C:\Test\fileInRoot2.tst");
			_fileInRoot3 = new FileInfo(@"C:\Test\fileInRoot3.tst");
			_filesInRoot = new FileInfo[] { _fileInRoot1, _fileInRoot2, _fileInRoot3 };

			_fileInSubDir1_1 = new FileInfo(@"C:\Test\subDir1\fileInSubDir1_1.tst");
			_fileInSubDir1 = new FileInfo[] { _fileInSubDir1_1 };

			_fileInSubDir2_1 = new FileInfo(@"C:\Test\subDir2\fileInSubDir2_1.tst");
			_fileInSubDir2_2 = new FileInfo(@"C:\Test\subDir2\fileInSubDir2_2.tst");
			_fileInSubDir2 = new FileInfo[] { _fileInSubDir2_1, _fileInSubDir2_2 };
		}

		[Test]
		public void FileSystemVisitorWithoutFilterTest()
		{
			_fileSystemVisitor = new FileSystemVisitor(@"C:\Test", _workTree, _logService);

			_workTree.GetFiles(_fileSystemVisitor.ValRootDirectory).Returns(_filesInRoot);
			_workTree.GetFiles(_subDir1).Returns(_fileInSubDir1);
			_workTree.GetFiles(_subDir2).Returns(_fileInSubDir2);

			_workTree.GetDirectories(_fileSystemVisitor.ValRootDirectory).Returns(_subDirs);
			_workTree.GetDirectories(_subDir1).Returns(new DirectoryInfo[] { });
			_workTree.GetDirectories(_subDir2).Returns(new DirectoryInfo[] { });

			FileSystemInfo[] allMustFindObjects = new FileSystemInfo[] { _fileSystemVisitor.ValRootDirectory, _fileInRoot1, _fileInRoot2, _fileInRoot3, _subDir1, _fileInSubDir1_1, _subDir2, _fileInSubDir2_1, _fileInSubDir2_2 };

			int i = 0;
			FileSystemInfo[] FindObjects = new FileSystemInfo[9];
			//using (StreamWriter file = new StreamWriter(@"C:\Users\Public\resultTest1.txt"))
			//{
			foreach (var s in _fileSystemVisitor.ScanDir(_fileSystemVisitor.ValRootDirectory))
			{
				//file.WriteLine(s.FullName);
				Assert.AreEqual(allMustFindObjects[i], s);
				FindObjects[i] = s;
				i++;
			}
			//}

			CollectionAssert.AreEqual(allMustFindObjects, FindObjects);
			Assert.AreEqual(i, 9);
		}

		[Test]
		public void FileSystemVisitorWithFilterTest()
		{
			_fileSystemVisitor = new FileSystemVisitor(@"C:\Test", _workTree, _logService, (name) => { return Regex.IsMatch(name, @".*1\.tst$"); }, true);

			_workTree.GetFiles(_fileSystemVisitor.ValRootDirectory).Returns(_filesInRoot);
			_workTree.GetFiles(_subDir1).Returns(_fileInSubDir1);
			_workTree.GetFiles(_subDir2).Returns(_fileInSubDir2);

			_workTree.GetDirectories(_fileSystemVisitor.ValRootDirectory).Returns(_subDirs);
			_workTree.GetDirectories(_subDir1).Returns(new DirectoryInfo[] { });
			_workTree.GetDirectories(_subDir2).Returns(new DirectoryInfo[] { });

			FileSystemInfo[] allMustFindObjects = new FileSystemInfo[] { _fileInRoot1, _fileInSubDir1_1, _fileInSubDir2_1 };

			int i = 0;
			FileSystemInfo[] FindObjects = new FileSystemInfo[3];

			foreach (var s in _fileSystemVisitor.ScanDir(_fileSystemVisitor.ValRootDirectory))
			{
				Assert.AreEqual(allMustFindObjects[i], s);
				FindObjects[i] = s;
				i++;
			}

			CollectionAssert.AreEqual(allMustFindObjects, FindObjects);
			Assert.AreEqual(i, 3);
		}

		[Test]
		public void FileSystemVisitorWithFilterWithEventTest()
		{
			_fileSystemVisitor = new FileSystemVisitor(@"C:\Test", _workTree, _logService, (name) => { return Regex.IsMatch(name, @".*1\.tst$"); }, true);

			_workTree.GetFiles(_fileSystemVisitor.ValRootDirectory).Returns(_filesInRoot);
			_workTree.GetFiles(_subDir1).Returns(_fileInSubDir1);
			_workTree.GetFiles(_subDir2).Returns(_fileInSubDir2);

			_workTree.GetDirectories(_fileSystemVisitor.ValRootDirectory).Returns(_subDirs);
			_workTree.GetDirectories(_subDir1).Returns(new DirectoryInfo[] { });
			_workTree.GetDirectories(_subDir2).Returns(new DirectoryInfo[] { });

			_fileSystemVisitor.FilteredFileFinded += EventObjectFilteredMessages;

			FileSystemInfo[] allMustFindObjects = new FileSystemInfo[] { _fileInRoot1 };

			int i = 0;
			FileSystemInfo[] FindObjects = new FileSystemInfo[1];

			foreach (var s in _fileSystemVisitor.ScanDir(_fileSystemVisitor.ValRootDirectory))
			{
				Assert.AreEqual(allMustFindObjects[i], s);
				FindObjects[i] = s;
				i++;
			}

			CollectionAssert.AreEqual(allMustFindObjects, FindObjects);
			Assert.AreEqual(i, 1);
		}

		[Test]
		public void FileSystemVisitorEventsTest()
		{
			_fileSystemVisitor = new FileSystemVisitor(@"C:\Test", _workTree, _logService, (name) => { return Regex.IsMatch(name, @".*$"); }, false);

			_workTree.GetFiles(_fileSystemVisitor.ValRootDirectory).Returns(_filesInRoot);
			_workTree.GetFiles(_subDir1).Returns(_fileInSubDir1);
			_workTree.GetFiles(_subDir2).Returns(_fileInSubDir2);

			_workTree.GetDirectories(_fileSystemVisitor.ValRootDirectory).Returns(_subDirs);
			_workTree.GetDirectories(_subDir1).Returns(new DirectoryInfo[] { });
			_workTree.GetDirectories(_subDir2).Returns(new DirectoryInfo[] { });

			_fileSystemVisitor.Start += EventStartMessages;
			_fileSystemVisitor.FileFinded += EventObjectFindedMessages;
			_fileSystemVisitor.FilteredFileFinded += EventObjectFilteredMessages;
			_fileSystemVisitor.DirectoryFinded += EventObjectFindedMessages;
			_fileSystemVisitor.FilteredDirectoryFinded += EventObjectFilteredMessages;
			_fileSystemVisitor.Finish += EventFinishMessages;

			int i = 0;
			foreach (var s in _fileSystemVisitor.ScanDir(_fileSystemVisitor.ValRootDirectory))
			{
				i++;
			}

			Assert.IsTrue(_eventStartChecked);
			Assert.IsTrue(_eventFileFindedChecked);
			Assert.IsTrue(_eventFilteredFileFindedChecked);
			Assert.IsTrue(_eventDirectoryFindedChecked);
			Assert.IsTrue(_eventFilteredDirectoryFindedChecked);
			Assert.IsTrue(_eventFinishChecked);
		}

		private void EventStartMessages(ILogService logService)
		{
			logService.Print("Процесс запущен...");
			_eventStartChecked = true;
		}

		private void EventObjectFindedMessages(FileInfo fileInfo, ILogService logService)
		{
			logService.Print(string.Format("Найден файл        : {0}", fileInfo.FullName));
			_eventFileFindedChecked = true;
		}

		private bool EventObjectFilteredMessages(FileInfo fileInfo, ILogService logService)
		{
			logService.Print(string.Format("Файл \"{0}\" соответствует заданной фильтрации. Поиск остановлен.", fileInfo.Name));
			_eventFilteredFileFindedChecked = true;
			return true;
		}

		private void EventObjectFindedMessages(DirectoryInfo directoryInfo, ILogService logService)
		{
			logService.Print(string.Format("Найдена директория : {0}", directoryInfo.FullName));
			_eventDirectoryFindedChecked = true;
		}

		private bool EventObjectFilteredMessages(DirectoryInfo directoryInfo, ILogService logService)
		{
			logService.Print(string.Format("Директория \"{0}\" соответствует заданной фильтрации. Продолжаем поиск...", directoryInfo.Name));
			_eventFilteredDirectoryFindedChecked = true;
			return false;
		}

		private void EventFinishMessages(ILogService logService)
		{
			logService.Print(string.Format("Процесс завершил свою работу."));
			_eventFinishChecked = true;
		}

	}
}
