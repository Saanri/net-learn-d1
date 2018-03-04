using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LinqToDB;
using NorthwindORM.Linq2db;
using LinqToDB.Data;

namespace NorthwindORM.Tests
{
    [TestClass]
    public class Linq2dbTests
    {
        private Northwind con;
        private DataConnectionTransaction tran;

        [TestInitialize]
        public void SetUp()
        {
            con = new Northwind();
            tran = con.BeginTransaction();
        }

        [TestCleanup]
        public void CleanUp()
        {
            tran.Rollback();
        }

        /// <summary>
        /// Список продуктов с категорией и поставщиком
        /// </summary>
        [TestMethod]
        public void Linq2dbTest_ProductListWithCaterorySupplier()
        {
            var products = from p in con.Products
                           select new { p.Name, CategoryName = p.Category.Name, SupplierName = p.Supplier.Name };

            Assert.IsTrue(products.Count() > 0);
        }

        /// <summary>
        /// Список сотрудников с указанием региона, за который они отвечают
        /// </summary>
        [TestMethod]
        public void Linq2dbTest_EmployeeWithRegions()
        {
            var empReg = (from et in con.EmployeeTerritories
                          select new { EmployeeName = (et.Employee.FirstName + " " + et.Employee.LastName), RegionDescription = (et.Territory.Region.Description) }
                         ).Distinct();

            Assert.IsTrue(empReg.Count() > 0);
        }

        /// <summary>
        /// Статистики по регионам: количества сотрудников по регионам
        /// </summary>
        [TestMethod]
        public void Linq2dbTest_CountEmployeeByRegion()
        {
            var regStat = from r in (from et in con.EmployeeTerritories
                                     select new
                                     {
                                         RegionDescription = (et.Territory.Region.Description),
                                         et.EmployeeID
                                     }
                                    ).Distinct()
                          group r by r.RegionDescription into rGroup
                          select new
                          {
                              RegionDescription = rGroup.Key,
                              EmployeeCount = (from e in rGroup select e.EmployeeID).Count()
                          };

            Assert.IsTrue(regStat.Count() > 0);
            foreach (var rs in regStat)
                Assert.IsTrue(rs.EmployeeCount >= 0);
        }

        /// <summary>
        /// Список «сотрудник – с какими грузоперевозчиками работал» (на основе заказов)
        /// </summary>
        [TestMethod]
        public void Linq2dbTest_ShipsOnWhichWorkEmployee()
        {
            int employeeID = 4;
            var empShip = from o in con.Orders
                          where o.EmployeeID == employeeID
                          group o by new
                          {
                              o.EmployeeID,
                              EmployeeName = (o.Employee.FirstName + " " + o.Employee.LastName)
                          } into oGr
                          select new
                          {
                              oGr.Key.EmployeeID,
                              oGr.Key.EmployeeName,
                              Ships = string.Join(", ", oGr.Select(e => e.ShipName))
                          };

            Assert.IsTrue(empShip.Count() > 0);
            foreach (var es in empShip)
                Assert.IsNotNull(es.Ships);
        }

        /// <summary>
        /// Добавить нового сотрудника, и указать ему список территорий, за которые он несет ответственность
        /// </summary>
        [TestMethod]
        public void Linq2dbTest_AddNewEmployeeWithTerritories()
        {
            var employee = new Employee() { FirstName = "FirstName", LastName = "LastName" };
            int newEmployeeID = Convert.ToInt32(con.InsertWithIdentity(employee));
            Assert.IsTrue(newEmployeeID > 0);

            var emplTerr = from et in con.Territories
                           where new[] { "01581", "03049" }.Contains(et.Id)
                           select et
            ;

            EmployeeTerritories[] employeeTerritories = new EmployeeTerritories[emplTerr.Count()];
            int i = 0;
            foreach (var et in emplTerr)
            {
                employeeTerritories[i] = new EmployeeTerritories() { EmployeeID = newEmployeeID, TerritoryID = et.Id };
                i++;
            }

            foreach (var et in employeeTerritories)
                con.InsertWithIdentity(et);

            var empReg1 = (from et in con.EmployeeTerritories
                           where et.EmployeeID == newEmployeeID
                           select new { EmployeeName = (et.Employee.FirstName + " " + et.Employee.LastName), TerritoryDescription = (et.Territory.Description), RegionDescription = (et.Territory.Region.Description) }
                          ).Distinct();
            Assert.IsTrue(empReg1.Count() > 0);
        }

