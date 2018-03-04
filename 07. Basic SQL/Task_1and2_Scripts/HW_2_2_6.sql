-- 6. По таблице Employees найти для каждого продавца его руководителя.

-- реализация 1
select emp.EmployeeID
     , emp.TitleOfCourtesy + ' ' + emp.LastName + ' ' + emp.FirstName as EmployeesName
	 , IsNull( (select e.TitleOfCourtesy + ' ' + e.LastName + ' ' + e.FirstName
                 from Employees e
                where e.EmployeeID = emp.ReportsTo)
             , 'Head'
             ) as EmployeesHeadName
  from Employees emp
;

-- реализация 2
with HierarchicalEmployees --(EmployeeID, ReportsTo, LastName, Level, pathStr)
as (select emp.EmployeeID
         , emp.ReportsTo
         , emp.TitleOfCourtesy + ' ' + emp.LastName + ' ' + emp.FirstName as EmployeesName
         , 0 As Level
		 , cast('' as varchar) As pathStr
      from Employees emp
     where emp.ReportsTo is null
    
	union all

    select emp.EmployeeID
         , emp.ReportsTo
         , emp.TitleOfCourtesy + ' ' + emp.LastName + ' ' + emp.FirstName
         , Level + 1
		 , cast((hem.pathStr + right(replicate('0',3) + cast(emp.EmployeeID as varchar),3)) as varchar)
      from Employees emp
        inner join HierarchicalEmployees hem
                on emp.ReportsTo = hem.EmployeeID
)
select EmployeeID
     , ReportsTo
     , space(level*2) + replicate('-',level) + space(1) + EmployeesName as EmployeesName
FROM HierarchicalEmployees
order by pathStr
;