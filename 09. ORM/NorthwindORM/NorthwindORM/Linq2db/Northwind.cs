using LinqToDB;
using LinqToDB.Data;

namespace NorthwindORM.Linq2db
{
    public class Northwind : DataConnection
    {
        public ITable<Category> Categories { get { return GetTable<Category>(); } }
        public ITable<Employee> Employees { get { return GetTable<Employee>(); } }
        public ITable<Product> Products { get { return GetTable<Product>(); } }
        public ITable<Supplier> Suppliers { get { return GetTable<Supplier>(); } }
        public ITable<EmployeeTerritories> EmployeeTerritories { get { return GetTable<EmployeeTerritories>(); } }
        public ITable<Order> Orders { get { return GetTable<Order>(); } }
        public ITable<Territory> Territories { get { return GetTable<Territory>(); } }
        public ITable<OrderDetail> OrderDetails { get { return GetTable<OrderDetail>(); } }
        public ITable<Customer> Customers { get { return GetTable<Customer>(); } }

        public Northwind() : base("Northwind")
        {
            LinqToDB.Common.Configuration.Linq.AllowMultipleQuery = true;
        }
    }
}
