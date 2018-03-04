using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BooksSerializer
{
    [XmlType(TypeName = "book")]
    public class Book
    {
        [XmlAttribute]
        public string id;
        public string isbn;
        public string author;
        public string title;
        public BookGenre genre;
        public string publisher;

        private DateTime _publishDate;
        [XmlElement("publish_date")]
        public string somePublishDate
        {
            get { return _publishDate.ToString("yyyy-MM-dd"); }
            set { _publishDate = DateTime.Parse(value); }
        }

        public string description;

        private DateTime _registrationDate;
        [XmlElement("registration_date")]
        public string someRegistrationDate
        {
            get { return _registrationDate.ToString("yyyy-MM-dd"); }
            set { _registrationDate = DateTime.Parse(value); }
        }
    }

    public enum BookGenre
    {
        [XmlEnum(Name = "Computer")]
        Computer,

        [XmlEnum(Name = "Fantasy")]
        Fantasy,

        [XmlEnum(Name = "Romance")]
        Romance,

        [XmlEnum(Name = "Horror")]
        Horror,

        [XmlEnum(Name = "Science Fiction")]
        ScienceFiction
    }
}
