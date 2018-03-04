-- 1. Выбрать все заказы (OrderID) из таблицы Order Details (заказы
--    не должны повторяться), где встречаются продукты с количеством
--    от 3 до 10 включительно – это колонка Quantity в таблице Order
--    Details. Использовать оператор BETWEEN. Запрос должен 
--    возвращать только колонку OrderID.

select distinct odt.OrderID
  from [Order Details] odt
 where odt.Quantity between 3 and 10
 order by odt.OrderID
;