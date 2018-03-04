-- 1. Найти общую сумму всех заказов из таблицы Order Details с 
--    учетом количества закупленных товаров и скидок по ним. 
--    Результатом запроса должна быть одна запись с одной колонкой
--    с названием колонки 'Totals'.

select Sum(odt.UnitPrice * odt.Quantity * odt.Discount) as Totals
  from [Order Details] odt
;