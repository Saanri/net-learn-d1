using LinqToDB.Mapping;

namespace NorthwindORM.Linq2db
{
    [Table("Suppliers")]
    public class Supplier
    {
        [Column("SupplierID"), Identity, PrimaryKey]
        public int? Id { get; set; }

        [Column("CompanyName")]
        public string Name { get; set; }
    }
}
