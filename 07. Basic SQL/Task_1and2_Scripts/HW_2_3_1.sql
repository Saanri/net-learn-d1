-- 1.	Определить продавцов, которые обслуживают регион 'Western' (таблица Region). 

select distinct
       emp.EmployeeID
     , emp.TitleOfCourtesy + ' ' + emp.LastName + ' ' + emp.FirstName as EmployeesName
  from Employees emp
    inner join EmployeeTerritories etr
	        on etr.EmployeeID = emp.EmployeeID
    inner join Territories ter
            on ter.TerritoryID = etr.TerritoryID
    inner join Region reg
	        on reg.RegionID = ter.RegionID
           and reg.RegionDescription = 'Western'
;