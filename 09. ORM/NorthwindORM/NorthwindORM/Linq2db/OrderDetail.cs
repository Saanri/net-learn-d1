using LinqToDB.Mapping;

namespace NorthwindORM.Linq2db
{
    [Table("[Order Details]")]
    public class OrderDetail
    {
        [Association(ThisKey = "OrderID", OtherKey = "Id")]
        public Order Order { get; set; }

        [Column, PrimaryKey]
        public int OrderID { get; set; }

        [Association(ThisKey = "ProductID", OtherKey = "Id")]
        public Product Product { get; set; }

        [Column, PrimaryKey]
        public int ProductID { get; set; }

        [Column]
        public int UnitPrice { get; set; }

        [Column]
        public int Quantity { get; set; }

        [Column]
        public float Discount { get; set; }
    }
}
