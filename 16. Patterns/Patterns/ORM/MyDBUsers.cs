using System.Data.Entity;

namespace ORM
{
    public class MyDBUsers : DbContext
    {
        public MyDBUsers()
            : base("MyDBUsers")
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }
}
