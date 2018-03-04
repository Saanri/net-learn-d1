-- 3. По таблице Orders найти количество заказов, сделанных каждым
--    продавцом и для каждого покупателя. Необходимо определить это
--    только для заказов, сделанных в 1998 году. 

select (select LastName + ' ' + FirstName
          from Employees
		 where EmployeeID = ord.EmployeeID
       ) as Seller
	 , ord.CustomerID
     , count(*) as Amount
  from Orders ord
 where Year(ord.OrderDate) = 1998
 group by ord.EmployeeID
        , ord.CustomerID
;