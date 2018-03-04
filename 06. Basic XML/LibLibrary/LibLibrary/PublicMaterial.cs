using System;
using System.Xml.Serialization;

namespace LibLibrary
{
	public abstract class PublicMaterial : LibraryUnit, IReadable
    {
        private string _publicationPlace;   // Место издания(город)
        private string _publisher;          // Название издательства
        private int _publicationYear;       // Год издания
        private string _reader;             // Читатель, на текущий момент

        public string PublicationPlaceVal
        {
            get { return _publicationPlace; }
            set
            {
                if (value.Length > 0)
                    _publicationPlace = value;
                else
                    throw (new ArgumentNullException("PublicationPlaceVal", "Место издания(город) не может быть пустым"));
            }
        }

        public string PublisherVal
        {
            get { return _publisher; }
            set
            {
                if (value.Length > 0)
                    _publisher = value;
                else
                    throw (new ArgumentNullException("PublisherVal", "Название издательства не может быть пустым"));
            }
        }

        public int PublicationYearVal
        {
            get { return _publicationYear; }
            set
            {
                if (value >= 1900)
                    _publicationYear = value;
                else
                    throw (new ArgumentOutOfRangeException("PublicationYearVal", "У нас нет изданий до 1900 года"));
            }
        }

        public string ReaderVal
        {
            get { return _reader; }
            set { _reader = value; }
        }

        public PublicMaterial() : base() { }

        public PublicMaterial(string name, int pageCount, string publicationPlace, string publisher, int publicationYear) : base (name, pageCount)
        {
            PublicationPlaceVal = publicationPlace;
            PublisherVal = publisher;
            PublicationYearVal = publicationYear;
            ReaderVal = "-";
        }

        public string TakeRead(string readerName)
        {
            ReaderVal = readerName;
            string res = string.Format("{0} взял почитать!", ReaderVal);
            WriteNote(res);

            return res;
        }

        public string Return()
        {
            string res = string.Format("{0} вернул!", ReaderVal);
            ReaderVal = "-";
            WriteNote(res);

            return res;
        }

        public override int GetPublicationYear()
        {
            return PublicationYearVal;
        }
    }
}
