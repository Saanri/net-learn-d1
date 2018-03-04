-- 1. Выбрать в таблице Orders заказы, которые были доставлены 
--    после 6 мая 1998 года (колонка ShippedDate) включительно 
--    и которые доставлены с ShipVia >= 2. 
--    Запрос должен возвращать только колонки OrderID, ShippedDate и ShipVia.

select ord.OrderID
     , ord.ShippedDate
	 , ord.ShipVia
  from Orders ord
 where ord.ShippedDate >= Convert(datetime,'19980506',112)
   and ord.ShipVia >= 2
;