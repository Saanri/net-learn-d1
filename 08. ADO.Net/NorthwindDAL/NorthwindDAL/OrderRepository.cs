using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Configuration;


namespace NorthwindDAL
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbProviderFactory _providerFactory;
        private readonly string _connectionString;

        public OrderRepository()
        {
            string provider = ConfigurationManager.ConnectionStrings["Northwind"].ProviderName;
            string connectionString = ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString;

            _providerFactory = DbProviderFactories.GetFactory(provider);
            _connectionString = connectionString;
        }

        public IEnumerable<Order> GetOrders()
        {
            var result = new List<Order>();

            using (var connection = _providerFactory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        @"select OrderID, CustomerID, EmployeeID, OrderDate, RequiredDate
                               , ShippedDate, ShipVia, Freight, ShipName, ShipAddress
                               , ShipCity, ShipRegion, ShipPostalCode, ShipCountry
                               , case
                                   when ShippedDate is not null then 2
                                   when RequiredDate is not null then 1
                                   else 0
                                 end as OrderStatus
                            from dbo.Orders
                         "
                    ;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var order = new Order();
                            order.OrderID = reader.GetInt32(0);
                            order.CustomerID = reader.IsDBNull(1) ? null : reader.GetString(1);
                            order.EmployeeID = reader.IsDBNull(2) ? null : (int?)reader.GetInt32(2);
                            order.OrderDate = reader.IsDBNull(3) ? null : (DateTime?)reader.GetDateTime(3);
                            order.RequiredDate = reader.IsDBNull(4) ? null : (DateTime?)reader.GetDateTime(4);
                            order.ShippedDate = reader.IsDBNull(5) ? null : (DateTime?)reader.GetDateTime(5);
                            order.ShipVia = reader.IsDBNull(6) ? null : (int?)reader.GetInt32(6);
                            order.Freight = reader.IsDBNull(7) ? null : (decimal?)reader.GetDecimal(7);
                            order.ShipName = reader.IsDBNull(8) ? null : reader.GetString(8);
                            order.ShipAddress = reader.IsDBNull(9) ? null : reader.GetString(9);
                            order.ShipCity = reader.IsDBNull(10) ? null : reader.GetString(10);
                            order.ShipRegion = reader.IsDBNull(11) ? null : reader.GetString(11);
                            order.ShipPostalCode = reader.IsDBNull(12) ? null : reader.GetString(12);
                            order.ShipCountry = reader.IsDBNull(13) ? null : reader.GetString(13);
                            order.OrderStatus = (OrderStatuses)reader.GetInt32(14);
                            result.Add(order);
                        }
                    }
                }
            }

            return result;
        }

        public Order GetOrderByID(int orderID)
        {
            using (var connection = _providerFactory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        @"select OrderID, CustomerID, EmployeeID, OrderDate, RequiredDate
                               , ShippedDate, ShipVia, Freight, ShipName, ShipAddress
                               , ShipCity, ShipRegion, ShipPostalCode, ShipCountry
                               , case
                                   when ShippedDate is not null then 2
                                   when RequiredDate is not null then 1
                                   else 0
                                 end as OrderStatus
                            from dbo.Orders
                           where OrderID = @pOrderID; " +
                        @"select odt.OrderID, odt.ProductID, prd.ProductName, odt.UnitPrice
                               , odt.Quantity, odt.Discount
							   , (odt.UnitPrice * (1- odt.Discount) * odt.Quantity) as TotalSum
                            from dbo.[Order Details] odt
                              inner join dbo.Products prd
                                      on prd.ProductID = odt.ProductID
                           where odt.OrderID = @pOrderID"
                    ;

                    var pOrderID = command.CreateParameter();
                    pOrderID.ParameterName = "@pOrderID";
                    pOrderID.Value = orderID;
                    command.Parameters.Add(pOrderID);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var order = new Order();

                            order.OrderID = reader.GetInt32(0);
                            order.CustomerID = reader.IsDBNull(1) ? null : reader.GetString(1);
                            order.EmployeeID = reader.IsDBNull(2) ? null : (int?)reader.GetInt32(2);
                            order.OrderDate = reader.IsDBNull(3) ? null : (DateTime?)reader.GetDateTime(3);
                            order.RequiredDate = reader.IsDBNull(4) ? null : (DateTime?)reader.GetDateTime(4);
                            order.ShippedDate = reader.IsDBNull(5) ? null : (DateTime?)reader.GetDateTime(5);
                            order.ShipVia = reader.IsDBNull(6) ? null : (int?)reader.GetInt32(6);
                            order.Freight = reader.IsDBNull(7) ? null : (decimal?)reader.GetDecimal(7);
                            order.ShipName = reader.IsDBNull(8) ? null : reader.GetString(8);
                            order.ShipAddress = reader.IsDBNull(9) ? null : reader.GetString(9);
                            order.ShipCity = reader.IsDBNull(10) ? null : reader.GetString(10);
                            order.ShipRegion = reader.IsDBNull(11) ? null : reader.GetString(11);
                            order.ShipPostalCode = reader.IsDBNull(12) ? null : reader.GetString(12);
                            order.ShipCountry = reader.IsDBNull(13) ? null : reader.GetString(13);
                            order.OrderStatus = (OrderStatuses)reader.GetInt32(14);

                            var orderDetails = new List<OrderDetail>();

                            reader.NextResult();
                            while (reader.Read())
                            {
                                var orderDetail = new OrderDetail();
                                orderDetail.OrderID = reader.GetInt32(0);
                                orderDetail.ProductID = reader.GetInt32(1);
                                orderDetail.ProductName = reader.GetString(2);
                                orderDetail.UnitPrice = reader.GetDecimal(3);
                                orderDetail.Quantity = reader.GetInt16(4);
                                orderDetail.Discount = reader.GetFloat(5);
                                orderDetail.TotalSum = reader.GetFloat(6);

                                orderDetails.Add(orderDetail);
                            }
                            order.OrderDetails = orderDetails;

                            return order;
                        }
                        else throw new OrderNotFound(string.Format(@"Order {0} not found.", orderID));
                    }
                }
            }
        }

        public int CreateOrder(Order order)
        {
            int orderID = 0;
            using (var connection = _providerFactory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    orderID = CreateOrder(connection, transaction, order);
                    transaction.Commit();
                }
            }
            return orderID;
        }

        public bool UpdateOrder(Order order)
        {
            bool result;
            using (var connection = _providerFactory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    result = UpdateOrder(connection, transaction, order);
                    transaction.Commit();
                }
            }
            return result;
        }

        public bool DeleteOrderByID(int orderID)
        {
            using (var connection = _providerFactory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;

                        command.CommandText = @"select case"
                                            + @"         when ShippedDate is not null then 2 "
                                            + @"         when RequiredDate is not null then 1 "
                                            + @"         else 0 "
                                            + @"       end as OrderStatus "
                                            + @"  from dbo.Orders "
                                            + @" where OrderID = @pOrderID"
                        ;

                        command.Parameters.Clear();
                        var pOrderID = command.CreateParameter();
                        pOrderID.ParameterName = "@pOrderID";
                        pOrderID.Value = orderID;
                        command.Parameters.Add(pOrderID);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var orderStatus = (OrderStatuses)reader.GetInt32(0);
                                if (orderStatus.Equals(OrderStatuses.Shipped))
                                    throw new OrderСantBeDeleted(string.Format(@"Order {0} in '{1}' status can't be deleted.", orderID, orderStatus));
                            }
                            else return false;
                        }

                        command.CommandText = @" delete from dbo.[Order Details] where OrderID = @pOrderId; "
                                            + @" delete from dbo.Orders where OrderID = @pOrderId; "
                        ;

                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }
            return true;
        }

        public bool OrderSendToWorkByID(int orderID, DateTime requiredDate)
        {
            using (var connection = _providerFactory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;

                        command.CommandText = @"select case"
                                            + @"         when ShippedDate is not null then 2 "
                                            + @"         when RequiredDate is not null then 1 "
                                            + @"         else 0 "
                                            + @"       end as OrderStatus "
                                            + @"  from dbo.Orders "
                                            + @" where OrderID = @pOrderID"
                        ;

                        command.Parameters.Clear();
                        var pOrderID = command.CreateParameter();
                        pOrderID.ParameterName = "@pOrderID";
                        pOrderID.Value = orderID;
                        command.Parameters.Add(pOrderID);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var orderStatus = (OrderStatuses)reader.GetInt32(0);
                                if (!orderStatus.Equals(OrderStatuses.New)) return false;
                            }
                            else throw new OrderNotFound(string.Format(@"Order {0} not found.", orderID));
                        }

                        command.CommandText = @" update Orders "
                                            + @" set RequiredDate = @pRequiredDate "
                                            + @" where OrderID = @pOrderID "
                        ;

                        var pRequiredDate = command.CreateParameter();
                        pRequiredDate.ParameterName = "@pRequiredDate";
                        pRequiredDate.Value = requiredDate;
                        command.Parameters.Add(pRequiredDate);

                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }
            return true;
        }

        public bool OrderMarkAsDoneByID(int orderID, DateTime shippedDate)
        {
            using (var connection = _providerFactory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;

                        command.CommandText = @"select case"
                                            + @"         when ShippedDate is not null then 2 "
                                            + @"         when RequiredDate is not null then 1 "
                                            + @"         else 0 "
                                            + @"       end as OrderStatus "
                                            + @"  from dbo.Orders "
                                            + @" where OrderID = @pOrderID"
                        ;

                        command.Parameters.Clear();
                        var pOrderID = command.CreateParameter();
                        pOrderID.ParameterName = "@pOrderID";
                        pOrderID.Value = orderID;
                        command.Parameters.Add(pOrderID);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var orderStatus = (OrderStatuses)reader.GetInt32(0);
                                if (!orderStatus.Equals(OrderStatuses.Process)) return false;
                            }
                            else throw new OrderNotFound(string.Format(@"Order {0} not found.", orderID));
                        }

                        command.CommandText = @" update Orders "
                                            + @" set ShippedDate = @pShippedDate "
                                            + @" where OrderID = @pOrderID "
                        ;

                        var pShippedDate = command.CreateParameter();
                        pShippedDate.ParameterName = "@pShippedDate";
                        pShippedDate.Value = shippedDate;
                        command.Parameters.Add(pShippedDate);

                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }
            return true;
        }

        public IEnumerable<CustOrderHist> GetCustOrderHist(string customerID)
        {
            var result = new List<CustOrderHist>();

            using (var connection = _providerFactory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"exec dbo.CustOrderHist @pCustomerID";
                    command.Parameters.Clear();
                    var pCustomerID = command.CreateParameter();
                    pCustomerID.ParameterName = "@pCustomerID";
                    pCustomerID.Value = customerID;
                    command.Parameters.Add(pCustomerID);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var custOrderHist = new CustOrderHist();
                            custOrderHist.ProductName = reader.GetString(0);
                            custOrderHist.Total = reader.GetInt32(1);
                            result.Add(custOrderHist);
                        }
                    }
                }
            }

            return result;
        }

        public IEnumerable<CustOrdersDetail> GetCustOrdersDetail(int orderID)
        {
            var result = new List<CustOrdersDetail>();

            using (var connection = _providerFactory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"exec dbo.CustOrdersDetail @pOrderID";
                    command.Parameters.Clear();
                    var pOrderID = command.CreateParameter();
                    pOrderID.ParameterName = "@pOrderID";
                    pOrderID.Value = orderID;
                    command.Parameters.Add(pOrderID);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var custOrdersDetail = new CustOrdersDetail();
                            custOrdersDetail.ProductName = reader.GetString(0);
                            custOrdersDetail.UnitPrice = reader.GetDecimal(1);
                            custOrdersDetail.Quantity = reader.GetInt16(2);
                            custOrdersDetail.Discount = reader.GetInt32(3);
                            custOrdersDetail.ExtendedPrice = reader.GetDecimal(4);
                            result.Add(custOrdersDetail);
                        }
                    }
                }
            }

            return result;
        }

        private int CreateOrder(DbConnection connection, DbTransaction transaction, Order order)
        {
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @" insert into dbo.Orders (CustomerID) "
                                    + @" output inserted.OrderID "
                                    + @" values ( null ) ";

                order.OrderID = (int)command.ExecuteScalar();
                UpdateOrder(connection, transaction, order);
            }
            return order.OrderID;
        }

        private bool UpdateOrder(DbConnection connection, DbTransaction transaction, Order order)
        {
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;

                command.CommandText = @"select case"
                                    + @"         when ShippedDate is not null then 2 "
                                    + @"         when RequiredDate is not null then 1 "
                                    + @"         else 0 "
                                    + @"       end as OrderStatus "
                                    + @"  from dbo.Orders "
                                    + @" where OrderID = @pOrderID"
                ;

                command.Parameters.Clear();
                var pOrderID = command.CreateParameter();
                pOrderID.ParameterName = "@pOrderID";
                pOrderID.Value = order.OrderID;
                command.Parameters.Add(pOrderID);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var orderStatus = (OrderStatuses)reader.GetInt32(0);
                        if (!orderStatus.Equals(OrderStatuses.New))
                            throw new OrderСantBeModified(string.Format(@"Order {0} in '{1}' status can't be modified.", order.OrderID, orderStatus));
                    }
                    else throw new OrderNotFound(string.Format(@"Order {0} not found.", order.OrderID));
                }

                command.Parameters.Clear();
                var countPar = 0;

                command.CommandText = @"update dbo.Orders set ";

                if (!string.IsNullOrEmpty(order.CustomerID))
                {
                    countPar++;
                    command.CommandText += @" CustomerID = @pCustomerID ";
                    var pCustomerID = command.CreateParameter();
                    pCustomerID.ParameterName = "@pCustomerID";
                    pCustomerID.Value = order.CustomerID;
                    command.Parameters.Add(pCustomerID);
                }

                if (!order.EmployeeID.Equals(null))
                {
                    countPar++;
                    command.CommandText += ((countPar > 0) ? " , " : "") + @" EmployeeID = @pEmployeeID ";
                    var pEmployeeID = command.CreateParameter();
                    pEmployeeID.ParameterName = "@pEmployeeID";
                    pEmployeeID.Value = order.EmployeeID;
                    command.Parameters.Add(pEmployeeID);
                }

                if (!order.OrderDate.Equals(null))
                {
                    countPar++;
                    command.CommandText += ((countPar > 0) ? " , " : "") + @" OrderDate = @pOrderDate ";
                    var pOrderDate = command.CreateParameter();
                    pOrderDate.ParameterName = "@pOrderDate";
                    pOrderDate.Value = order.OrderDate;
                    command.Parameters.Add(pOrderDate);
                }

                if (!order.RequiredDate.Equals(null))
                {
                    countPar++;
                    command.CommandText += ((countPar > 0) ? " , " : "") + @" RequiredDate = @pRequiredDate ";
                    var pRequiredDate = command.CreateParameter();
                    pRequiredDate.ParameterName = "@pRequiredDate";
                    pRequiredDate.Value = order.RequiredDate;
                    command.Parameters.Add(pRequiredDate);
                }

                if (!order.ShippedDate.Equals(null))
                {
                    countPar++;
                    command.CommandText += ((countPar > 0) ? " , " : "") + @" ShippedDate = @pShippedDate ";
                    var pShippedDate = command.CreateParameter();
                    pShippedDate.ParameterName = "@pShippedDate";
                    pShippedDate.Value = order.ShippedDate;
                    command.Parameters.Add(pShippedDate);
                }

                if (!order.ShipVia.Equals(null))
                {
                    countPar++;
                    command.CommandText += ((countPar > 0) ? " , " : "") + @" ShipVia = @pShipVia ";
                    var pShipVia = command.CreateParameter();
                    pShipVia.ParameterName = "@pShipVia";
                    pShipVia.Value = order.ShipVia;
                    command.Parameters.Add(pShipVia);
                }

                if (!order.Freight.Equals(null))
                {
                    countPar++;
                    command.CommandText += ((countPar > 0) ? " , " : "") + @" Freight = @pFreight ";
                    var pFreight = command.CreateParameter();
                    pFreight.ParameterName = "@pFreight";
                    pFreight.Value = order.Freight;
                    command.Parameters.Add(pFreight);
                }

                if (!string.IsNullOrEmpty(order.ShipName))
                {
                    countPar++;
                    command.CommandText += ((countPar > 0) ? " , " : "") + @" ShipName = @pShipName ";
                    var pShipName = command.CreateParameter();
                    pShipName.ParameterName = "@pShipName";
                    pShipName.Value = order.ShipName;
                    command.Parameters.Add(pShipName);
                }

                if (!string.IsNullOrEmpty(order.ShipAddress))
                {
                    countPar++;
                    command.CommandText += ((countPar > 0) ? " , " : "") + @" ShipAddress = @pShipAddress ";
                    var pShipAddress = command.CreateParameter();
                    pShipAddress.ParameterName = "@pShipAddress";
                    pShipAddress.Value = order.ShipAddress;
                    command.Parameters.Add(pShipAddress);
                }

                if (!string.IsNullOrEmpty(order.ShipCity))
                {
                    countPar++;
                    command.CommandText += ((countPar > 0) ? " , " : "") + @" ShipCity = @pShipCity ";
                    var pShipCity = command.CreateParameter();
                    pShipCity.ParameterName = "@pShipCity";
                    pShipCity.Value = order.ShipCity;
                    command.Parameters.Add(pShipCity);
                }

                if (!string.IsNullOrEmpty(order.ShipRegion))
                {
                    countPar++;
                    command.CommandText += ((countPar > 0) ? " , " : "") + @" ShipRegion = @pShipRegion ";
                    var pShipRegion = command.CreateParameter();
                    pShipRegion.ParameterName = "@pShipRegion";
                    pShipRegion.Value = order.ShipRegion;
                    command.Parameters.Add(pShipRegion);
                }

                if (!string.IsNullOrEmpty(order.ShipPostalCode))
                {
                    countPar++;
                    command.CommandText += ((countPar > 0) ? " , " : "") + @" ShipPostalCode = @pShipPostalCode ";
                    var pShipPostalCode = command.CreateParameter();
                    pShipPostalCode.ParameterName = "@pShipPostalCode";
                    pShipPostalCode.Value = order.ShipPostalCode;
                    command.Parameters.Add(pShipPostalCode);
                }

                if (!string.IsNullOrEmpty(order.ShipCountry))
                {
                    countPar++;
                    command.CommandText += ((countPar > 0) ? " , " : "") + @" ShipCountry = @pShipCountry ";
                    var pShipCountry = command.CreateParameter();
                    pShipCountry.ParameterName = "@pShipCountry";
                    pShipCountry.Value = order.ShipCountry;
                    command.Parameters.Add(pShipCountry);
                }

                command.CommandText += @" where OrderID = @pOrderID ";
                command.Parameters.Add(pOrderID);

                if (countPar == 0) return false;
                command.ExecuteNonQuery();

                foreach (var orderDetail in order.OrderDetails)
                {
                    command.CommandText =
                        @"merge into dbo.[Order Details] trg
                           using ( select @pOrderID as OrderID
                                        , @pProductID as ProductID
                                        , @pUnitPrice as UnitPrice
                                        , @pQuantity as Quantity
                                        , @pDiscount as Discount
                                 ) src
                              on (    src.OrderID = trg.OrderID
	                              and src.ProductID = trg.ProductID
	                             )
                          when matched then
                            update set UnitPrice = src.UnitPrice
                                     , Quantity = src.Quantity
                                     , Discount = src.Discount
                          when not matched then
                            insert (OrderID, ProductID, UnitPrice, Quantity, Discount)
                            values (src.OrderID, src.ProductID, src.UnitPrice, src.Quantity, src.Discount)
                          ;
                         "
                    ;
                    command.Parameters.Clear();
                    command.Parameters.Add(pOrderID);

                    var pProductID = command.CreateParameter();
                    pProductID.ParameterName = "@pProductID";
                    pProductID.Value = orderDetail.ProductID;
                    command.Parameters.Add(pProductID);

                    var pUnitPrice = command.CreateParameter();
                    pUnitPrice.ParameterName = "@pUnitPrice";
                    pUnitPrice.Value = orderDetail.UnitPrice;
                    command.Parameters.Add(pUnitPrice);

                    var pQuantity = command.CreateParameter();
                    pQuantity.ParameterName = "@pQuantity";
                    pQuantity.Value = orderDetail.Quantity;
                    command.Parameters.Add(pQuantity);

                    var pDiscount = command.CreateParameter();
                    pDiscount.ParameterName = "@pDiscount";
                    pDiscount.Value = orderDetail.Discount;
                    command.Parameters.Add(pDiscount);

                    command.ExecuteNonQuery();
                }
            }
            return true;
        }
    }
}
