// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
//
//Copyright (C) Microsoft Corporation.  All rights reserved.

using SampleSupport;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Task.Data;

// Version Mad01

namespace SampleQueries
{
    [Title("LINQ Module")]
    [Prefix("Linq")]
    public class LinqSamples : SampleHarness
    {

        private DataSource dataSource = new DataSource();

        [Category("Restriction Operators")]
        [Title("Where - Task 1")]
        [Description("This sample uses the where clause to find all elements of an array with a value less than 5.")]
        public void Linq1()
        {
            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            var lowNums =
                from num in numbers
                where num < 5
                select num;

            Console.WriteLine("Numbers < 5:");
            foreach (var x in lowNums)
            {
                Console.WriteLine(x);
            }
        }

        [Category("Restriction Operators")]
        [Title("Where - Task 2")]
        [Description("This sample return return all presented in market products")]
        public void Linq2()
        {
            var products =
                from p in dataSource.Products
                where p.UnitsInStock > 0
                select p;

            foreach (var p in products)
            {
                ObjectDumper.Write(p);
            }
        }

        //-------------------------------------------------------

        [Category("Home work")]
        [Title("HW_01")]
        [Description("Выдайте список всех клиентов, чей суммарный оборот (сумма всех заказов) превосходит некоторую величину X. Продемонстрируйте выполнение запроса с различными X (подумайте, можно ли обойтись без копирования запроса несколько раз)")]
        public void Linq3()
        {
            decimal[] xArray = { 30000.00m, 50000.00m, 100000.00m };

            foreach (decimal x in xArray)
            {
                ObjectDumper.Write(x);

                var query =
                   from c in (from c in dataSource.Customers
                              select new
                              {
                                  c.CustomerID,
                                  c.CompanyName,
                                  sumTotal = (from o in c.Orders select o.Total).Sum()
                              }
                    )
                   where c.sumTotal >= x
                   select new { c.CustomerID, c.CompanyName, c.sumTotal }
                ;

                foreach (var q in query)
                    ObjectDumper.Write(q);
            }
        }

        [Category("Home work")]
        [Title("HW_02")]
        [Description("Для каждого клиента составьте список поставщиков, находящихся в той же стране и том же городе.Сделайте задания с использованием группировки и без.")]
        public void Linq4()
        {
            var query1 =
                from c in dataSource.Customers
                join s in dataSource.Suppliers on new { c.City, c.Country } equals new { s.City, s.Country } into cs
                from s in cs.DefaultIfEmpty()
                orderby c.CustomerID
                select new
                {
                    c.CustomerID,
                    c.CompanyName,
                    c.Country,
                    c.City,
                    cs
                };
            ;

            foreach (var q in query1)
            {
                ObjectDumper.Write(q);
                ObjectDumper.Write("Поставщики:");
                foreach (var s in q.cs)
                    ObjectDumper.Write(s);
                ObjectDumper.Write("");
            }

        }

        [Category("Home work")]
        [Title("HW_03")]
        [Description("Найдите всех клиентов, у которых были заказы, превосходящие по сумме величину X")]
        public void Linq5()
        {
            decimal x = 5500.00m;

            var query =
                (from c in dataSource.Customers
                 from o in c.Orders
                 where o.Total > x
                 select new { c.CustomerID, c.CompanyName }
                ).Distinct()
            ;

            foreach (var q in query)
                ObjectDumper.Write(q);
        }


        [Category("Home work")]
        [Title("HW_04")]
        [Description("Выдайте список клиентов с указанием, начиная с какого месяца какого года они стали клиентами (принять за таковые месяц и год самого первого заказа)")]
        public void Linq6()
        {
            var query =
               from c in (from c in dataSource.Customers
                          from o in c.Orders
                          orderby c.CustomerID, o.OrderDate
                          select new { c.CustomerID, c.CompanyName, o.OrderDate }
                         )
               group c by new { c.CustomerID, c.CompanyName } into cGroup
               select new
               {
                   CustomerID = cGroup.Key.CustomerID,
                   CompanyName = cGroup.Key.CompanyName,
                   OrderYear = (from d in cGroup select d.OrderDate).Min().Year,
                   OrderMonth = (from d in cGroup select d.OrderDate).Min().Month
               }
            ;

            foreach (var q in query)
                ObjectDumper.Write(q);
        }

