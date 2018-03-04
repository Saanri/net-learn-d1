using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibLibrary.Tests
{
    [TestClass]
    public class LibraryTest
    {
        private Library library;

        [TestInitialize]
        public void Initialize()
        {
            Book book1 = new Book("книга1", 200, "Город1", "Издательство1", 2003, new string[] { "Автор1" }, "0-306-40615-2");
            Book book2 = new Book("книга2", 202, "Город1", "Издательство1", 2001, new string[] { "Автор1", "Автор2" }, "978-0-306-40615-7");
            Book book3 = new Book("книга3", 204, "Город2", "Издательство2", 2002, new string[] { "Автор2" }, "978-5-699-16651-0");
            Newspaper newspaper1 = new Newspaper("Газета1", 5, "Город1", "Издательство1", 2002, 101, new DateTime(2000, 12, 14), "0378-5955");
            Newspaper newspaper2 = new Newspaper("Газета2", 6, "Город2", "Издательство2", 2000, 102, new DateTime(2001, 06, 23), "0028-0836");
            Newspaper newspaper3 = new Newspaper("Газета3", 7, "Город2", "Издательство2", 2001, 103, new DateTime(2001, 07, 12), "0251-2645");
            Patent patent1 = new Patent("Патент1", 10, "Автор1", "Страна1", "RE000126", new DateTime(2001, 12, 31), new DateTime(2005, 01, 31));
            Patent patent2 = new Patent("Патент2", 12, "Автор2", "Страна1", "D0000126", new DateTime(2004, 12, 31), new DateTime(2002, 01, 31));
            Patent patent3 = new Patent("Патент3", 14, "Автор3", "Страна2", "H0000001", new DateTime(2004, 12, 31), new DateTime(2004, 01, 31));

            library = new Library();
            library.AddUnit(book1); library.AddUnit(book2); library.AddUnit(book3);
            library.AddUnit(newspaper1); library.AddUnit(newspaper2); library.AddUnit(newspaper3);
            library.AddUnit(patent1); library.AddUnit(patent2); library.AddUnit(patent3);
        }

        // Добавление записей в каталог
        [TestMethod]
        public void libraryTestAddUnit()
        {
            Book book4 = new Book("книга4", 206, "Город1", "Издательство1", 2002, new string[] { "Автор3" }, "978-5-17-047247-5");
            Newspaper newspaper4 = new Newspaper("Газета4", 8, "Город3", "Издательство3", 2002, 104, new DateTime(2002, 07, 11), "1609-140X");
            Patent patent4 = new Patent("Патент4", 16, "Автор4", "Страна2", "T0000001", new DateTime(2001, 12, 31), new DateTime(2002, 01, 31));

            IList<LibraryUnit> unitList;

            unitList = library.FindByName("книга4");
            Assert.AreEqual(unitList.Count, 0);

            library.AddUnit(book4);
            unitList = library.FindByName("книга4");
            Assert.AreEqual(unitList.Count, 1);

            unitList = library.FindByName("Газета4");
            Assert.AreEqual(unitList.Count, 0);

            library.AddUnit(newspaper4);
            unitList = library.FindByName("Газета4");
            Assert.AreEqual(unitList.Count, 1);

            unitList = library.FindByName("Патент4");
            Assert.AreEqual(unitList.Count, 0);

            library.AddUnit(patent4);
            unitList = library.FindByName("Патент4");
            Assert.AreEqual(unitList.Count, 1);
        }

        // Удаление записей из каталога с указанием типа удаляемой записи и имени
        [TestMethod]
        public void libraryTestDelUnit()
        {
            IList<LibraryUnit> unitList;

            unitList = library.FindByName("книга1");
            Assert.AreEqual(unitList.Count, 1);

            library.DelUnit(UnitType.Book, "книга1");
            unitList = library.FindByName("книга1");
            Assert.AreEqual(unitList.Count, 0);

            unitList = library.FindByName("Газета1");
            Assert.AreEqual(unitList.Count, 1);

            library.DelUnit(UnitType.Newspaper, "Газета1");
            unitList = library.FindByName("Газета1");
            Assert.AreEqual(unitList.Count, 0);

            unitList = library.FindByName("Патент1");
            Assert.AreEqual(unitList.Count, 1);

            library.DelUnit(UnitType.Patent, "Патент1");
            unitList = library.FindByName("Патент1");
            Assert.AreEqual(unitList.Count, 0);
        }

        // Просмотр каталога, возврат string
        [TestMethod]
        public void libraryTestGetCatalog()
        {
            List<LibraryUnit> unitList1 = (List<LibraryUnit>)library.GetCatalog();
            StringBuilder unitSrc = new StringBuilder();

            foreach (LibraryUnit lu in unitList1)
                unitSrc.AppendLine(lu.GetShortInfo());

            StringBuilder unitTrg = new StringBuilder();
            unitTrg.AppendLine("Книга: \"книга1\"; Автор(ы): Автор1; Количество страниц: 200");
            unitTrg.AppendLine("Книга: \"книга2\"; Автор(ы): Автор1, Автор2; Количество страниц: 202");
            unitTrg.AppendLine("Книга: \"книга3\"; Автор(ы): Автор2; Количество страниц: 204");
            unitTrg.AppendLine("Газета: \"Газета1\"; Номер: 101; Количество страниц: 5");
            unitTrg.AppendLine("Газета: \"Газета2\"; Номер: 102; Количество страниц: 6");
            unitTrg.AppendLine("Газета: \"Газета3\"; Номер: 103; Количество страниц: 7");
            unitTrg.AppendLine("Патент: \"Патент1\"; Изобретатель(и): Автор1; Страна: Страна1; Регистрационный номер: RE000126");
            unitTrg.AppendLine("Патент: \"Патент2\"; Изобретатель(и): Автор2; Страна: Страна1; Регистрационный номер: D0000126");
            unitTrg.AppendLine("Патент: \"Патент3\"; Изобретатель(и): Автор3; Страна: Страна2; Регистрационный номер: H0000001");
            Assert.AreEqual(unitSrc.ToString(), unitTrg.ToString());

            Book book01 = new Book("книга__", 999, "Город1", "Издательство1", 2025, new string[] { "Автор1" }, "0-306-40615-2");
            Book book02 = new Book("книга__", 999, "Город1", "Издательство1", 2025, new string[] { "Автор1", "Автор2" }, "978-0-306-40615-7");
            library.AddUnit(book01); library.AddUnit(book02);

            List<LibraryUnit> unitList2 = (List<LibraryUnit>)library.GetCatalog(unit => (unit.NameVal == "книга__" && unit.pageCountVal > 900 && unit.GetPublicationYear() > 2020), unit => unit.NameVal);
            Assert.AreEqual(unitList2.Count, 2);
        }

        // Поиск по названию
        [TestMethod]
        public void libraryTestFindByName()
        {
            List<LibraryUnit> unitList = (List<LibraryUnit>)library.FindByName("книга1");
            Assert.AreEqual(unitList.Count, 1);
        }

        // Сортировка по году выпуска в прямом(0) и обратном(1) порядке
        [TestMethod]
        public void libraryTestSortByPublicationYear()
        {
            List<LibraryUnit> unitList = (List<LibraryUnit>)library.SortByPublicationYear(MethodSort.Ascending);
            StringBuilder sortSrc = new StringBuilder();

            foreach (LibraryUnit lu in unitList)
                sortSrc.AppendLine(lu.GetShortInfo());

            StringBuilder sortTrg = new StringBuilder();
            sortTrg.AppendLine("Книга: \"книга2\"; Автор(ы): Автор1, Автор2; Количество страниц: 202");
            sortTrg.AppendLine("Книга: \"книга3\"; Автор(ы): Автор2; Количество страниц: 204");
            sortTrg.AppendLine("Книга: \"книга1\"; Автор(ы): Автор1; Количество страниц: 200");
            sortTrg.AppendLine("Газета: \"Газета2\"; Номер: 102; Количество страниц: 6");
            sortTrg.AppendLine("Газета: \"Газета3\"; Номер: 103; Количество страниц: 7");
            sortTrg.AppendLine("Газета: \"Газета1\"; Номер: 101; Количество страниц: 5");
            sortTrg.AppendLine("Патент: \"Патент2\"; Изобретатель(и): Автор2; Страна: Страна1; Регистрационный номер: D0000126");
            sortTrg.AppendLine("Патент: \"Патент3\"; Изобретатель(и): Автор3; Страна: Страна2; Регистрационный номер: H0000001");
            sortTrg.AppendLine("Патент: \"Патент1\"; Изобретатель(и): Автор1; Страна: Страна1; Регистрационный номер: RE000126");
            Assert.AreEqual(sortSrc.ToString(), sortTrg.ToString());

            unitList = (List<LibraryUnit>)library.SortByPublicationYear(MethodSort.Descending);
            sortSrc.Clear();

            foreach (LibraryUnit lu in unitList)
                sortSrc.AppendLine(lu.GetShortInfo());

            sortTrg.Clear();

            sortTrg.AppendLine("Книга: \"книга1\"; Автор(ы): Автор1; Количество страниц: 200");
            sortTrg.AppendLine("Книга: \"книга3\"; Автор(ы): Автор2; Количество страниц: 204");
            sortTrg.AppendLine("Книга: \"книга2\"; Автор(ы): Автор1, Автор2; Количество страниц: 202");
            sortTrg.AppendLine("Газета: \"Газета1\"; Номер: 101; Количество страниц: 5");
            sortTrg.AppendLine("Газета: \"Газета3\"; Номер: 103; Количество страниц: 7");
            sortTrg.AppendLine("Газета: \"Газета2\"; Номер: 102; Количество страниц: 6");
            sortTrg.AppendLine("Патент: \"Патент1\"; Изобретатель(и): Автор1; Страна: Страна1; Регистрационный номер: RE000126");
            sortTrg.AppendLine("Патент: \"Патент3\"; Изобретатель(и): Автор3; Страна: Страна2; Регистрационный номер: H0000001");
            sortTrg.AppendLine("Патент: \"Патент2\"; Изобретатель(и): Автор2; Страна: Страна1; Регистрационный номер: D0000126");
            Assert.AreEqual(sortSrc.ToString(), sortTrg.ToString());
        }

        // Поиск всех книг данного автора(в том числе, тех, у которых он является соавтором)
        [TestMethod]
        public void libraryTestGetBooksByAuthor()
        {
            List<Book> bookList = library.GetBooksByAuthor("Автор2");
            Assert.AreEqual(bookList.Count, 2);
            Assert.AreEqual(bookList[0].GetShortInfo(), "Книга: \"книга2\"; Автор(ы): Автор1, Автор2; Количество страниц: 202");
            Assert.AreEqual(bookList[1].GetShortInfo(), "Книга: \"книга3\"; Автор(ы): Автор2; Количество страниц: 204");
        }

        // Вывод всех книг, название издательства которых начинаются с заданного набора символов, с группировкой по издательству
        [TestMethod]
        public void libraryTestGetBooksByPublisher()
        {
            List<Book> bookList = library.GetBooksByPublisher("Издательство1");
            Assert.AreEqual(bookList.Count, 2);
            Assert.AreEqual(bookList[0].GetShortInfo(), "Книга: \"книга1\"; Автор(ы): Автор1; Количество страниц: 200");
            Assert.AreEqual(bookList[1].GetShortInfo(), "Книга: \"книга2\"; Автор(ы): Автор1, Автор2; Количество страниц: 202");
        }

        // Группировка записей по годам издания
        [TestMethod]
        public void libraryTestGetYearsGroup()
        {
            var YearGroups = library.GetYearsGroup();
            StringBuilder yearsGroupSrc = new StringBuilder();
            foreach (KeyValuePair<int, List<LibraryUnit>> kvp in YearGroups)
            {
                yearsGroupSrc.AppendLine(string.Format("{0}:", kvp.Key));
                foreach (LibraryUnit lu in kvp.Value)
                    yearsGroupSrc.AppendLine(string.Format("- {0}", lu.NameVal));
            }

            StringBuilder yearsGroupTrg = new StringBuilder();
            yearsGroupTrg.AppendLine("2003:");
            yearsGroupTrg.AppendLine("- книга1");
            yearsGroupTrg.AppendLine("2001:");
            yearsGroupTrg.AppendLine("- книга2");
            yearsGroupTrg.AppendLine("- Газета3");
            yearsGroupTrg.AppendLine("2002:");
            yearsGroupTrg.AppendLine("- книга3");
            yearsGroupTrg.AppendLine("- Газета1");
            yearsGroupTrg.AppendLine("- Патент2");
            yearsGroupTrg.AppendLine("2000:");
            yearsGroupTrg.AppendLine("- Газета2");
            yearsGroupTrg.AppendLine("2005:");
            yearsGroupTrg.AppendLine("- Патент1");
            yearsGroupTrg.AppendLine("2004:");
            yearsGroupTrg.AppendLine("- Патент3");

            Assert.AreEqual(yearsGroupSrc.ToString(), yearsGroupTrg.ToString());
        }

        [TestMethod]
        public void libraryTestSaveLoadTxt()
        {
            int unitCount = library.GetCatalog().Count;
            Assert.IsTrue(unitCount > 0);

            List<LibraryUnit> unitList = (List<LibraryUnit>)library.GetCatalog();
            string[] unitsBefore = new string[unitList.Count];
            int i = 0;

            foreach (LibraryUnit u in unitList)
            {
                unitsBefore[i] = u.ToString();
                i++;
            }

            library.Save(@"C:\Users\Public\lib.txt");
            library.Load(@"C:\Users\Public\lib.txt", 1);

            string[] unitsAfter = new string[unitList.Count];
            i = 0;

            foreach (LibraryUnit u in unitList)
            {
                unitsAfter[i] = u.ToString();
                Assert.AreEqual(unitsBefore[i],unitsAfter[i]);
                i++;
            }
        }

        [TestMethod]
        public void libraryTestSaveLoadXML()
        {
            int unitCount = library.GetCatalog().Count;
            Assert.IsTrue(unitCount > 0);

            List<LibraryUnit> unitListBefore = (List<LibraryUnit>)library.GetCatalog();
            string[] unitsBefore = new string[unitListBefore.Count];
            int i = 0;

            foreach (LibraryUnit u in unitListBefore)
            {
                unitsBefore[i] = u.ToString();
                i++;
            }

            library.Save(@"C:\Users\Public\lib.xml");
            Assert.IsTrue(library.Load(@"C:\Users\Public\lib.xml", 1));

            unitCount = library.GetCatalog().Count;
            Assert.IsTrue(unitCount > 0);

            List<LibraryUnit> unitListAfter = (List<LibraryUnit>)library.GetCatalog();
            string[] unitsAfter = new string[unitListAfter.Count];
            i = 0;

            foreach (LibraryUnit u in unitListAfter)
            {
                unitsAfter[i] = u.ToString();
                Assert.AreEqual(unitsBefore[i], unitsAfter[i]);
                i++;
            }
        }

    }
}