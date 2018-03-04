-- 1. Выдать всех поставщиков (колонка CompanyName в таблице Suppliers),
--    у которых нет хотя бы одного продукта на складе (UnitsInStock 
--    в таблице Products равно 0). Использовать вложенный SELECT для
--    этого запроса с использованием оператора IN. 

select sup.CompanyName
  from Suppliers sup
 where sup.SupplierID in (select p.SupplierID
                            from Products p
                           where IsNull(p.UnitsInStock,0) = 0
                         )
;