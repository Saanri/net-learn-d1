-- 2. По таблице Orders найти количество заказов, которые еще не
--    были доставлены (т.е. в колонке ShippedDate нет значения даты
--    доставки). Использовать при этом запросе только оператор COUNT.
--    Не использовать предложения WHERE и GROUP.

select Sum(case when ord.ShippedDate is null then 1 else 0 end) as countNotShipped
  from Orders ord
;

-- проверка )
select count(*) from Orders ord where ord.ShippedDate is null;
select count(*) from Orders ord where ord.ShippedDate is not null;
select count(*) from Orders ord;