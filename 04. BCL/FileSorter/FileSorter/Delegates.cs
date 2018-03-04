namespace FileSorter
{
    public delegate void EventStartDelegate();
    public delegate void EventActivateDirWatcherDelegate(string sourceDir);
    public delegate void EventFileDetectedDelegate(string fileFullPath);
    public delegate void EventRuleFoundDelegate(bool isFinded, string fileName);
    public delegate void EventFileMovedDelegate(bool isMoved, string sourceFileFullPath, string targetFileFullPath);
}
