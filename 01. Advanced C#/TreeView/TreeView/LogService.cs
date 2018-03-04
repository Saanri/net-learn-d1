using System;

namespace TreeView
{
    public class LogService : ILogService
    {
        public void Print(string message)
        {
            Console.WriteLine(message);
        }
    }
}