        /// <summary>
        /// Перенести продукты из одной категории в другую
        /// </summary>
        [TestMethod]
        public void Linq2dbTest_ChengeCategoryByProduct()
        {
            var newProduct = new Product() { Name = "New product", SupplierID = 1, CategoryID = 1 };
            int newProductId = Convert.ToInt32(con.InsertWithIdentity(newProduct));
            Assert.IsTrue(newProductId > 0);

            var product = con.Products.Single(t => t.Id == newProductId);
            var oldCategoryId = con.Categories.Single(t => t.Id == product.CategoryID).Id;
            Assert.IsTrue(oldCategoryId > 0);
            product.CategoryID = 2;
            con.Update(product);
            product = con.Products.Single(t => t.Id == newProductId);
            var newCategoryId = con.Categories.Single(t => t.Id == product.CategoryID).Id;
            Assert.IsTrue(newCategoryId > 0);
            Assert.AreNotEqual(oldCategoryId, newCategoryId);
        }

        /// <summary>
        /// Добавить список продуктов со своими поставщиками и категориями (массовое занесение), при этом 
        /// если поставщик или категория с таким названием есть, то использовать их – иначе создать новые
        /// </summary>
        [TestMethod]
        public void Linq2dbTest_AddProductListWithSelfSuppliersAndCategories()
        {
            string[] newSuplierNames = new string[] { "newSupplier1", "newSupplier2", "newSupplier3" };
            string[] newCategoryNames = new string[] { "newCategory1", "newCategory2", "newCategory3" };
            string[] newProductNames = new string[] { "newProduct1", "newProduct2", "newProduct3" };

            for (int j = 0; j < newSuplierNames.Count(); j++)
            {
                int? findSupplierId = con.Suppliers.Where(s => s.Name == newSuplierNames[j]).Select(s => s.Id).Max();
                int? newSupplierId = findSupplierId.Equals(null) ? Convert.ToInt32(con.InsertWithIdentity(new Supplier() { Name = newSuplierNames[j] })) : findSupplierId;

                int? findCategoryId = con.Categories.Where(c => c.Name == newCategoryNames[j]).Select(c => c.Id).Max();
                int? newCategoryId = findCategoryId.Equals(null) ? Convert.ToInt32(con.InsertWithIdentity(new Category() { Name = newCategoryNames[j] })) : findCategoryId;

                var newProductId = Convert.ToInt32(con.InsertWithIdentity(new Product() { Name = newProductNames[j], CategoryID = newCategoryId, SupplierID = newSupplierId }));
                Assert.IsTrue(newProductId > 0);
            }
        }

        /// <summary>
        /// Замена продукта на аналогичный: во всех еще неисполненных заказах(считать таковыми 
        /// заказы, у которых ShippedDate = NULL) заменить один продукт на другой
        /// </summary>
        [TestMethod]
        public void Linq2dbTest_ReplaceProductOnAnalogInNotSheepedOrders()
        {
            int productIdReplaceable = con.Products.Select(c => c.Id).Min();
            int productIdForReplace = con.Products.Select(c => c.Id).Max();
            string minCustomerID = con.Customers.Select(c => c.Id).Min();
            int minEmployeeID = con.Employees.Select(c => c.Id).Min();

            int newOrderId = Convert.ToInt32(con.InsertWithIdentity(new Order() { CustomerID = minCustomerID, EmployeeID = minEmployeeID }));
            Assert.IsTrue(newOrderId > 0);
            con.Insert(new OrderDetail() { OrderID = newOrderId, ProductID = productIdReplaceable, UnitPrice = 1, Quantity = 1, Discount = 0 });

            var orders = con.Orders.Where(o => o.ShippedDate.Equals(null)).ToArray();
            foreach (var o in orders)
            {
                try
                {
                    var orderDet = con.OrderDetails.Single(od => od.OrderID == o.Id && od.ProductID == productIdReplaceable);
                    con.Delete(orderDet);
                    orderDet.ProductID = productIdForReplace;
                    con.Insert(orderDet);
                }
                catch (InvalidOperationException) { }
            }
        }
    }
}
