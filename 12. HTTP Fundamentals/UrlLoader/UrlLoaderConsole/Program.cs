using System;
using System.Collections.Generic;
using System.Linq;
using UrlLoader;

namespace UrlLoaderConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            LogService logger = new LogService();
            Loader loader = new Loader("http://www.zavod-vcm.ru", @"loaded\", 0, true, new string[] { "css", "doc", "gif", "jpg", "png" }.ToList(), true, logger);
            loader.Load();

            Console.WriteLine("Загрузка завершена!");
           
            Console.ReadKey();
        }
    }

    public class LogService : ILogService
    {
        public void Print(string message)
        {
            Console.WriteLine(message);
        }
    }
}
