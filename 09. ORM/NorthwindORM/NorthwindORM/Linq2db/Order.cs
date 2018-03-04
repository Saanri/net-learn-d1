using LinqToDB.Mapping;
using System;

namespace NorthwindORM.Linq2db
{
    [Table("Orders")]
    public class Order
    {
        [Column("OrderID"), Identity, PrimaryKey]
        public int Id { get; set; }

        [Association(ThisKey = "CustomerID", OtherKey = "Id")]
        public Customer Customer { get; set; }

        [Column]
        public string CustomerID { get; set; }

        [Association(ThisKey = "EmployeeID", OtherKey = "Id")]
        public Employee Employee { get; set; }

        [Column]
        public int EmployeeID { get; set; }

        [Column]
        public DateTime? ShippedDate { get; set; }

        [Column("ShipName")]
        public string ShipName { get; set; }
    }
}
