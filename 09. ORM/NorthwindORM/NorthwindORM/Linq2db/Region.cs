using LinqToDB.Mapping;

namespace NorthwindORM.Linq2db
{
    [Table]
    public class Region
    {
        [Column("RegionID"), Identity, PrimaryKey]
        public int Id { get; set; }

        [Column("RegionDescription")]
        public string Description { get; set; }
    }
}
