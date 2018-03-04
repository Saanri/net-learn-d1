-- 1. По таблице Orders найти количество заказов с группировкой по
--    годам. В результатах запроса надо возвращать две колонки c
--    названиями Year и Total. Написать проверочный запрос, который
--    вычисляет количество всех заказов.

select Year(ord.OrderDate) as orderYear
     , count(*)
  from Orders ord
 group by Year(ord.OrderDate)
 order by orderYear
;