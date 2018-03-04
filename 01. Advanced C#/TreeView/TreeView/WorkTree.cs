using System.IO;

namespace TreeView
{
    internal class WorkTree : IWorkTree
	{
		public FileInfo[] GetFiles(DirectoryInfo dir)
		{
			return dir.GetFiles("*.*");
		}

		public DirectoryInfo[] GetDirectories(DirectoryInfo dir)
		{
			return dir.GetDirectories();
		}
	}
}
