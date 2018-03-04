using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BooksSerializer
{
    [XmlRoot(Namespace = "http://library.by/catalog"), XmlType(TypeName = "catalog")]
    public class Catalog
    {
        private DateTime _date;
        [XmlAttribute("date")]
        public string valDate
        {
            get { return _date.ToString("yyyy-MM-dd"); }
            set { _date = DateTime.Parse(value); }
        }

        [XmlElement("book")]
        public List<Book> Books;

        public Catalog()
        {
            Books = new List<Book>();
        }

        // не знаю как правильнее... вынести сериализацию в отдельный класс или внутри сделать
        // опробовал оба варианта :)
        /*
        /// <summary>
        /// Выгрузка данных (сериализация в XML)
        /// </summary>
        /// <param name="path"></param>
        public void SaveXML(string path)
        {
            using (var writer = new StreamWriter(path))
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "http://library.by/catalog");

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Catalog));
                xmlSerializer.Serialize(writer, this, ns);
            }
        }

        /// <summary>
        /// Загрузка данных (десериализация из XML)
        /// </summary>
        /// <param name="path"></param>
        public void LoadXML(string path)
        {
            using (var reader = new FileStream(path, FileMode.Open))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Catalog), "http://library.by/catalog");
                Catalog res = xmlSerializer.Deserialize(reader) as Catalog;
                Books = res.Books;
                valDate = res.valDate;
            }
        }
        */

    }

}
