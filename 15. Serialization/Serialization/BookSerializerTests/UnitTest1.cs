using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BooksSerializer;
using System.Linq;
using System.IO;

namespace BookSerializerTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SerializerLoadXMLTest()
        {
            Catalog catalog = Serializer.LoadXML(@"Data\books.xml");
            Assert.IsTrue(catalog.Books.Any());
        }

        [TestMethod]
        public void SerializerSaveXMLTest()
        {
            File.Delete(@"books.xml");

            Book book1 = new Book()
            {
                id = "bk101",
                isbn = "0-596-00103-7",
                author = "Löwy, Juval",
                title = "COM and .NET Component Services",
                genre = BookGenre.Computer,
                publisher = "O'Reilly",
                somePublishDate = "2001-09-01",
                description = @"
      COM & .NET Component Services provides both traditional COM programmers and new .NET
      component developers with the information they need to begin developing applications that take full advantage of COM+ services.
      This book focuses on COM+ services, including support for transactions, queued components, events, concurrency management, and security
    ",
                someRegistrationDate = "2016-01-05"
            };
            Book book2 = new Book()
            {
                id = "bk102",
                author = "Ralls, Kim",
                title = "Midnight Rain",
                genre = BookGenre.Fantasy,
                publisher = "R & D",
                somePublishDate = "2000-12-16",
                description = @"
      A former architect battles corporate zombies,
      an evil sorceress, and her own childhood to become queen
      of the world.
    ",
                someRegistrationDate = "2017-01-01"
            };

            Catalog catalog = new Catalog();
            catalog.Books.Add(book1);
            catalog.Books.Add(book2);
            catalog.valDate = "2016-02-05";
            Serializer.SaveXML(catalog, "books.xml");
            Assert.IsTrue(File.Exists("books.xml"));

            Catalog loadedCatalog = Serializer.LoadXML("books.xml");
            Assert.IsTrue(loadedCatalog.Books.Count() == 2);
        }

    }
}
