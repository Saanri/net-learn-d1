-- 3. Выбрать всех заказчиков из таблицы Customers, у которых
--    название страны начинается на буквы из диапазона b и g,
--    не используя оператор BETWEEN.

select cus.CustomerID
     , cus.Country
  from Customers cus
 where lower(SubString(cus.Country, 1, 1)) >= 'b' 
   and lower(SubString(cus.Country, 1, 1)) <= 'g'
 order by cus.Country
;