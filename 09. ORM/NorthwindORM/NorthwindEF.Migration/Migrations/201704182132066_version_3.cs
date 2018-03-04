namespace NorthwindEF.Migration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version_3 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Region", newName: "Regions");
            AddColumn("dbo.Customers", "FoundingDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "FoundingDate");
            RenameTable(name: "dbo.Regions", newName: "Region");
        }
    }
}
