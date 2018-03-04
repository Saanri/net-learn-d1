using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace UrlLoader.tests
{
    [TestClass]
    public class UrlLoaderTests
    {
        private string loadUrl;
        private string dirDest;
        private string folderDestName;
        private static int isLogExec = 0;
        private LogService logger;

        [TestInitialize]
        public void Initialize()
        {
            loadUrl = "http://www.zavod-vcm.ru";
            dirDest = @"loaded\";
            folderDestName = dirDest + @"www.zavod-vcm.ru\";
            logger = new LogService();
        }

        [TestMethod]
        public void LoadUrlWithoutFileFilterAndNotVerboseTest()
        {
            Loader loader = new Loader(loadUrl, dirDest, 0, true, new string[] { }.ToList(), false, logger);
            loader.Load();

            // почему у тебя loadableFileList =  new string[] { "css", "jpg", "png" } а загружаетс я"about.php" ??
            // php я считаю страницей, а страницы не фильтруются данным параметром
            Assert.IsTrue(File.Exists(folderDestName + @"about.php"));
            Assert.IsTrue(File.Exists(folderDestName + @"contact.php"));
            Assert.IsTrue(File.Exists(folderDestName + @"cooperation.php"));
            Assert.IsTrue(File.Exists(folderDestName + @"eventvcm.php"));
            Assert.IsTrue(File.Exists(folderDestName + @"feedback.php"));
            Assert.IsTrue(File.Exists(folderDestName + @"index.html"));
            Assert.IsTrue(File.Exists(folderDestName + @"index.php"));
            Assert.IsTrue(File.Exists(folderDestName + @"partner.php"));
            Assert.IsTrue(File.Exists(folderDestName + @"product.php"));
            Assert.IsTrue(File.Exists(folderDestName + @"shareholder.php"));
            Assert.IsTrue(File.Exists(folderDestName + @"sitemap.html"));
            Assert.IsTrue(File.Exists(folderDestName + @"tender.php"));
            Assert.IsTrue(File.Exists(folderDestName + @"tradeunion.php"));
            Assert.IsTrue(File.Exists(folderDestName + @"vacancies.php"));
            Assert.IsTrue(File.Exists(folderDestName + @"css/site.css"));
            Assert.IsTrue(File.Exists(folderDestName + @"css/slider.css"));
            Assert.IsTrue(File.Exists(folderDestName + @"en-en/index.php"));
            Assert.IsTrue(File.Exists(folderDestName + @"img/baner1.jpg"));
            Assert.IsTrue(File.Exists(folderDestName + @"img/baner2.jpg"));
            Assert.IsTrue(File.Exists(folderDestName + @"img/baner3.jpg"));
            Assert.IsTrue(File.Exists(folderDestName + @"img/logo.png"));

            Assert.AreEqual(isLogExec, 0);
        }

        [TestMethod]
        public void LoadUrlWithFileFilterAndVerboseTest()
        {
            Loader loader = new Loader(loadUrl, dirDest, 0, true, new string[] { ".css", ".jpg" }.ToList(), true, logger);
            loader.Load();

            Assert.IsTrue(File.Exists(folderDestName + @"index.html"));
            Assert.IsTrue(File.Exists(folderDestName + @"css/site.css"));
            Assert.IsTrue(File.Exists(folderDestName + @"img/baner1.jpg"));
            Assert.IsFalse(File.Exists(folderDestName + @"img/logo.png"));

            Assert.AreEqual(isLogExec, 1);
        }

        [TestCleanup]
        public void Cleanup()
        {
            isLogExec = 0;

            if (Directory.Exists(dirDest))
                Directory.Delete(dirDest, true);
        }

        public class LogService : ILogService
        {
            public void Print(string message)
            {
                isLogExec = 1;
                Console.WriteLine(message);
            }
        }
    }
}
