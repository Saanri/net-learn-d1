using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindLibrary;
using System.Linq;
using System.Threading;
using System.Data.SqlClient;
using System.Configuration;


namespace CachingSolutionsSamples
{
    [TestClass]
    public class CacheTests
    {
        private string _user;

        [TestInitialize]
        public void Setup()
        {
            _user = Thread.CurrentPrincipal.Identity.Name;
        }

        [TestMethod]
        public void СategorieMemoryCacheTimeoutTest()
        {
            DataMemoryCache dataMemoryCache = new DataMemoryCache("Сategorie_");
            DataManager dataManager = new DataManager(dataMemoryCache);

            var categories = dataManager.GetData<Category>();
            var categoriesFromCash = dataMemoryCache.Get<Category>(_user);

            Assert.IsTrue(categories.Any());
            Assert.AreEqual(categories, categoriesFromCash);

            Thread.Sleep(7000);
            Assert.IsNull(dataMemoryCache.Get<Category>(_user));
        }

        [TestMethod]
        public void СategorieMemoryCacheSQLMonitorTest()
        {
            string connectionStrings = ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString;
            string commandString = "select CategoryID, CategoryName From dbo.Categories";

            DataMemoryCache dataMemoryCache = new DataMemoryCache("Сategorie_");
            DataManager dataManager = new DataManager(dataMemoryCache, connectionStrings, commandString);

            var categories = dataManager.GetData<Category>();
            var categoriesFromCash = dataMemoryCache.Get<Category>(_user);

            Assert.IsTrue(categories.Any());
            Assert.AreEqual(categories, categoriesFromCash);
            
            using (SqlConnection connection = new SqlConnection(connectionStrings))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = "update dbo.Categories set CategoryName = 'MyCat88' where CategoryID = 11";
                command.ExecuteNonQuery();
            }
            
            Thread.Sleep(1000);
            categoriesFromCash = dataMemoryCache.Get<Category>(_user);
            Assert.IsNull(categoriesFromCash);
        }

        [TestMethod]
        public void СategorieRedisCacheTimeoutTest()
        {
            DataRedisCache<Category> dataRedisCache = new DataRedisCache<Category>("localhost", "Сategorie_");
            DataManager dataManager = new DataManager(dataRedisCache);

            var categories = dataManager.GetData<Category>();
            var categoriesFromCash = dataRedisCache.Get<Category>(_user);

            Assert.IsTrue(categories.Any());
            Assert.IsTrue(categoriesFromCash.Any());
            Assert.IsTrue(categories.Count().Equals(categoriesFromCash.Count()));

            Thread.Sleep(7000);
            Assert.IsNull(dataRedisCache.Get<Category>(_user));
        }

        [TestMethod]
        public void EmployeeMemoryCacheTest()
        {
            DataMemoryCache dataMemoryCache = new DataMemoryCache("Employee_");
            DataManager dataManager = new DataManager(dataMemoryCache);

            var employees = dataManager.GetData<Employee>();
            var employeesFromCash = dataMemoryCache.Get<Employee>(_user);

            Assert.IsTrue(employees.Any());
            Assert.AreEqual(employees, employeesFromCash);
        }

        [TestMethod]
        public void CustomerRedisCacheSQLMonitorTest()
        {
            string connectionStrings = ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString;
            string commandString = "select CustomerID, CompanyName From dbo.Customers";

            DataRedisCache<Customer> dataRedisCache = new DataRedisCache<Customer>("localhost", "Customer_");
            DataManager dataManager = new DataManager(dataRedisCache, connectionStrings, commandString);

            var customers = dataManager.GetData<Customer>();
            var customersFromCash = dataRedisCache.Get<Customer>(_user);

            Assert.IsTrue(customers.Any());
            Assert.IsTrue(customersFromCash.Any());
            Assert.IsTrue(customers.Count().Equals(customersFromCash.Count()));
            
            using (SqlConnection connection = new SqlConnection(connectionStrings))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = "update dbo.Customers set CompanyName = 'Wolski  Zajazd+' where CustomerID = 'WOLZA'";
                command.ExecuteNonQuery();
            }
            
            Thread.Sleep(1000);
            customersFromCash = dataRedisCache.Get<Customer>(_user);
            Assert.IsNull(customersFromCash);
        }

    }
}
