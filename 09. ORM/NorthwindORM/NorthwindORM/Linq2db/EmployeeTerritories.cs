using LinqToDB.Mapping;

namespace NorthwindORM.Linq2db
{
    [Table("EmployeeTerritories")]
    public class EmployeeTerritories
    {
        [Association(ThisKey = "EmployeeID", OtherKey = "Id")]
        public Employee Employee { get; set; }

        [Column("EmployeeID"), PrimaryKey]
        public int EmployeeID { get; set; }

        [Association(ThisKey = "TerritoryID", OtherKey = "Id")]
        public Territory Territory { get; set; }

        [Column("TerritoryID"), PrimaryKey]
        public string TerritoryID { get; set; }
    }
}
