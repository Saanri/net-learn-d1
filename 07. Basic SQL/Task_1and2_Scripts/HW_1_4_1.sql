-- 1. В таблице Products найти все продукты (колонка ProductName),
--    где встречается подстрока 'chocolade'. Известно, что в 
--    подстроке 'chocolade' может быть изменена одна буква 'c' 
--    в середине - найти все продукты, которые удовлетворяют этому
--    условию.

select prd.ProductName
  from Products prd
 where lower(prd.ProductName) like '%cho_olade%'
;