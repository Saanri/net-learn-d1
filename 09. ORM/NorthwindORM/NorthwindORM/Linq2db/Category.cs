using LinqToDB.Mapping;

namespace NorthwindORM.Linq2db
{
    [Table("Categories")]
    public class Category
    {
        [Column("CategoryID"), PrimaryKey, Identity]
        public int? Id { get; set; }

        [Column("CategoryName")]
        public string Name { get; set; }
    }
}