        [Category("Home work")]
        [Title("HW_05")]
        [Description("Сделайте предыдущее задание, но выдайте список отсортированным по году, месяцу, оборотам клиента(от максимального к минимальному) и имени клиента")]
        public void Linq7()
        {
            var query =
               from c in (from c in (from c in dataSource.Customers
                                     from o in c.Orders
                                     orderby c.CustomerID, o.OrderDate
                                     select new { c.CustomerID, c.CompanyName, o.OrderDate, o.Total }
                                    )
                          group c by new { c.CustomerID, c.CompanyName } into cGroup
                          select new
                          {
                              OrderYear = (from d in cGroup select d.OrderDate).Min().Year,
                              OrderMonth = (from d in cGroup select d.OrderDate).Min().Month,
                              OrderTotal = (from o in cGroup select o.Total).Sum(),
                              cGroup.Key.CompanyName,
                              cGroup.Key.CustomerID
                          }
                         )
               orderby c.OrderYear, c.OrderMonth, c.OrderTotal descending, c.CompanyName
               select c
            ;

            foreach (var q in query)
                ObjectDumper.Write(q);
        }

        private bool IsMatch(string s, string mask)
        {
            if (s == null) return false;

            return Regex.IsMatch(s, mask);
        }

        [Category("Home work")]
        [Title("HW_06")]
        [Description("Укажите всех клиентов, у которых указан нецифровой почтовый код или не заполнен регион или в телефоне не указан код оператора(для простоты считаем, что это равнозначно «нет круглых скобочек в начале»).")]
        public void Linq8()
        {
            var query = from c in dataSource.Customers
                        where !IsMatch(c.PostalCode, @"^[0-9]*$") || c.Region == null || !IsMatch(c.Phone, @"^[\(]\d*[/)]")
                        select new { c.CustomerID, c.CompanyName, c.PostalCode, c.Region, c.Phone }
            ;

            foreach (var q in query)
                ObjectDumper.Write(q);
        }

        [Category("Home work")]
        [Title("HW_07")]
        [Description("Сгруппируйте все продукты по категориям, внутри – по наличию на складе, внутри последней группы отсортируйте по стоимости")]
        public void Linq9()
        {
            var query = from p in dataSource.Products
                        group p by p.Category into CategoryGroup
                        select new
                        {
                            Category = CategoryGroup.Key,
                            IsInStock = from pp in CategoryGroup
                                        group pp by pp.UnitsInStock == 0 into IsInStockGroup
                                        select new
                                        {
                                            IsInStock = IsInStockGroup.Key ? "нет в наличии" : "в наличии",
                                            Products = IsInStockGroup
                                        }
                        }
            ;

            foreach (var q in query)
            {
                ObjectDumper.Write(q.Category);
                foreach (var s in q.IsInStock)
                {
                    ObjectDumper.Write(s.IsInStock);
                    foreach (var p in s.Products)
                        ObjectDumper.Write(p);
                }
                ObjectDumper.Write("");
            }
        }

