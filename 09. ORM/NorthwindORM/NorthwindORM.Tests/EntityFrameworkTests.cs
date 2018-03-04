using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LinqToDB;
using NorthwindORM.EntityFramework;

namespace NorthwindORM.Tests
{
    [TestClass]
    public class EntityFrameworkTests
    {
        /// <summary>
        /// Выборка списка заказов, по одной категории (т.е. те заказы, в которые включены продукты
        /// определенной категории)
        ///	Выборка должна включать:
        /// - Список детальных строк
        /// - Имя заказчика
        /// - Имена продуктов
        /// </summary>
        [TestMethod]
        public void EntityFrameworkTest_SelectOrdersByCategory()
        {
            using (var con = new NorthwindDB())
            {
                var needCategoryID = con.Categories.Select(c => c.CategoryID).Min();

                var orders = (from p in con.Products
                              join od in con.Order_Details on p.ProductID equals od.ProductID
                              join odd in con.Order_Details on od.OrderID equals odd.OrderID
                              where p.CategoryID == needCategoryID
                              select new
                              {
                                  od.Order.OrderID,
                                  od.Order.OrderDate,
                                  od.Order.RequiredDate,
                                  od.Order.ShippedDate,
                                  od.Order.Customer.CompanyName,
                                  odd.Product.ProductName,
                                  odd.Product.Category.CategoryName,
                                  odd.Quantity,
                                  odd.UnitPrice,
                                  odd.Discount
                              }
                             ).Distinct();

                Assert.IsTrue(orders.Count() > 0);
            }
        }
    }
}
