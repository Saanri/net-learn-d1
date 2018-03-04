namespace NorthwindEF.Migration.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using NorthwindEF.Migration.ModelDB;

    internal sealed class Configuration : DbMigrationsConfiguration<NorthwindEF.Migration.ModelDB.NorthwindDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "NorthwindEF.Migration.ModelDB.NorthwindDB";
        }

        protected override void Seed(NorthwindEF.Migration.ModelDB.NorthwindDB context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            context.Categories.AddOrUpdate
                (new Category { CategoryName = "Beverages" },
                 new Category { CategoryName = "Condiments" },
                 new Category { CategoryName = "Confections" },
                 new Category { CategoryName = "Dairy Products" },
                 new Category { CategoryName = "Grains / Cereals" },
                 new Category { CategoryName = "Meat / Poultry" },
                 new Category { CategoryName = "Produce" },
                 new Category { CategoryName = "Seafood" }
                );

            context.Regions.AddOrUpdate
                (new Region { RegionDescription = "Eastern" },
                 new Region { RegionDescription = "Western" },
                 new Region { RegionDescription = "Northern" },
                 new Region { RegionDescription = "Southern" }
                );

            context.Territories.AddOrUpdate
                (new Territory { TerritoryDescription = "Westboro" },
                 new Territory { TerritoryDescription = "Bedford" },
                 new Territory { TerritoryDescription = "Georgetow" },
                 new Territory { TerritoryDescription = "Providence" },
                 new Territory { TerritoryDescription = "Rockville" }
                );
        }
    }
}
