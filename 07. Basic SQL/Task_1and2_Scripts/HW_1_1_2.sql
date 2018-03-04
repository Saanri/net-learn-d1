-- 2. Написать запрос, который выводит только недоставленные
--    заказы из таблицы Orders. В результатах запроса возвращать
--    для колонки ShippedDate вместо значений NULL строку ‘Not Shipped’
--    (использовать системную функцию CASЕ). Запрос должен возвращать
--    только колонки OrderID и ShippedDate.

select ord.OrderID
     , case when ord.ShippedDate is null then 'Not Shipped' end as ShippedDate
  from Orders ord
 where ord.ShippedDate is null
;