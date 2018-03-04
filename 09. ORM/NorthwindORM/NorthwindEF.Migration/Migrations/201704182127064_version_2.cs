namespace NorthwindEF.Migration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version_2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmployeeCreditCards",
                c => new
                    {
                        CreditCardsID = c.Int(nullable: false, identity: true),
                        EndDate = c.DateTime(),
                        CardHolderName = c.String(maxLength: 50),
                        EmployeeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CreditCardsID)
                .ForeignKey("dbo.Employees", t => t.EmployeeID)
                .Index(t => t.EmployeeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmployeeCreditCards", "EmployeeID", "dbo.Employees");
            DropIndex("dbo.EmployeeCreditCards", new[] { "EmployeeID" });
            DropTable("dbo.EmployeeCreditCards");
        }
    }
}
