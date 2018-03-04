using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace LibLibrary
{
    public abstract class LibraryUnit : ILibrable
    {
        private string _name;                   // Название
        private int _pageCount;                 // Количество страниц
        private List<string> _note;             // Примечание
       // internal bool where;

        public string NameVal
        {
            get { return _name; }
            set
            {
                if (value.Length > 0)
                    _name = value;
                else
                    throw (new ArgumentNullException("NameVal", "Название не может быть пустым"));
            }
        }

        public int pageCountVal
        {
            get { return _pageCount; }
            set { _pageCount = value; }
        }

        public List<string> NoteVal
        {
            get { return _note; }
            set { _note = value; }
        }

        public void WriteNote(string note)
        {
            _note.Add(string.Format("{0} : {1}", DateTime.Now, note));
        }

        public void RewriteNote(List<string> note)
        {
            _note.Clear();
            foreach (string s in note)
                _note.Add(s);
        }

        public LibraryUnit() { }

        public LibraryUnit(string name, int pageCount)
        {
            NameVal = name;
            pageCountVal = pageCount;
            _note = new List<string>();
            WriteNote("добавлено");
        }

        protected string GetNote()
        {
            StringBuilder res = new StringBuilder();
            foreach (string s in _note)
                res.Append(res.Length.Equals(0) ? s : "^" + s);

            return res.ToString();
        }

        public abstract string GetShortInfo();
        public override abstract string ToString();
        public abstract int GetPublicationYear();
    }
}
