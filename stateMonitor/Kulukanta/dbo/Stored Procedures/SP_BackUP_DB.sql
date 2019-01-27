-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE SP_BackUP_DB

	@db nvarchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @storeLocation nvarchar(500)
	declare @dateS nvarchar(100)
	set @storeLocation = 'C:\data\DB Backups\' + @db --TÄmänkin voisi antaa parametrina
	set @dateS = convert(varchar(25), getdate(), 120)
	set @dateS =  REPLACE(@dateS ,' ','_');  
	set @dateS =  REPLACE(@dateS ,':','_');  
	set @storeLocation = @storeLocation + '_' + @dateS + '.bak'
	--select @dateS
	BACKUP DATABASE @db
	TO  DISK = @storeLocation 
END