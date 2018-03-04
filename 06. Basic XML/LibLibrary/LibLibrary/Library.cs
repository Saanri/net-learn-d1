using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace LibLibrary
{
    public class Library
    {
        private readonly List<LibraryUnit> _libraryUnits;
        private readonly XMLValidator _XMLValidator;

        public class SerializeLibraryUnits
        {
            public DateTime dateSave;

            [XmlArrayItem(typeof(Book)), XmlArrayItem(typeof(Patent)), XmlArrayItem(typeof(Newspaper))]
            public LibraryUnit[] libraryUnits;
        }

        public Library()
        {
            _libraryUnits = new List<LibraryUnit>();
            _XMLValidator = new XMLValidator();
        }

        /// <summary>
        /// Добавление записей в каталог
        /// </summary>
        /// <param name="patent"></param>
        public void AddUnit(LibraryUnit unit)
        {
            _libraryUnits.Add(unit);
        }

        /// <summary>
        /// Удаление записей из каталога с указанием типа удаляемой записи и имени
        /// </summary>
        /// <param name="unitType"></param>
        /// <param name="unitName"></param>
        public void DelUnit(UnitType unitType, string unitName)
        {
            if (unitType == UnitType.Book)
                DelBook(unitName);
            else if (unitType == UnitType.Newspaper)
                DelNewspaper(unitName);
            else if (unitType == UnitType.Patent)
                DelPatent(unitName);
        }

        /// <summary>
        /// Просмотр каталога
        /// </summary>
        public IList<LibraryUnit> GetCatalog()
        {
            var resList = new List<LibraryUnit>();

            foreach (LibraryUnit u in _libraryUnits)
                resList.Add(u);

            resList.Sort(
                delegate (LibraryUnit x, LibraryUnit y)
                {
                    return ((x is Book ? "10000" : x is Newspaper ? "20000" : x is Patent ? "30000" : "0") + x.GetShortInfo()).CompareTo((y is Book ? "10000" : y is Newspaper ? "20000" : y is Patent ? "30000" : "0") + y.GetShortInfo());
                }
            );

            return resList;
        }

        /// <summary>
        /// Просмотр каталога с возможностью поиска и сортировки результата, с помощю лямбда выражения
        /// </summary>
        public IList<LibraryUnit> GetCatalog<TKey>(Func<LibraryUnit, bool> where, Func<LibraryUnit, TKey> order)
        {
            var resList = _libraryUnits.Where(where).OrderBy(units => order).ToList();

            return resList;
        }

        /// <summary>
        /// Поиск по названию
        /// </summary>
        /// <param name="unitName"></param>
        public IList<LibraryUnit> FindByName(string unitName)
        {
            var resList = new List<LibraryUnit>();

            foreach (LibraryUnit u in _libraryUnits)
                if (u.NameVal.Equals(unitName))
                    resList.Add(u);

            return resList;
        }

        /// <summary>
        /// Сортировка по году выпуска в прямом(0) и обратном(1) порядке
        /// </summary>
        /// <param name="methodSort"></param>
        public IList<LibraryUnit> SortByPublicationYear(MethodSort methodSort)
        {
            var resList = new List<LibraryUnit>();

            foreach (LibraryUnit u in _libraryUnits)
                resList.Add(u);

            resList.Sort(
                delegate (LibraryUnit x, LibraryUnit y)
                {
                    return methodSort == MethodSort.Ascending
                                ? ((x is Book ? 10000 : x is Newspaper ? 20000 : x is Patent ? 30000 : 0) + x.GetPublicationYear()).CompareTo((y is Book ? 10000 : y is Newspaper ? 20000 : y is Patent ? 30000 : 0) + y.GetPublicationYear())
                                : ((y is Book ? 30000 : y is Newspaper ? 20000 : y is Patent ? 10000 : 0) + y.GetPublicationYear()).CompareTo((x is Book ? 30000 : x is Newspaper ? 20000 : x is Patent ? 10000 : 0) + x.GetPublicationYear());
                }
            );

            return resList;
        }

        /// <summary>
        /// Поиск всех книг данного автора(в том числе, тех, у которых он является соавтором)
        /// </summary>
        /// <param name="authorName"></param>
        public List<Book> GetBooksByAuthor(string authorName)
        {
            var resList = new List<Book>();

            foreach (LibraryUnit u in _libraryUnits)
                if (u is Book)
                    foreach (string s in ((Book)u).AuthorsVal)
                        if (s.Equals(authorName))
                            resList.Add((Book)u);

            return resList;
        }

        /// <summary>
        /// Вывод всех книг, название издательства которых начинаются с заданного набора символов, с группировкой по издательству
        /// </summary>
        /// <param name="publisherName"></param>
        public List<Book> GetBooksByPublisher(string publisherName)
        {
            List<Book> resList = new List<Book>();

            foreach (LibraryUnit u in _libraryUnits)
                if (u is Book)
                    if (((Book)u).PublisherVal.StartsWith(publisherName))
                        resList.Add((Book)u);

            return resList;
        }

        /// <summary>
        /// Группировка записей по годам издания
        /// </summary>
        public Dictionary<int, List<LibraryUnit>> GetYearsGroup()
        {
            var yearGroups = new Dictionary<int, List<LibraryUnit>>();

            foreach (LibraryUnit u in _libraryUnits)
            {
                List<LibraryUnit> unitList;
                if (yearGroups.TryGetValue(u.GetPublicationYear(), out unitList))
                    unitList.Add(u);
                else
                {
                    unitList = new List<LibraryUnit>();
                    unitList.Add(u);
                    yearGroups.Add(u.GetPublicationYear(), unitList);
                }
            }

            return yearGroups;
        }

        /// <summary>
        /// Выгрузка данных из фонда
        /// </summary>
        /// <param name="path"></param>
        public void Save(string path)
        {
            if (path.Substring(path.Length - 4, 4).ToLower().Equals(".txt"))
                SaveTxt(path);
            else if (path.Substring(path.Length - 4, 4).ToLower().Equals(".xml"))
                SaveXML(path);
            else
                throw (new Exception("Указанный формат файла не поддерживается!"));
        }

        /// <summary>
        /// Загрузка данных в фонд
        /// </summary>
        /// <param name="path"></param>
        /// <param name="loadMethod"></param>
        public bool Load(string path, int loadMethod)
        {
            if (path.Substring(path.Length - 4, 4).ToLower().Equals(".txt"))
                return LoadTxt(path, loadMethod);
            else if (path.Substring(path.Length - 4, 4).ToLower().Equals(".xml"))
                return LoadXML(path, loadMethod);
            else
                throw (new Exception("Указанный формат файла не поддерживается!"));
        }

        private void DelBook(string bookName)
        {
            for (int i = 0; i < _libraryUnits.Count; i++)
                if (_libraryUnits[i] is Book && _libraryUnits[i].NameVal == bookName)
                    _libraryUnits.RemoveAt(i);
        }

        private void DelNewspaper(string newspaperName)
        {
            for (int i = 0; i < _libraryUnits.Count; i++)
                if (_libraryUnits[i] is Newspaper && _libraryUnits[i].NameVal == newspaperName)
                    _libraryUnits.RemoveAt(i);
        }

        private void DelPatent(string patentName)
        {
            for (int i = 0; i < _libraryUnits.Count; i++)
                if (_libraryUnits[i] is Patent && _libraryUnits[i].NameVal == patentName)
                    _libraryUnits.RemoveAt(i);
        }

        /// <summary>
        /// Очистка фонда библиотеки
        /// </summary>
        private void ClearLibrary()
        {
            _libraryUnits.Clear();
        }

        /// <summary>
        /// Выгрузка данных из фонда в файл txt
        /// </summary>
        /// <param name="path"></param>
        private void SaveTxt(string path)
        {
            string[] units = new string[_libraryUnits.Count];
            int i = 0;

            foreach (LibraryUnit u in _libraryUnits)
            {
                units[i] = u.ToString();
                i++;
            }

            File.WriteAllLines(path, units);
        }

        /// <summary>
        /// Загрузка данных в фонд из файла txt
        /// </summary>
        /// <param name="path"></param>
        /// <param name="loadMethod"></param>
        private bool LoadTxt(string path, int loadMethod)
        {
            var units = new List<LibraryUnit>();

            int lineIndex = 0;
            int succeedLine = 0;
            int failedLine = 0;

            using (var logFile = new StreamWriter(path + ".log"))
            {
                string[] loadedLines = File.ReadAllLines(path);

                foreach (string s in loadedLines)
                {
                    lineIndex++;
                    try
                    {
                        string[] properties = s.Split('|');

                        LibraryUnit unit;

                        if (properties[0].Equals("Book"))
                        {
                            unit = new Book(properties[1], int.Parse(properties[2]), properties[4], properties[5], int.Parse(properties[6]), properties[8].Split('^'), properties[9]);
                            ((Book)unit).ReaderVal = properties[7];
                        }
                        else if (properties[0].Equals("Newspaper"))
                        {
                            unit = new Newspaper(properties[1], int.Parse(properties[2]), properties[4], properties[5], int.Parse(properties[6]), int.Parse(properties[8]), DateTime.ParseExact(properties[9], "ddMMyyyy", null), properties[10]);
                            ((Newspaper)unit).ReaderVal = properties[7];
                        }
                        else if (properties[0].Equals("Patent"))
                        {
                            unit = new Patent(properties[1], int.Parse(properties[2]), properties[4], properties[5], properties[6], DateTime.ParseExact(properties[7], "ddMMyyyy", null), DateTime.ParseExact(properties[8], "ddMMyyyy", null));
                        }
                        else
                        {
                            throw new Exception("Unknown object");
                        }

                        List<string> nootList = new List<string>();
                        foreach (string str in properties[3].Split('^'))
                            nootList.Add(str);

                        unit.RewriteNote(nootList);

                        units.Add(unit);
                        succeedLine++;
                    }
                    catch (Exception e)
                    {
                        logFile.WriteLine("{0} line - load exception: {1}", lineIndex, e.Message);
                        failedLine++;

                        if (loadMethod == 0)
                        {
                            logFile.WriteLine("Load stoped!");
                            return false;
                        }
                    }
                }

                if (units.Count > 0)
                {
                    ClearLibrary();
                    foreach (LibraryUnit u in units)
                        AddUnit(u);
                }

                logFile.WriteLine("---" + Environment.NewLine
                                 + "{0} - all processed lines " + Environment.NewLine
                                 + "{1} - succeed load " + Environment.NewLine
                                 + "{2} - failed load "
                                 , lineIndex, succeedLine, failedLine
                                 );
            }

            return true;
        }

        /// <summary>
        /// Выгрузка данных из фонда (сериализация в XML)
        /// </summary>
        /// <param name="path"></param>
        private void SaveXML(string path)
        {
            using (var writer = new StreamWriter(path))
            {
                SerializeLibraryUnits serializeLibraryUnits = new SerializeLibraryUnits();
                serializeLibraryUnits.dateSave = DateTime.Today;
                serializeLibraryUnits.libraryUnits = _libraryUnits.ToArray();
                var serializer = new XmlSerializer(typeof(SerializeLibraryUnits));
                serializer.Serialize(writer, serializeLibraryUnits);
            }
        }

        /// <summary>
        /// Загрузка данных в фонда (десериализация из XML)
        /// </summary>
        /// <param name="path"></param>
        /// <param name="loadMethod"></param>
        private bool LoadXML(string path, int loadMethod)
        {
            int objIndex = 0;
            int succeed = 0;
            int failed = 0;

            using (var logFile = new StreamWriter(path + ".log"))
            {
                using (var reader = XmlReader.Create(path, _XMLValidator.SettingsVal))
                {
                    try
                    {
                        SerializeLibraryUnits deserializeUnit = (SerializeLibraryUnits)new XmlSerializer(typeof(SerializeLibraryUnits)).Deserialize(reader);

                        var units = new List<LibraryUnit>();

                        foreach (LibraryUnit u in deserializeUnit.libraryUnits)
                            try
                            {
                                objIndex++;
                                units.Add(u);
                                succeed++;
                            }
                            catch (Exception e)
                            {
                                logFile.WriteLine("{0} object - load exception: {1}", objIndex, e.Message);
                                failed++;

                                if (loadMethod == 0)
                                {
                                    logFile.WriteLine("Load stoped!");
                                    return false;
                                }
                            }

                        if (units.Count > 0)
                        {
                            ClearLibrary();
                            foreach (LibraryUnit u in units)
                                AddUnit(u);
                        }
                    }
                    catch (Exception e)
                    {
                        logFile.WriteLine("Error deserialize: {0}", e.Message);
                        return false;
                    }
                }

                logFile.WriteLine("---" + Environment.NewLine
                 + "{0} - all processed object " + Environment.NewLine
                 + "{1} - succeed load " + Environment.NewLine
                 + "{2} - failed load "
                 , objIndex, succeed, failed
                 );
            }

            return true;
        }
    }
}
