using LinqToDB.Mapping;

namespace NorthwindORM.Linq2db
{
    [Table("Territories")]
    public class Territory
    {
        [Column("TerritoryID"), Identity, PrimaryKey]
        public string Id { get; set; }

        [Column("TerritoryDescription")]
        public string Description { get; set; }

        [Association(ThisKey = "RegionID", OtherKey = "Id")]
        public Region Region { get; set; }

        [Column]
        public int RegionID { get; set; }
    }
}