        [Category("Home work")]
        [Title("HW_08")]
        [Description("Сгруппируйте товары по группам «дешевые», «средняя цена», «дорогие». Границы каждой группы задайте сами")]
        public void Linq10()
        {
            var priceMin = (from p in dataSource.Products
                            select p.UnitPrice
                           ).Min()
            ;

            var priceMax = (from p in dataSource.Products
                            select p.UnitPrice
                           ).Max()
            ;

            var fronVal = (priceMax - priceMin) / 3;

            var query = from p in dataSource.Products
                        group p by p.UnitPrice >= (priceMin + fronVal * 2) ? "дорогие" : p.UnitPrice >= (priceMin + fronVal) ? "средняя цена" : "дешевые" into PriceGroup
                        select new
                        {
                            PriceGroup = PriceGroup.Key,
                            Products = from pg in PriceGroup orderby pg.UnitPrice select pg
                        }
            ;

            foreach (var q in query)
            {
                ObjectDumper.Write(q.PriceGroup);
                foreach (var p in q.Products)
                    ObjectDumper.Write(p);
                ObjectDumper.Write("");
            }
        }

        [Category("Home work")]
        [Title("HW_09")]
        [Description("Рассчитайте среднюю прибыльность каждого города(среднюю сумму заказа по всем клиентам из данного города) и среднюю интенсивность(среднее количество заказов, приходящееся на клиента из каждого города)")]
        public void Linq11()
        {
            var query = from co in (from c in dataSource.Customers
                                    from o in c.Orders
                                    select new { c.City, c.CustomerID, o.Total }
                                   )
                        group co by co.City into CityGroup
                        orderby CityGroup.Key
                        select new
                        {
                            City = CityGroup.Key,
                            AvgOrderSum = (from cg in CityGroup select cg.Total).Average(),
                            AvgOrderCount = (from cg in CityGroup
                                             group cg by cg.CustomerID into CustGroup
                                             select (from cug in CustGroup
                                                     select cug.CustomerID
                                                    ).Count()
                                            ).Average()
                        }
            ;

            foreach (var q in query)
                ObjectDumper.Write(q);
        }

        [Category("Home work")]
        [Title("HW_10")]
        [Description("Сделайте среднегодовую статистику активности клиентов по месяцам(без учета года), статистику по годам, по годам и месяцам(т.е.когда один месяц в разные годы имеет своё значение).")]
        public void Linq12()
        {
            ObjectDumper.Write("Среднегодовая сумма продаж всех клиентов по месяцам(без учета года)");
            var query1 = from co in (from c in dataSource.Customers
                                     from o in c.Orders
                                     select new { c.CustomerID, o.OrderDate, o.Total }
                                    )
                         group co by co.OrderDate.Month into MonthGroup
                         orderby MonthGroup.Key
                         select new
                         {
                             Month = MonthGroup.Key,
                             AvgYearStatByMonth = (from mg in MonthGroup
                                                   group mg by mg.OrderDate.Year into YearGroup
                                                   select (from yg in YearGroup
                                                           select yg.Total
                                                          ).Sum()
                                                  ).Average()
                         }
            ;

            foreach (var q in query1)
                ObjectDumper.Write(q);

            ObjectDumper.Write("Всего продано по годам");
            var query2 = from co in (from c in dataSource.Customers
                                     from o in c.Orders
                                     select new { c.CustomerID, o.OrderDate, o.Total }
                                    )
                         group co by co.OrderDate.Year into YearGroup
                         orderby YearGroup.Key
                         select new
                         {
                             Year = YearGroup.Key,
                             Sum = (from yg in YearGroup
                                    select yg.Total
                                   ).Sum()
                         }
            ;

            foreach (var q in query2)
                ObjectDumper.Write(q);

            ObjectDumper.Write("Всего продано по годам и месяцам");
            var query3 = from co in (from c in dataSource.Customers
                                     from o in c.Orders
                                     select new { c.CustomerID, o.OrderDate, o.Total }
                                    )
                         group co by new { co.OrderDate.Year, co.OrderDate.Month } into YMGroup
                         orderby YMGroup.Key.Year, YMGroup.Key.Month
                         select new
                         {
                             YMGroup.Key.Year,
                             YMGroup.Key.Month,
                             Sum = (from yg in YMGroup
                                    select yg.Total
                                   ).Sum()
                         }
            ;

            foreach (var q in query3)
                ObjectDumper.Write(q);

        }

    }
}
