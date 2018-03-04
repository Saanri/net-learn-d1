using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LibLibrary.Tests
{
    [TestClass]
    public class PatentTest
    {
        private Patent _patent;

        [TestInitialize]
        public void Initialize()
        {
            _patent = new Patent("Патент1"
                        , 300
                        , "Иванов И.И."
                        , "Россия"
                        , "PP000126"
                        , new DateTime(2000, 12, 31)
                        , new DateTime(2001, 01, 31)
                        );
        }

        [TestMethod]
        public void PatentTestGetShortInfo()
        {
            string patentShortInfo = _patent.GetShortInfo();

            Assert.AreEqual(patentShortInfo, "Патент: \"Патент1\"; Изобретатель(и): Иванов И.И.; Страна: Россия; Регистрационный номер: PP000126");
        }

        [TestMethod]
        public void PatentTestGetPublicationYear()
        {
            Assert.AreEqual(_patent.GetPublicationYear(), 2001);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void PatentTestNameValException()
        {
            _patent.NameVal = "";
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void PatentTestInventorValException()
        {
            _patent.InventorVal = "";
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void PatentTestCountryValException()
        {
            _patent.CountryVal = "";
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void PatentTestApplicationDateValException()
        {
            _patent.ApplicationDateVal = new DateTime(1949, 12, 31);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void PatentTestPublicationDateValException()
        {
            _patent.PublicationDateVal = new DateTime(1949, 12, 31);
        }
    }
}
