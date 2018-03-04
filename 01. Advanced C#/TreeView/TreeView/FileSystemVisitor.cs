using System.Collections.Generic;
using System.IO;

namespace TreeView
{
	public class FileSystemVisitor
	{
		private readonly IWorkTree _workTree;
		private readonly ILogService _logService;
		private readonly bool _filterMode = false;
		private readonly FilteringDelegate _fd;
        private bool _breakNexStep = false;

        public FileSystemVisitor(string beginPoint, IWorkTree workTree, ILogService logService)
		{
			_rootDirectory = new DirectoryInfo(beginPoint);
			_workTree = workTree;
			_logService = logService;
		}

		public FileSystemVisitor(string beginPoint, IWorkTree workTree, ILogService logService, FilteringDelegate fd, bool includeOnlyFiltered) : this(beginPoint, workTree, logService)
		{
			_filterMode = true;
			_fd = fd;
			_includeOnlyFiltered = includeOnlyFiltered;
		}

		public event EventStartDelegate Start;
		public event EventFileFindedDelegate FileFinded;
		public event EventFilteredFileFindedDelegate FilteredFileFinded;
		public event EventDirectoryFindedDelegate DirectoryFinded;
		public event EventFilteredDirectoryFindedDelegate FilteredDirectoryFinded;
		public event EventFinishDelegate Finish;

		private readonly DirectoryInfo _rootDirectory;
		public DirectoryInfo ValRootDirectory
		{
			get { return _rootDirectory; }
		}
		private bool _includeOnlyFiltered;
		public bool ValIncludeOnlyFiltered
		{
			get { return _includeOnlyFiltered; }
			set { _includeOnlyFiltered = value; }
		}

		public IEnumerable<FileSystemInfo> ScanDir(DirectoryInfo dir)
		{
			Start?.Invoke(_logService);

			int level = 0;

			if (_filterMode)
				foreach (var treeObject in ScanDir(dir, level))
				{
					if (_breakNexStep) yield break;
					if (FilteringTreeObject(treeObject))
						yield return treeObject;
					else if (!_includeOnlyFiltered)
						yield return treeObject;
				}
			else
				foreach (var treeObject in ScanDir(dir, level))
					yield return treeObject;

			Finish?.Invoke(_logService);
		}

		private IEnumerable<FileSystemInfo> ScanDir(DirectoryInfo dir, int level)
		{
			if (_breakNexStep) yield break;

			DirectoryFinded?.Invoke(dir, _logService);
			yield return dir;

			FileInfo[] files = _workTree.GetFiles(dir);

			foreach (FileInfo f in files)
			{
				if (_breakNexStep) yield break;
				FileFinded?.Invoke(f, _logService);
				yield return f;
			}

			DirectoryInfo[] subDirs = _workTree.GetDirectories(dir);

			foreach (DirectoryInfo d in subDirs)
				foreach (var s in ScanDir(d, ++level))
					yield return s;
		}

		private bool FilteringTreeObject(FileSystemInfo fileSystemInfo)
		{
			string trrObjectName = fileSystemInfo.Name;

			if (_fd(trrObjectName))
			{
				if (fileSystemInfo is FileInfo && FilteredFileFinded != null)
					_breakNexStep = FilteredFileFinded((FileInfo)fileSystemInfo, _logService);
				else if (fileSystemInfo is DirectoryInfo && FilteredDirectoryFinded != null)
					_breakNexStep = FilteredDirectoryFinded((DirectoryInfo)fileSystemInfo, _logService);

				return true;
			}

			return false;
		}
	}
}
