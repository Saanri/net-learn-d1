-- 2. Выдать всех продавцов, которые имеют более 150 заказов.
--    Использовать вложенный SELECT.

select emp.TitleOfCourtesy + ' ' + emp.LastName + ' ' + emp.FirstName as EmployeesName
  from Employees emp
 where emp.EmployeeID in (select o.EmployeeID
                            from Orders o
                           group by o.EmployeeID
                          having count(*) > 150
                         )
;