-- 2. Выдать в результатах запроса имена всех заказчиков из таблицы
--    Customers и суммарное количество их заказов из таблицы Orders.
--    Принять во внимание, что у некоторых заказчиков нет заказов,
--    но они также должны быть выведены в результатах запроса.
--    Упорядочить результаты запроса по возрастанию количества заказов.

select cus.CustomerID
     , cus.CompanyName
     , count(ord.CustomerID) as OrdersCount
  from Customers cus
    left join Orders ord
	       on ord.CustomerID = cus.CustomerID
 group by cus.CustomerID
        , cus.CompanyName
 order by 3
;

