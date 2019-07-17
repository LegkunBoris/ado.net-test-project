select top 100 * from Users
where Name like 'F%' and Sex = 0
order by Name, Surname;