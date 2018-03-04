using System;
using System.Collections.Generic;

namespace NorthwindDAL
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetOrders();

        Order GetOrderByID(int orderID);

        int CreateOrder(Order order);

        bool UpdateOrder(Order order);

        bool DeleteOrderByID(int orderID);

        bool OrderSendToWorkByID(int orderID, DateTime requiredDate);

        bool OrderMarkAsDoneByID(int orderID, DateTime shippedDate);

        IEnumerable<CustOrderHist> GetCustOrderHist(string customerID);

        IEnumerable<CustOrdersDetail> GetCustOrdersDetail(int orderID);
    }
}
