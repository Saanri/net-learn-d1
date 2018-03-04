using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Task.DB;
using Task.TestHelpers;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Reflection;
using System.CodeDom;
using System.Collections.ObjectModel;

namespace Task
{
    [TestClass]
    public class SerializationSolutions
    {
        Northwind dbContext;

        [TestInitialize]
        public void Initialize()
        {
            dbContext = new Northwind();
        }

        [TestMethod]
        public void SerializationCallbacks()
        {
            dbContext.Configuration.ProxyCreationEnabled = false;

            var tester = new XmlDataContractSerializerTester<IEnumerable<Category>>(new NetDataContractSerializer(), true);
            var categories = dbContext.Categories.ToList();

            var c = categories.First();

            var res = tester.SerializeAndDeserialize(categories);
            Assert.IsTrue(res.Any());
            foreach (var category in res)
            {
                Assert.IsTrue(category.Products.Any());
            }
        }

        [TestMethod]
        public void ISerializable()
        {
            dbContext.Configuration.ProxyCreationEnabled = false;

            var tester = new XmlDataContractSerializerTester<IEnumerable<Product>>(new NetDataContractSerializer(), true);
            var products = dbContext.Products.Include("Category").ToList();

            var res = tester.SerializeAndDeserialize(products);
            Assert.IsTrue(res.Any());
            foreach (var product in res)
            {
                Assert.IsNotNull(product.Category);
            }
        }

        [TestMethod]
        public void ISerializationSurrogate()
        {
            dbContext.Configuration.ProxyCreationEnabled = false;

            //var tester = new XmlDataContractSerializerTester<IEnumerable<Order_Detail>>(new NetDataContractSerializer(), true);
            //var orderDetails = dbContext.Order_Details.ToList();
            //
            //var res = tester.SerializeAndDeserialize(orderDetails);
            //Assert.IsTrue(res.Any());
            //foreach (var orderDetail in res)
            //{
            //    Assert.IsNotNull(orderDetail.Product);
            //}

            SurrogateSelector selector = new SurrogateSelector();
            selector.AddSurrogate(
                typeof(Order_Detail),
                new StreamingContext(StreamingContextStates.All),
                new OrderDetailSurrogate());

            var formatter = new NetDataContractSerializer();
            formatter.SurrogateSelector = selector;

            var tester = new XmlDataContractSerializerTester<IEnumerable<Order_Detail>>(formatter, true);

            var orderDetails = dbContext.Order_Details.ToList();

            var res = tester.SerializeAndDeserialize(orderDetails);
            Assert.IsTrue(res.Any());
            foreach (var orderDetail in res)
            {
                Assert.IsNotNull(orderDetail.Order);
                Assert.IsNotNull(orderDetail.Product);
            }
        }

        [TestMethod]
        public void IDataContractSurrogate()
        {
            dbContext.Configuration.ProxyCreationEnabled = true;
            dbContext.Configuration.LazyLoadingEnabled = true;

            //var tester =
            //    new XmlDataContractSerializerTester<IEnumerable<Order>>
            //    (
            //        new DataContractSerializer(typeof(IEnumerable<Order>)),
            //        true
            //    );
            //var orders = dbContext.Orders.ToList();

            //tester.SerializeAndDeserialize(orders);

            var formatterSettings = new DataContractSerializerSettings()
            {
                DataContractSurrogate = new OrderSurrogate()
            };

            var formatter = new DataContractSerializer(typeof(IEnumerable<Order>), formatterSettings);

            var tester = new XmlDataContractSerializerTester<IEnumerable<Order>>(formatter, true);

            var orders = dbContext.Orders.ToList();

            var res = tester.SerializeAndDeserialize(orders);
            Assert.IsTrue(res.Any());
            foreach (var o in res)
            {
                Assert.IsNotNull(o.Customer);
                Assert.IsNotNull(o.Employee);
                Assert.IsTrue(o.GetType() == typeof(Order));
                Assert.IsTrue(o.Customer.GetType() == typeof(Customer));
                Assert.IsTrue(o.Employee.GetType() == typeof(Employee));
            }
        }

