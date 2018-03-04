-- 1. Выбрать из таблицы Customers всех заказчиков, проживающих
--    в USA и Canada. Запрос сделать только с помощью оператора IN.
--    Возвращать колонки с именем пользователя и названием страны
--    в результатах запроса. Упорядочить результаты запроса по имени
--    заказчиков и по месту проживания.

select cus.CustomerID
     , cus.Country
  from Customers cus
 where cus.Country in ('USA','Canada')
 order by cus.CompanyName
        , cus.Address
;