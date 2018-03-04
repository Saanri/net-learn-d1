using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB.Mapping;
using LinqToDB.Data;
using LinqToDB;
using NorthwindORM;
using NorthwindORM.Linq2db;

namespace NorthwindORMConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //Linq2dbClass.Linq2dbConsole();
            EntityFrameworkClass.EntityFrameworkConsole();
        }
    }
}
