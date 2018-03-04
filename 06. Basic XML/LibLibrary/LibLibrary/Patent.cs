using System;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace LibLibrary
{
    public class Patent : LibraryUnit
    {
        private string _inventor;            // Изобретатель(и)
        private string _country;             // Страна
        private string _registrationNumber;     // Регистрационный номер
        private DateTime _applicationDate;   // Дата подачи заявки
        private DateTime _publicationDate;   // Дата публикации

        public string InventorVal
        {
            get { return _inventor; }
            set
            {
                if (value.Length > 0)
                    _inventor = value;
                else
                    throw (new ArgumentNullException("InventorVal", "Изобретатель(и) не может быть пустым"));
            }
        }

        public string CountryVal
        {
            get { return _country; }
            set
            {
                if (value.Length > 0)
                    _country = value;
                else
                    throw (new ArgumentNullException("CountryVal", "Страна не может быть пустым"));
            }
        }

        public string RegistrationNumberVal
        {
            get { return _registrationNumber; }
            set
            {
                if (CheckRegistrationNumber(value))
                    _registrationNumber = value;
                else
                    throw (new ArgumentOutOfRangeException("RegistrationNumberVal", "Регистрационный номер"));
            }
        }

        public DateTime ApplicationDateVal
        {
            get { return _applicationDate; }
            set
            {
                if (value.Year >= 1950)
                    _applicationDate = value;
                else
                    throw (new ArgumentOutOfRangeException("ApplicationDateVal", "Дата подачи заявки не ранее 1950 года"));
            }
        }

        public DateTime PublicationDateVal
        {
            get { return _publicationDate; }
            set
            {
                if (value.Year >= 1950)
                    _publicationDate = value;
                else
                    throw (new ArgumentOutOfRangeException("PublicationDateVal", "Дата публикации не ранее 1950 года"));
            }
        }

        public Patent() : base() { }

        public Patent(string name, int pageCount, string inventor, string country, string registrationNumber, DateTime applicationDate, DateTime publicationDate) : base(name, pageCount)
        {
            InventorVal = inventor;
            CountryVal = country;
            RegistrationNumberVal = registrationNumber;
            ApplicationDateVal = applicationDate;
            PublicationDateVal = publicationDate;
        }

        public override string GetShortInfo()
        {
            return string.Format("Патент: \"{0}\"; Изобретатель(и): {1}; Страна: {2}; Регистрационный номер: {3}"
                                , NameVal, _inventor, _country, _registrationNumber
                                );
        }

        public override string ToString()
        {
            return string.Format("Patent|{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}"
                                , NameVal, pageCountVal, GetNote(), InventorVal, CountryVal, RegistrationNumberVal, ApplicationDateVal.ToString("ddMMyyyy"), PublicationDateVal.ToString("ddMMyyyy")
                                );
        }

        public override int GetPublicationYear()
        {
            return PublicationDateVal.Year;
        }

        private bool CheckRegistrationNumber(string registrationNumber)
        {
            if (Regex.IsMatch(registrationNumber, @"^[1-9]\d{5,6}$" // Utility
                                                + "|" + @"^[Re][Ee]\d{6}$" // Reissue
                                                + "|" + @"^[Pp]{2}\d{6}$" // Plant Patents
                                                + "|" + @"^[Dd]\d{7}$" // Design
                                                + "|" + @"^[Aa][Ii]\d{6}$" // Additions of Improvements
                                                + "|" + @"^[Xx]\d{7}$" // X Patents
                                                + "|" + @"^[Hh]\d{7}$" // H Documents
                                                + "|" + @"^[Tt]\d{7}$" // T Documents
                                                + "|" + @"^\d+\-\d{4}\/\d+$"  // Self 1
                             )
               ) return true;

            return false;
        }

    }
}
