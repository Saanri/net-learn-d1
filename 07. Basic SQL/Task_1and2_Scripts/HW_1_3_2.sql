-- 2. Выбрать всех заказчиков из таблицы Customers, у которых 
--    название страны начинается на буквы из диапазона b и g.
--    Использовать оператор BETWEEN. Проверить, что в результаты 
--    запроса попадает Germany. Запрос должен возвращать только 
--    колонки CustomerID и Country и отсортирован по Country.

select cus.CustomerID
     , cus.Country
  from Customers cus
 where lower(SubString(cus.Country, 1, 1)) between 'b' and 'g'
 order by cus.Country
;