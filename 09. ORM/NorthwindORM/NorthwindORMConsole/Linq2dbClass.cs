using System;
using System.Linq;
using LinqToDB;
using NorthwindORM.Linq2db;

namespace NorthwindORMConsole
{
    public static class Linq2dbClass
    {
        public static void Linq2dbConsole()
        {
            using (var con = new Northwind())
            {
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("Список продуктов с категорией и поставщиком:");
                var products = from p in con.Products
                               select new { p.Name, CategoryName = p.Category.Name, SupplierName = p.Supplier.Name };

                Console.WriteLine("[продукт], [категория], [поставщик]");
                foreach (var p in products)
                    Console.WriteLine(" - {0}, {1}, {2}", p.Name, p.CategoryName, p.SupplierName);

                Console.WriteLine();
                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine("Список сотрудников с указанием региона, за который они отвечают:");
                var empReg = (from et in con.EmployeeTerritories
                              select new { EmployeeName = (et.Employee.FirstName + " " + et.Employee.LastName), RegionDescription = (et.Territory.Region.Description) }
                             ).Distinct();

                Console.WriteLine("[сотрудник], [регион]");
                foreach (var er in empReg)
                    Console.WriteLine(" - {0}, {1}", er.EmployeeName, er.RegionDescription);

                Console.WriteLine();
                Console.WriteLine("-----------------------------------------------------------");
                Console.WriteLine("Статистики по регионам: количества сотрудников по регионам:");
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

                Console.WriteLine("[регион], [количество сотрудников]");
                foreach (var rs in regStat)
                    Console.WriteLine(" - {0} , {1}", rs.RegionDescription, rs.EmployeeCount);

                Console.WriteLine();
                Console.WriteLine("-----------------------------------------------------------------------------");
                Console.WriteLine("Список «сотрудник – с какими грузоперевозчиками работал» (на основе заказов):");
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

                Console.WriteLine("[Сотрудник] : [грузоперевозчик]");
                foreach (var es in empShip)
                    Console.WriteLine(" - {0} : {1}", es.EmployeeName, es.Ships);

                Console.WriteLine();
                Console.WriteLine("---------------------------------------------------------------");
                Console.WriteLine("Добавить нового сотрудника, и указать ему список территорий, за");
                Console.WriteLine("которые он несет ответственность:");
                var employee = new Employee() { FirstName = "FirstName", LastName = "LastName" };
                int newEmployeeID = Convert.ToInt32(con.InsertWithIdentity(employee));
                Console.WriteLine("... добавлен новый сотрудник с Id = {0}", newEmployeeID);

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

                Console.WriteLine("... добавлены территории, за которые отвечает сотрудник:");
                Console.WriteLine("[Сотрудник], [Территория], [Регион]");
                var empReg1 = (from et in con.EmployeeTerritories
                               where et.EmployeeID == newEmployeeID
                               select new { EmployeeName = (et.Employee.FirstName + " " + et.Employee.LastName), TerritoryDescription = (et.Territory.Description), RegionDescription = (et.Territory.Region.Description) }
                              ).Distinct();
                foreach (var er in empReg1)
                    Console.WriteLine(" - {0}, {1}, {2}", er.EmployeeName, er.TerritoryDescription, er.RegionDescription);

                Console.WriteLine();
                Console.WriteLine("----------------------------------------------");
                Console.WriteLine("Перенести продукты из одной категории в другую...");
                var newProduct = new Product() { Name = "New product", SupplierID = 1, CategoryID = 1 };
                int newProductId = Convert.ToInt32(con.InsertWithIdentity(newProduct));
                Console.WriteLine("... создан новый продукт id = {0}", newProductId);

                var product = con.Products.Single(t => t.Id == newProductId);
                Console.WriteLine("... категория по продукту \"{0}\"", con.Categories.Single(t => t.Id == product.CategoryID).Name);
                product.CategoryID = 2;
                con.Update(product);
                product = con.Products.Single(t => t.Id == newProductId);
                Console.WriteLine("... категория по продукту изменена, теперь категория по продукту \"{0}\"", con.Categories.Single(t => t.Id == product.CategoryID).Name);

                Console.WriteLine();
                Console.WriteLine("--------------------------------------------------------------");
                Console.WriteLine("Добавить список продуктов со своими поставщиками и категориями");
                Console.WriteLine("(массовое занесение), при этом если поставщик или категория с");
                Console.WriteLine("таким названием есть, то использовать их – иначе создать");
                Console.WriteLine("новые. ");
                string[] newSuplierNames = new string[] { "newSupplier1", "newSupplier2", "newSupplier3" };
                string[] newCategoryNames = new string[] { "newCategory1", "newCategory2", "newCategory3" };
                string[] newProductNames = new string[] { "newProduct1", "newProduct2", "newProduct3" };

                for (int j = 0; j < newSuplierNames.Count(); j++)
                {
                    int? findSupplierId = con.Suppliers.Where(s => s.Name == newSuplierNames[j]).Select(s => s.Id).Max();
                    int? newSupplierId = findSupplierId.Equals(null) ? Convert.ToInt32(con.InsertWithIdentity(new Supplier() { Name = newSuplierNames[j] })) : findSupplierId;

                    int? findCategoryId = con.Categories.Where(c => c.Name == newCategoryNames[j]).Select(c => c.Id).Max();
                    int? newCategoryId = findCategoryId.Equals(null) ? Convert.ToInt32(con.InsertWithIdentity(new Category() { Name = newCategoryNames[j] })) : findCategoryId;

                    newProductId = Convert.ToInt32(con.InsertWithIdentity(new Product() { Name = newProductNames[j], CategoryID = newCategoryId, SupplierID = newSupplierId }));
                    Console.WriteLine("... создан новый продукт id = {0}", newProductId);
                }

                Console.WriteLine();
                Console.WriteLine("--------------------------------------------------------------");
                Console.WriteLine("Замена продукта на аналогичный: во всех еще неисполненных");
                Console.WriteLine("заказах(считать таковыми заказы, у которых ShippedDate = NULL)");
                Console.WriteLine("заменить один продукт на другой.");
                int productIdReplaceable = con.Products.Select(c => c.Id).Min();
                int productIdForReplace = con.Products.Select(c => c.Id).Max();
                string minCustomerID = con.Customers.Select(c => c.Id).Min();
                int minEmployeeID = con.Employees.Select(c => c.Id).Min();

                int newOrderId = Convert.ToInt32(con.InsertWithIdentity(new Order() { CustomerID = minCustomerID, EmployeeID = minEmployeeID }));
                Console.WriteLine("... создан новый заказ id = {0}", newOrderId);
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
                        Console.WriteLine("... произведена замена продукта с id = {0} на продукт с id = {1}, в заказе id = {2}", productIdReplaceable, productIdForReplace, o.Id);
                    }
                    catch (InvalidOperationException) { }
                }
            }
            Console.ReadKey();
        }
    }
}
