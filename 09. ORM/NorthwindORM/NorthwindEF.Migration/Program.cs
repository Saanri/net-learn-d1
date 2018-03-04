using NorthwindEF.Migration.ModelDB;

namespace NorthwindEF.Migration
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var con = new NorthwindDB())
            {
                con.Database.Create();
            }
        }
    }
}
