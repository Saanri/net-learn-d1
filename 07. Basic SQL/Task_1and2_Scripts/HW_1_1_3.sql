-- 3. Выбрать в таблице Orders заказы, которые были доставлены
--    после 6 мая 1998 года (ShippedDate) не включая эту дату 
--    или которые еще не доставлены. В запросе должны возвращаться
--    только колонки OrderID (переименовать в Order Number) 
--    и ShippedDate (переименовать в Shipped Date). В результатах
--    запроса возвращать для колонки ShippedDate вместо значений NULL
--    строку ‘Not Shipped’, для остальных значений возвращать дату 
--    в формате по умолчанию.

select ord.OrderID as 'Order Number'
	 , case 
	     when ord.ShippedDate is null then 'Not Shipped'
		 else Convert(varchar, ord.ShippedDate)
       end as 'Shipped Date'
  from Orders ord
 where IsNull(ord.ShippedDate,Convert(datetime,'99991231',112)) > Convert(datetime,'19980506',112)
;