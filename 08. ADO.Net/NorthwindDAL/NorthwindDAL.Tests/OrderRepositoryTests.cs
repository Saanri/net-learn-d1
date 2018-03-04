using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindDAL;
using System.Collections.Generic;

namespace NorthwindDAL.Tests
{
    [TestClass]
    public class OrderRepositoryTests
    {
        private OrderRepository orderRepository;

        [TestInitialize]
        public void Initialize()
        {
            orderRepository = new OrderRepository();
        }

        [TestMethod]
        public void TestGetOrders()
        {
            List<Order> orders = (List<Order>)orderRepository.GetOrders();
            Assert.IsTrue(orders.Count > 0);
        }

        [TestMethod]
        public void TestGetOrderByID()
        {
            List<Order> orders = (List<Order>)orderRepository.GetOrders();

            int i = 0;
            foreach (var o in orders)
            {
                Order order = orderRepository.GetOrderByID(o.OrderID);
                Assert.AreEqual(order.OrderID, o.OrderID);

                i++;
                if (i > 5) break;
            }
        }

        [TestMethod]
        public void TestCreateUpdateOrder()
        {
            Order order = new Order();
            order.CustomerID = "HANAR";
            order.EmployeeID = 2;
            order.OrderDate = new DateTime(2017, 01, 01);
            order.ShipVia = 1;
            order.Freight = 99.9m;
            order.ShipName = "ShipName";
            order.ShipAddress = "ShipAddress";
            order.ShipCity = "ShipCity";
            order.ShipRegion = "ShipRegion";

            OrderDetail od1 = new OrderDetail(); od1.ProductID = 11; od1.UnitPrice = 14m; od1.Quantity = 12; od1.Discount = 0;
            OrderDetail od2 = new OrderDetail(); od2.ProductID = 42; od2.UnitPrice = 9.8m; od2.Quantity = 10; od2.Discount = 0;
            OrderDetail od3 = new OrderDetail(); od3.ProductID = 72; od3.UnitPrice = 34.8m; od3.Quantity = 5; od3.Discount = 0;
            OrderDetail od4 = new OrderDetail(); od4.ProductID = 14; od4.UnitPrice = 18.6m; od4.Quantity = 9; od4.Discount = 0.15f;

            List<OrderDetail> odLst = new List<OrderDetail>();
            odLst.Add(od1);
            odLst.Add(od2);
            odLst.Add(od3);
            odLst.Add(od4);

            order.OrderDetails = odLst;

            int orderID = orderRepository.CreateOrder(order);
            Assert.IsTrue(orderID > 0);

            order = orderRepository.GetOrderByID(orderID);
            Assert.AreEqual(order.CustomerID, "HANAR");
            Assert.AreEqual(order.EmployeeID, 2);
            Assert.AreEqual(order.OrderDate, new DateTime(2017, 01, 01));

            order.CustomerID = "BERGS";
            order.EmployeeID = 3;
            order.OrderDate = new DateTime(2017, 01, 02);

            Assert.IsTrue(orderRepository.UpdateOrder(order));

            order = orderRepository.GetOrderByID(orderID);
            Assert.AreEqual(order.CustomerID, "BERGS");
            Assert.AreEqual(order.EmployeeID, 3);
            Assert.AreEqual(order.OrderDate, new DateTime(2017, 01, 02));

            Assert.IsTrue(orderRepository.DeleteOrderByID(orderID));
        }

        [TestMethod]
        [ExpectedException(typeof(OrderNotFound))]
        public void TestDeleteOrderByID()
        {
            Order order = new Order();

            int orderID = orderRepository.CreateOrder(order);
            Assert.IsTrue(orderID > 0);
            Assert.IsTrue(orderRepository.DeleteOrderByID(orderID));
            order = orderRepository.GetOrderByID(orderID);
        }

        [TestMethod]
        [ExpectedException(typeof(OrderNotFound))]
        public void TestOrderSendToWorkByID()
        {
            Order order = new Order();
            int orderID = orderRepository.CreateOrder(order);

            Assert.IsTrue(orderID > 0);
            Assert.IsTrue(orderRepository.OrderSendToWorkByID(orderID, new DateTime(2017, 02, 12)));

            order = orderRepository.GetOrderByID(orderID);
            Assert.AreEqual(order.RequiredDate, new DateTime(2017, 02, 12));

            Assert.IsFalse(orderRepository.OrderSendToWorkByID(orderID, new DateTime(2017, 02, 12)));

            Assert.IsTrue(orderRepository.DeleteOrderByID(orderID));

            orderRepository.OrderSendToWorkByID(orderID, new DateTime(2017, 02, 13));
        }

        [TestMethod]
        [ExpectedException(typeof(OrderСantBeDeleted))]
        public void TestOrderMarkAsDoneByID()
        {
            Order order = new Order();
            int orderID = orderRepository.CreateOrder(order);

            Assert.IsTrue(orderID > 0);
            Assert.IsFalse(orderRepository.OrderMarkAsDoneByID(orderID, new DateTime(2017, 02, 12)));
            Assert.IsTrue(orderRepository.OrderSendToWorkByID(orderID, new DateTime(2017, 02, 12)));
            Assert.IsTrue(orderRepository.OrderMarkAsDoneByID(orderID, new DateTime(2017, 02, 12)));

            order = orderRepository.GetOrderByID(orderID);
            Assert.AreEqual(order.ShippedDate, new DateTime(2017, 02, 12));

            Assert.IsFalse(orderRepository.OrderMarkAsDoneByID(orderID, new DateTime(2017, 02, 12)));

            orderRepository.DeleteOrderByID(orderID);
        }

        [TestMethod]
        public void TestGetCustOrderHist()
        {
            List<CustOrderHist> custOrderHists = (List<CustOrderHist>)orderRepository.GetCustOrderHist("HANAR");
            Assert.IsTrue(custOrderHists.Count > 0);
        }

        [TestMethod]
        public void TestGetCustOrdersDetail()
        {
            List<CustOrdersDetail> custOrdersDetails = (List<CustOrdersDetail>)orderRepository.GetCustOrdersDetail(16110);
            Assert.IsTrue(custOrdersDetails.Count > 0);
        }
    }
}
