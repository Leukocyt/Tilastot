-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create FUNCTION [dbo].[F_Kulut_Per_Tyyppi]
(
	-- Add the parameters for the function here
	@start datetime, @end datetime, @typeList nvarchar(200), @placeList nvarchar(200) = ''
)
RETURNS 
@retTable TABLE 
(
	-- Add the column definitions for the TABLE variable here
	rowid int identity(1,1),
	weekNumber int,
	summa float
)
AS
BEGIN
	-- Fill the table variable with the rows for your result set

	insert @retTable (summa, weekNumber)

	select 
		sum(maara) as summa, tyyppi as type from kulut k
	where timestamp 
		between @start and @end --and k.paikkaID in (select string from dbo.ufn_CSVToTable(@placeList, ','))

		and k.tyyppi in (select string from dbo.ufn_CSVToTable(@typeList, ','))

		and (@placeList = '' or k.paikkaID in (select string from dbo.ufn_CSVToTable(@placeList, ',')))
	group by 
		tyyppi
	order by 
		summa desc
	
	RETURN 
END