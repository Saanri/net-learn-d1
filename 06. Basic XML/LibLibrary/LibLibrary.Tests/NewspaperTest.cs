using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LibLibrary.Tests
{
    [TestClass]
    public class NewspaperTest
    {
        private Newspaper _newspaper;

        [TestInitialize]
        public void Initialize()
        {
            _newspaper = new Newspaper("Взгляд"
                        , 30
                        , "Москва"
                        , "Дом Печати"
                        , 2006
                        , 7777888
                        , new DateTime(2000, 12, 31)
                        , "0378-5955"
                        );
        }

        [TestMethod]
        public void NewspaperTestGetShortInfo()
        {
            Assert.AreEqual(_newspaper.GetShortInfo(), "Газета: \"Взгляд\"; Номер: 7777888; Количество страниц: 30");
        }

        [TestMethod]
        public void NewspaperTestTakeRead()
        {
            _newspaper.TakeRead("Курочкин В.В.");

            Assert.AreEqual(_newspaper.ReaderVal, "Курочкин В.В.");
        }

        [TestMethod]
        public void NewspaperTestReturn()
        {
            _newspaper.TakeRead("Курочкин В.В.");
            _newspaper.Return();

            Assert.AreEqual(_newspaper.ReaderVal, "-");
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void NewspaperTestNameValException()
        {
            _newspaper.NameVal = "";
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void NewspaperTestPublicationPlaceValException()
        {
            _newspaper.PublicationPlaceVal = "";
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void NewspaperTestPublisherValException()
        {
            _newspaper.PublisherVal = "";
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void NewspaperTestPublicationYearValException()
        {
            _newspaper.PublicationYearVal = 1899;
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void NewspaperTestNumberValException()
        {
            _newspaper.NumberVal = -1;
        }
    }
}
