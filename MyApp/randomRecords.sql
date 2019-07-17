Declare @Id int 
declare @FromDate date = '1950-01-01'
declare @ToDate date = '2018-12-31'
Set @Id = 1

While @Id <= 12000
Begin 
   Insert Into Users values ((select right(NEWID(),12)),
              (select right(NEWID(),12)),
			  (select right(NEWID(),12)),
			  CAST(RAND() * 2 AS INT),
			  (select dateadd(day, 
               rand(checksum(newid()))*(1+datediff(day, @FromDate, @ToDate)), 
               @FromDate)))
   Set @Id = @Id + 1
End