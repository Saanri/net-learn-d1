using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LibLibrary.Tests
{
    [TestClass]
    public class BookTest
    {
        private Book _book;

        [TestInitialize]
        public void Initialize()
        {
            _book = new Book("Между планетами"
                           , 546
                           , "Москва"
                           , "Радуга"
                           , 2006
                           , new string[] { "Роберт Хайнлайн" }
                           , "0-306-40615-2"
                           );
        }

        [TestMethod]
        public void BookTestGetShortInfo()
        {
            Assert.AreEqual(_book.GetShortInfo(), "Книга: \"Между планетами\"; Автор(ы): Роберт Хайнлайн; Количество страниц: 546");
        }

        [TestMethod]
        public void BookTestTakeRead()
        {
            _book.TakeRead("Курочкин В.В.");

            Assert.AreEqual(_book.ReaderVal, "Курочкин В.В.");
        }

        [TestMethod]
        public void BookTestReturn()
        {
            _book.TakeRead("Курочкин В.В.");
            _book.Return();

            Assert.AreEqual(_book.ReaderVal, "-");
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void BookTestNameValException()
        {
            _book.NameVal = "";
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void BookTestPublicationPlaceValException()
        {
            _book.PublicationPlaceVal = "";
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void BookTestPublisherValException()
        {
            _book.PublisherVal = "";
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void BookTestPublicationYearValException()
        {
            _book.PublicationYearVal = 1899;
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void BookTestAuthorsValException()
        {
            _book.AuthorsVal = new string[] { "" };
        }
    }
}
