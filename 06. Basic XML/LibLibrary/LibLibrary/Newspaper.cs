using System;
using System.Text.RegularExpressions;

namespace LibLibrary
{
    public class Newspaper : PublicMaterial
    {
        private int _number;      // Номер
        private DateTime _date;   // Дата
        private string _ISSN;     // Международный стандартный номер серийного издания(_ISSN)

        public int NumberVal
        {
            get { return _number; }
            set
            {
                if (value >= 0)
                    _number = value;
                else
                    throw (new ArgumentOutOfRangeException("NumberVal", "Номер должен быть положительным"));
            }
        }

        public DateTime DateVal
        {
            get { return _date; }
            set { _date = value; }
        }

        public string ISSNVal
        {
            get { return _ISSN; }
            set
            {
                if (CheckISSN(value))
                    _ISSN = value;
                else
                    throw (new ArgumentOutOfRangeException("ISSNVal", "Некорректный ISSN"));
            }
        }

        public Newspaper() : base() { }

        public Newspaper(string name, int pageCount, string publicationPlace, string publisher, int publicationYear, int number, DateTime date, string ISSN) : base(name, pageCount, publicationPlace, publisher, publicationYear)
        {
            NumberVal = number;
            DateVal = date;
            ISSNVal = ISSN;
        }

        public override string GetShortInfo()
        {
            return string.Format("Газета: \"{0}\"; Номер: {1}; Количество страниц: {2}"
                                , NameVal, _number, pageCountVal
                                );
        }

        public override string ToString()
        {
            return string.Format("Newspaper|{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}"
                                , NameVal, pageCountVal, GetNote(), PublicationPlaceVal, PublisherVal, PublicationYearVal, ReaderVal, NumberVal, DateVal.ToString("ddMMyyyy"), ISSNVal
                                );
        }

        private bool CheckISSN(string ISSN)
        {
            if ((Regex.IsMatch(ISSN, @"^\d{4}-\d{3}[\dxX]$")))
            {
                char[] ISSNArray = ISSN.Replace("-", "").ToCharArray();
                int n = 0
                  , checkDigit = 10;

                if (!ISSNArray[ISSNArray.Length - 1].Equals('X'))
                    checkDigit = Convert.ToInt32(ISSNArray[ISSNArray.Length - 1] - '0');

                for (int i = 0; i < ISSNArray.Length - 1; i++)
                    n += Convert.ToInt32(ISSNArray[i] - '0') * (8 - i);

                if (!((n + checkDigit) % 11).Equals(0)) return false;

                n = 11 - n % 11;

                if (!n.Equals(checkDigit)) return false;

                return true;
            }

            return false;
        }
    }
}
