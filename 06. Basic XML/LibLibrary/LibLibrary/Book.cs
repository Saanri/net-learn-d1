using System;
using System.Text;
using System.Text.RegularExpressions;

namespace LibLibrary
{
    public class Book : PublicMaterial
    {
        private string[] _authors;  // Автор(ы)
        private string _ISBN;       // Международный стандартный номер книги(ISBN)

        public string[] AuthorsVal
        {
            get { return _authors; }
            set
            {
                foreach (string s in value)
                    if (!(s.Length > 0))
                        throw (new ArgumentNullException("AuthorsVal[]", "Автор(ы) не может быть пустым"));

                _authors = value;
            }
        }

        public string ISBNVal
        {
            get { return _ISBN; }
            set
            {
                if (CheckISBN(value))
                    _ISBN = value;
                else
                    throw (new ArgumentOutOfRangeException("ISBNVal", "Некорректный ISBN"));
            }
        }

        public Book() : base() { }

        public Book(string name, int pageCount, string publicationPlace, string publisher, int publicationYear, string[] authors, string ISBN) : base(name, pageCount, publicationPlace, publisher, publicationYear)
        {
            AuthorsVal = authors;
            ISBNVal = ISBN;
        }

        public override string GetShortInfo()
        {
            return string.Format("Книга: \"{0}\"; Автор(ы): {1}; Количество страниц: {2}"
                                , NameVal, AuthorsString(), pageCountVal
                                );
        }

        public override string ToString()
        {
            return string.Format("Book|{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}"
                                , NameVal, pageCountVal, GetNote(), PublicationPlaceVal, PublisherVal, PublicationYearVal, ReaderVal, GetAuthors(), ISBNVal
                                );
        }

        private string AuthorsString()
        {
            StringBuilder res = new StringBuilder();
            foreach (string s in _authors)
                res.Append(res.Length.Equals(0) ? s : ", " + s);

            return res.ToString();
        }

        private string GetAuthors()
        {
            StringBuilder res = new StringBuilder();
            foreach (string s in _authors)
                res.Append(res.Length.Equals(0) ? s : "^" + s);

            return res.ToString();
        }

        private bool CheckISBN(string ISBN)
        {
            if ((Regex.IsMatch(ISBN, @"^\d+\-\d+\-\d+\-\d+$")) || (Regex.IsMatch(ISBN, @"^\d+\-\d+\-\d+\-\d+-\d+$")))
            {
                char[] ISBNArray = ISBN.Replace("-", "").ToCharArray();
                int n = 0
                  , checkDigit = Convert.ToInt32(ISBNArray[ISBNArray.Length - 1] - '0');

                if (ISBNArray.Length.Equals(10))
                {
                    for (int i = 0; i < ISBNArray.Length - 1; i++)
                        n += Convert.ToInt32(ISBNArray[i] - '0') * (10 - i);

                    if (!((n + checkDigit) % 11).Equals(0)) return false;

                    n = (11 - n % 11) % 11;
                }
                else if (ISBNArray.Length.Equals(13))
                {
                    for (int i = 0; i < ISBNArray.Length - 1; i++)
                        n += Convert.ToInt32(ISBNArray[i] - '0') * (i % 2 == 0 ? 1 : 3);

                    if (!((n + checkDigit) % 10).Equals(0)) return false;

                    n = (10 - n % 10) % 10;
                }
                else return false;

                if (!n.Equals(checkDigit)) return false;

                return true;
            }

            return false;
        }

    }
}
