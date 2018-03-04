using System.IO;

namespace TreeView
{
    public delegate bool FilteringDelegate(string name);
    public delegate void EventStartDelegate(ILogService logService);
    public delegate void EventFileFindedDelegate(FileInfo fileInfo, ILogService logService);
    public delegate bool EventFilteredFileFindedDelegate(FileInfo fileInfo, ILogService logService);
    public delegate void EventDirectoryFindedDelegate(DirectoryInfo directoryInfo, ILogService logService);
    public delegate bool EventFilteredDirectoryFindedDelegate(DirectoryInfo directoryInfo, ILogService logService);
    public delegate void EventFinishDelegate(ILogService logService);
}
