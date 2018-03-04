-- 3. Выдать всех заказчиков (таблица Customers), которые не имеют
--    ни одного заказа (подзапрос по таблице Orders). Использовать
--    оператор EXISTS.

select *
  from Customers cus
 where not exists (select 1
                     from Orders o
                    where o.CustomerID = cus.CustomerID
                  )
;