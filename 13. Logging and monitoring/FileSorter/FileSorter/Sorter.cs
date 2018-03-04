using FileSorter.Configuration;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace FileSorter
{
    public class Sorter
    {
        //private List<string> _dirs = new List<string>();
        //private Dictionary<string, string> _rules = new Dictionary<string, string>() { };
        //private string _defaultDestinationDir;
        private SimpleConfigurationSection _config;

        public Sorter(SimpleConfigurationSection config)
        {
            //_rules.Add(@".*\.txt$", @"C:\Users\Public\targetFolder\txt");
            //_rules.Add(@".*\.doc$", @"C:\Users\Public\targetFolder\doc");
            //_rules.Add(@".*\.xls$", @"C:\Users\Public\targetFolder\xls");

            //_dirs.Add(@"C:\Users\Public\sourceFolder0");
            //_dirs.Add(@"C:\Users\Public\sourceFolder1");
            //_dirs.Add(@"C:\Users\Public\sourceFolder2");

            //_defaultDestinationDir = @"C:\Users\Public\targetFolder\other";
            _config = config;
        }

        public event EventStartDelegate Start;
        public event EventActivateDirWatcherDelegate ActivateDirWatcher;
        public event EventFileDetectedDelegate FileDetected;
        public event EventRuleFoundDelegate RuleFound;
        public event EventFileMovedDelegate FileMoved;

        public void StartProcess()
        {
            Start?.Invoke();

            //foreach (string sourceDir in _dirs)
            //{
            //    FileSystemWatcher watcher = new FileSystemWatcher(sourceDir, "*.*");
            //    watcher.Created += new FileSystemEventHandler(ObjectDetected);
            //    watcher.EnableRaisingEvents = true;

            //    ActivateDirWatcher?.Invoke(sourceDir);
            //}
            foreach (ViewedDirElement viewedDir in _config.ViewedDirs)
            {
                FileSystemWatcher watcher = new FileSystemWatcher(viewedDir.Dir, "*.*");
                watcher.Created += new FileSystemEventHandler(ObjectDetected);
                watcher.EnableRaisingEvents = true;

                ActivateDirWatcher?.Invoke(viewedDir.Dir);
            }
        }

        private void ObjectDetected(object source, FileSystemEventArgs e)
        {
            if (File.Exists(e.FullPath))
            {
                FileDetected?.Invoke(e.FullPath);

                //string destinationDir = _defaultDestinationDir;
                string destinationDir = _config.DefaultDestinationDir.Dir;
                bool isAddNumber = true;
                bool isAddMoveDate = false;

                //foreach (var rule in _rules)
                //    if (Regex.IsMatch(e.Name, rule.Key))
                //        destinationDir = rule.Value;
                foreach (RuleElement rule in _config.Rules)
                    if (Regex.IsMatch(e.Name, rule.Rule))
                    {
                        destinationDir = rule.TargetFolder;
                        isAddNumber = rule.IsAddNumber;
                        isAddMoveDate = rule.IsAddMoveDate;
                    }

                if (destinationDir.Equals(_config.DefaultDestinationDir.Dir)) RuleFound?.Invoke(false, e.Name);
                else RuleFound?.Invoke(true, e.Name);

                string targetFileName = e.Name;

                if (isAddMoveDate)
                {
                    string fileExtension = "";
                    int fileExtensionIdx = -1;
                    Match match = Regex.Match(e.Name, @"\.[а-я|А-Я|a-z|A-z|0-9]*$");
                    if (match.Success)
                    {
                        fileExtension = match.Value;
                        fileExtensionIdx = match.Index;
                    }

                    targetFileName = string.Format("{0} {1}{2}", e.Name.Substring(0, fileExtensionIdx), DateTime.Today.ToString("yyyyMMdd"), fileExtension);
                }

                string destinationFile = destinationDir + "\\" + targetFileName;

                if (!File.Exists(destinationFile))
                {
                    Thread.Sleep(1000);
                    File.Move(e.FullPath, destinationFile);
                    FileMoved?.Invoke(true, e.FullPath, destinationFile);
                }
                else if (isAddNumber)
                {
                    string fileExtension = "";
                    int fileExtensionIdx = -1;
                    Match match = Regex.Match(destinationFile, @"\.[а-я|А-Я|a-z|A-z|0-9]*$");
                    if (match.Success)
                    {
                        fileExtension = match.Value;
                        fileExtensionIdx = match.Index;
                    }

                    int fileIdx = 0;
                    int fileIdxIdx = fileExtensionIdx;
                    string pattern = @"\(\d*\)$";
                    if (fileExtensionIdx > 0) match = Regex.Match(destinationFile.Substring(0, fileExtensionIdx), pattern);
                    else match = Regex.Match(destinationFile, pattern);

                    if (match.Success)
                    {
                        fileIdx = Convert.ToInt32(Regex.Replace(match.Value, @"[^\d]+", "")) + 1;
                        fileIdxIdx = match.Index;
                    }

                    while (true)
                    {
                        destinationFile = string.Format("{0} ({1}){2}", destinationFile.Substring(0, fileIdxIdx), fileIdx, fileExtension);
                        if (!File.Exists(destinationFile))
                        {
                            Thread.Sleep(1000);
                            File.Move(e.FullPath, destinationFile);
                            FileMoved?.Invoke(true, e.FullPath, destinationFile);
                            break;
                        }
                        fileIdx++;
                    }
                }
                else
                    FileMoved?.Invoke(false, e.FullPath, "");
            }
        }
    }
}
