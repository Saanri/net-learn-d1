-- 5. Найти всех покупателей, которые живут в одном городе. 

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
  from Employees emp
 group by emp.City
 having count(*) > 1
;