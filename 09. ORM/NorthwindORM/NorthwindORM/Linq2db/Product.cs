using LinqToDB.Mapping;

namespace NorthwindORM.Linq2db
{
    [Table("Products")]
    public class Product
    {
        [Column("ProductID"), Identity, PrimaryKey]
        public int Id { get; set; }

        [Column("ProductName")]
        public string Name { get; set; }

        [Association(ThisKey = "SupplierID", OtherKey = "Id")]
        public Supplier Supplier { get; set; }

        [Column]
        public int? SupplierID { get; set; }

        [Association(ThisKey = "CategoryID", OtherKey = "Id")]
        public Category Category { get; set; }

        [Column]
        public int? CategoryID { get; set; }
    }
}
