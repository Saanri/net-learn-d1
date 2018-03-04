using System.IO;

namespace TreeView
{
    public interface IWorkTree
    {
        FileInfo[] GetFiles(DirectoryInfo dir);
        DirectoryInfo[] GetDirectories(DirectoryInfo dir);
    }
}
