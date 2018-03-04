-- 3. По таблице Orders найти количество различных покупателей
--    (CustomerID), сделавших заказы. Использовать функцию COUNT
--    и не использовать предложения WHERE и GROUP.

select Count(distinct CustomerID) as countCustomers
  from Orders ord
;