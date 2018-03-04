using LinqToDB.Mapping;

namespace NorthwindORM.Linq2db
{
    [Table("Customers")]
    public class Customer
    {
        [Column("CustomerID"), Identity, PrimaryKey]
        public string Id { get; set; }

        [Column]
        public string CompanyName { get; set; }
    }
}
