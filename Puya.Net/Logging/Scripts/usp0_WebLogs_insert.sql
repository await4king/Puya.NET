SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE or ALTER PROCEDURE [dbo].[usp0_WebLogs_insert]
(
	@MaxLog				[int]			,
	@MaxDailyLog		[int]			,
	@AppId				[int]			,
	@LogDate			[datetime]		,
	@LogType			[tinyint]		,
	@OperationResult	[tinyint]		,
	@Category			[nvarchar](500)	,
	@File				[nvarchar](1000),
	@Line				[int]			,
	@MemberName			[nvarchar](500)	,
	@User				[nvarchar](100)	,
	@Ip					[nvarchar](50)	,
	@Message			[nvarchar](max)	,
	@StackTrace			[nvarchar](max)	,
	@Data				[nvarchar](max)	,
	@BrowserName		[nvarchar](100)	,
	@BrowserVersion		[nvarchar](20)	,
	@Method				[nvarchar](20)	,
	@Url				[nvarchar](1000),
	@Referrer			[nvarchar](1000),
	@Headers			[nvarchar](2000),
	@Form				[nvarchar](max)	,
	@Cookies			[nvarchar](2000)
)
AS
BEGIN
	SET NOCOUNT ON;

	if @maxDailyLog > 0
    begin
        declare @fromdate date
        declare @todate date

        set @fromdate = isnull(@LogDate, getdate())
		set @todate = dateadd(day, 1, @fromdate)

        if (select count(*) from dbo.WebLogs where LogDate > @fromdate and LogDate < @todate) > @maxDailyLog - 1
            delete from dbo.WebLogs where LogDate > @fromdate and LogDate < @todate
    end

    if @maxLog > 0
        if (select count(*) from dbo.WebLogs) > @maxLog - 1
            truncate table dbo.WebLogs

    declare @BrowserId int
	declare @BrowserVersionId int

	if dbo.IsEmpty(@BrowserName) = 1
		set @BrowserName = 'Other'
	
	if dbo.IsEmpty(@BrowserVersion) = 1
		set @BrowserVersion = '0.0'
		
	select @BrowserId = Id from dbo.Browsers where Name = @BrowserName
	
	if @BrowserId is null
	begin
		insert into dbo.Browsers(Name) values (left(@BrowserName, 200))

		set @BrowserId = scope_identity()
	end

	select @BrowserVersionId = Id from dbo.BrowserVersions
	where BrowserId = @BrowserId and [Version] = @BrowserVersion
	
	if @BrowserVersionId is null
	begin
		insert into dbo.BrowserVersions([BrowserId], [Version]) values (@BrowserId, left(@BrowserVersion, 200))

		set @BrowserVersionId = scope_identity()
	end

	INSERT INTO [dbo].[WebLogs]
    (
		 [AppId]
		,[LogDate]
		,[LogType]
		,[OperationResult]
		,[Category]
		,[File]
		,[Line]
		,[MemberName]
		,[User]
		,[Ip]
		,[Message]
		,[StackTrace]
		,[Data]
		,[BrowserVersionId]
		,[Method]
		,[Url]
		,[Referrer]
		,[Headers]
		,[Form]
		,[Cookies]
	)
    VALUES
    (
		 @AppId
		,@LogDate
		,@LogType
		,@OperationResult
		,left(@Category, 500)
		,left(@File, 1000)
		,@Line
		,left(@MemberName, 500)
		,left(@User, 100)
		,left(@Ip, 50)
		,@Message
		,@StackTrace
		,@Data
		,@BrowserVersionId
		,left(@Method, 20)
		,left(@Url, 1000)
		,left(@Referrer, 1000)
		,left(@Headers, 2000)
		,@Form
		,left(@Cookies, 2000)
	)
END
go

----------------------------------------------------------------------------------------------------
