using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace BooksSerializer
{
    public static class Serializer
    {
        /// <summary>
        /// Выгрузка данных (сериализация в XML)
        /// </summary>
        /// <param name="catalog"></param>
        /// <param name="path"></param>
        public static void SaveXML(Catalog catalog, string path)
        {
            using (var writer = new StreamWriter(path))
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "http://library.by/catalog");

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Catalog));
                xmlSerializer.Serialize(writer, catalog, ns);
            }
        }

        /// <summary>
        /// Загрузка данных (десериализация из XML)
        /// </summary>
        /// <param name="path"></param>
        public static Catalog LoadXML(string path)
        {
            using (var reader = new FileStream(path, FileMode.Open))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Catalog), "http://library.by/catalog");
                return xmlSerializer.Deserialize(reader) as Catalog;
            }
        }


    }
}