        // --------------------------------------------------------------------------------------------------
        private class OrderDetailSurrogate : ISerializationSurrogate
        {
            public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
            {
                var orderDetail = (Order_Detail)obj;
                info.AddValue("OrderID", orderDetail.OrderID);
                info.AddValue("ProductID", orderDetail.ProductID);
                info.AddValue("Quantity", orderDetail.Quantity);
                info.AddValue("UnitPrice", orderDetail.UnitPrice);
            }

            public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
            {
                var orderDetail = (Order_Detail)obj;
                orderDetail.OrderID = info.GetInt32("OrderID");
                orderDetail.ProductID = info.GetInt32("ProductID");
                orderDetail.Quantity = info.GetInt16("Quantity");
                orderDetail.UnitPrice = info.GetDecimal("UnitPrice");

                using (var dbContext = new Northwind())
                {
                    dbContext.Configuration.ProxyCreationEnabled = false;
                    orderDetail.Order = dbContext.Orders.Find(orderDetail.OrderID);
                    orderDetail.Product = dbContext.Products.Find(orderDetail.ProductID);
                }

                return orderDetail;
            }
        }

        public class OrderSurrogate : IDataContractSurrogate
        {
            #region Not implemented
            public object GetCustomDataToExport(Type clrType, Type dataContractType)
            {
                throw new NotImplementedException();
            }

            public object GetCustomDataToExport(MemberInfo memberInfo, Type dataContractType)
            {
                throw new NotImplementedException();
            }
            public Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData)
            {
                throw new NotImplementedException();
            }

            public CodeTypeDeclaration ProcessImportedType(CodeTypeDeclaration typeDeclaration, CodeCompileUnit compileUnit)
            {
                throw new NotImplementedException();
            }
            public void GetKnownCustomDataTypes(Collection<Type> customDataTypes)
            {
                throw new NotImplementedException();
            }
            #endregion

            public Type GetDataContractType(Type type)
            {
                return type;
            }

            public object GetObjectToSerialize(object obj, Type targetType)
            {
                Order order = obj as Order;
                if (order != null)
                {
                    return new Order()
                    {
                        OrderID = order.OrderID,
                        CustomerID = order.CustomerID,
                        EmployeeID = order.EmployeeID,
                        OrderDate = order.OrderDate,
                        RequiredDate = order.RequiredDate,
                        ShippedDate = order.ShippedDate,
                        ShipVia = order.ShipVia,
                        Freight = order.Freight,
                        ShipName = order.ShipName,
                        ShipAddress = order.ShipAddress,
                        ShipCity = order.ShipCity,
                        ShipRegion = order.ShipRegion,
                        ShipPostalCode = order.ShipPostalCode,
                        ShipCountry = order.ShipCountry,
                        Employee = new Employee()
                        {
                            EmployeeID = order.Employee.EmployeeID,
                            LastName = order.Employee.LastName,
                            FirstName = order.Employee.FirstName,
                            Title = order.Employee.Title,
                            TitleOfCourtesy = order.Employee.TitleOfCourtesy,
                            BirthDate = order.Employee.BirthDate,
                            HireDate = order.Employee.HireDate,
                            Address = order.Employee.Address,
                            City = order.Employee.City,
                            Region = order.Employee.Region,
                            PostalCode = order.Employee.PostalCode,
                            Country = order.Employee.Country,
                            HomePhone = order.Employee.HomePhone,
                            Extension = order.Employee.Extension,
                            Photo = order.Employee.Photo,
                            Notes = order.Employee.Notes,
                            ReportsTo = order.Employee.ReportsTo,
                            PhotoPath = order.Employee.PhotoPath
                        },
                        Customer = new Customer()
                        {
                            CustomerID = order.Customer.CustomerID,
                            CompanyName = order.Customer.CompanyName,
                            ContactName = order.Customer.ContactName,
                            ContactTitle = order.Customer.ContactTitle,
                            Address = order.Customer.Address,
                            City = order.Customer.City,
                            Region = order.Customer.Region,
                            PostalCode = order.Customer.PostalCode,
                            Country = order.Customer.Country,
                            Phone = order.Customer.Phone,
                            Fax = order.Customer.Fax
                        },
                    };
                }

                return obj;
            }

            public object GetDeserializedObject(object obj, Type targetType)
            {
                return obj;
            }
        }


    }
}
