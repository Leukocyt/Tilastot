-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[F_Menot_Per_Viikko]
(
	-- Add the parameters for the function here
	@start datetime, @end datetime, @typeList nvarchar(200), @placeList nvarchar(200), @groupType int = 0, @resMax int = 5
)
RETURNS 
@retTable TABLE 
(
	-- Add the column definitions for the TABLE variable here
	rowid int identity(1,1),
	weekNumber nvarchar(300),
	summa float
)
AS
BEGIN
	-- Fill the table variable with the rows for your result set
	if @groupType = 0 begin
		insert @retTable (summa, weekNumber)
		select 
			round(sum(maara),2) as summa, datepart(wk, timestamp) as weekNumber from kulut k
		where timestamp 
			between @start and @end --and k.paikkaID in (select string from dbo.ufn_CSVToTable(@placeList, ','))				
			and k.tyyppi in (select string from dbo.ufn_CSVToTable(@typeList, ','))
			and (@placeList = '' or k.paikkaID in (select string from dbo.ufn_CSVToTable(@placeList, ',')))
		group by 
			datepart(wk, timestamp) 
		order by 
			weekNumber asc
		end
	--Tyypit
	else if @groupType = 1 begin
		insert @retTable (summa, weekNumber)
		select  t1.summa, t.kuvaus from (

		select 
			round(sum(maara),2) as summa, tyyppi from kulut k
		where timestamp 
			between @start and @end --and k.paikkaID in (select string from dbo.ufn_CSVToTable(@placeList, ','))
			and k.tyyppi in (select string from dbo.ufn_CSVToTable(@typeList, ','))

			and (@placeList = '' or k.paikkaID in (select string from dbo.ufn_CSVToTable(@placeList, ',')))
		group by 
			tyyppi
		) t1 left join kulutyypit t on t.id = t1.tyyppi
		
		order by t1.summa desc

		if (select count(*) from @retTable) > @resMax begin
			declare @tmpSum float
			set @tmpSum = ( select sum(summa) from @retTable  where rowid not in (select top (@resMax) rowid from @retTable order by summa desc ))
			delete from @retTable where rowid not in (select top (@resMax) rowid from @retTable order by summa desc )

			insert @retTable (weekNumber, summa) select 'Muut', @tmpSum

			set @groupType = 0
		end
		
	end
	--Paikat
	else if @groupType = 2 begin
		
		insert @retTable (summa, weekNumber)
		select  t1.summa, p.selite from (

		select 
			round(sum(maara),2) as summa, paikkaID from kulut k
		where timestamp 
			between @start and @end --and k.paikkaID in (select string from dbo.ufn_CSVToTable(@placeList, ','))
			and k.tyyppi in (select string from dbo.ufn_CSVToTable(@typeList, ','))

			and (@placeList = '' or k.paikkaID in (select string from dbo.ufn_CSVToTable(@placeList, ',')))
		group by 
			paikkaID
		) t1 left join paikat p on p.rowid =  t1.paikkaID
		
		order by t1.summa desc
	end
	
	RETURN 
END