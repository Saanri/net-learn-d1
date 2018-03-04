-- 2. Выбрать из таблицы Customers всех заказчиков, не проживающих
--    в USA и Canada. Запрос сделать с помощью оператора IN. 
--    Возвращать колонки с именем пользователя и названием страны
--    в результатах запроса. Упорядочить результаты запроса по 
--    имени заказчиков.

select cus.CustomerID
     , cus.Country
  from Customers cus
 where cus.Country not in ('USA','Canada')
 order by cus.CompanyName
;