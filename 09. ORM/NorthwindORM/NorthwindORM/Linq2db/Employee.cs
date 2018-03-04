using LinqToDB.Mapping;

namespace NorthwindORM.Linq2db
{
    [Table("Employees")]
    public class Employee
    {
        [Column("EmployeeID"), Identity, PrimaryKey]
        public int Id { get; set; }

        [Column]
        public string LastName { get; set; }

        [Column]
        public string FirstName { get; set; }
    }
}
