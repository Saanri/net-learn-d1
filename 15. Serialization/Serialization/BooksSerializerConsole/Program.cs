using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BooksSerializer;

namespace BooksSerializerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Catalog catalog = Serializer.LoadXML(@"Data\books.xml");
            Serializer.SaveXML(catalog, @"books.xml");

            //Catalog catalog = new Catalog();
            //catalog.LoadXML(@"Data\books.xml");
            //catalog.SaveXML(@"books.xml");
        }
    }
}
