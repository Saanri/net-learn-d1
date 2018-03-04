-- 4. Найти покупателей и продавцов, которые живут в одном городе.
--    Если в городе живут только один или несколько продавцов, или
--    только один или несколько покупателей, то информация о таких 
--    покупателя и продавцах не должна попадать в результирующий
--    набор. Не использовать конструкцию JOIN. 

select emp.City
	 , Stuff( (select  ', ' + e.LastName + ' ' + e.FirstName
                 from Employees e
                where e.City = emp.City
               for xml path('')
              )
            , 1
            , 2
            , ''
            ) as Employees
	 , Stuff( (select ', ' + c.CompanyName
                 from Customers c
                where c.City = emp.City
               for xml path('')
              )
            , 1
            , 2
            , ''
            ) as Customers
  from Employees emp
 where emp.City in (select cus.City from Customers cus)
 group by emp.City
;