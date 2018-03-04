using System;
using System.Linq;
using LinqToDB;
using NorthwindORM.EntityFramework;

namespace NorthwindORMConsole
{
    public static class EntityFrameworkClass
    {
        public static void EntityFrameworkConsole()
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

                int lastOrderId = 0;
                foreach (var o in orders)
                {
                    if (o.OrderID != lastOrderId)
                    {
                        Console.WriteLine("-------------------");
                        Console.WriteLine("Заказ №{0} от {1}", o.OrderID, o.OrderDate);
                        Console.WriteLine("Дата подтверждения {0}", o.RequiredDate);
                        Console.WriteLine("Дата доставки {0}", o.ShippedDate);
                        Console.WriteLine("Заказчик \"{0}\"", o.CompanyName);
                        Console.WriteLine("Включает следующие продукты:");
                        lastOrderId = o.OrderID;
                    }
                    Console.WriteLine("   - {0} : {1} | {2} | {3} | {4}", o.ProductName, o.CategoryName, o.Quantity, o.UnitPrice, o.Discount);
                }
            }
            Console.ReadKey();
        }
    }
}
