SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE or ALTER PROCEDURE [dbo].[usp0_WebLogs_insert]
(
    @LogTable           [nvarchar](100) ,
	@MaxLog				[int]			,
	@MaxDailyLog		[int]			,
	@ThreadId           [int]           ,
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
	@ContentType		[nvarchar](100)	,
	@Url				[nvarchar](1000),
	@Referrer			[nvarchar](1000),
	@Headers			[nvarchar](2000),
	@Form				[nvarchar](max)	,
	@Cookies			[nvarchar](2000),
	@Body				[nvarchar](max)
)
AS
BEGIN
	SET NOCOUNT ON;

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

	declare @query nvarchar(max)

    if object_id(@LogTable) is null
        RAISERROR(N'invalid log table', 16, 1)

    set @query = N'
	if @maxDailyLog > 0
    begin
        declare @fromdate date
        declare @todate date

        set @fromdate = isnull(@LogDate, getdate())
        set @todate = dateadd(day, 1, @fromdate)

        if (select count(*) from ' + @LogTable + ' where LogDate > @fromdate and LogDate < @todate) > @maxDailyLog - 1
            delete from ' + @LogTable + ' where LogDate > @fromdate and LogDate < @todate
    end

    if @maxLog > 0
        if (select count(*) from ' + @LogTable + ') > @maxLog - 1
            truncate table ' + @LogTable + '

    INSERT INTO ' + @LogTable + '
    (
		 [ThreadId]
		,[AppId]
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
		,[ContentType]
		,[Url]
		,[Referrer]
		,[Headers]
		,[Form]
		,[Cookies]
		,[Body]
	)
    VALUES
    (
		 @ThreadId
		,@AppId
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
		,left(@ContentType, 100)
		,left(@Url, 1000)
		,left(@Referrer, 1000)
		,left(@Headers, 2000)
		,@Form
		,left(@Cookies, 2000)
		,@Body
	)'

    exec sp_executesql @query,
    N'
	@MaxLog				[int]			,
	@MaxDailyLog		[int]           ,
	@ThreadId           [int]           ,
    @AppId				[int]           ,
	@LogDate			[datetime]      ,
	@LogType			[tinyint]       ,
	@OperationResult	[tinyint]       ,
	@Category			[nvarchar](500) ,
	@File				[nvarchar](1000),
	@Line				[int]           ,
	@MemberName		    [nvarchar](500) ,
	@User				[nvarchar](100) ,
	@Ip				    [nvarchar](50)  ,
	@Message			[nvarchar](max) ,
	@StackTrace		    [nvarchar](max) ,
	@Data				[nvarchar](max)	,
	@BrowserVersionId	[int]			,
	@Method				[nvarchar](20)	,
	@ContentType		[nvarchar](100)	,
	@Url				[nvarchar](1000),
	@Referrer			[nvarchar](1000),
	@Headers			[nvarchar](2000),
	@Form				[nvarchar](max)	,
	@Cookies			[nvarchar](2000),
	@Body				[nvarchar](max)	',
	@MaxLog				= @MaxLog           ,
    @MaxDailyLog		= @MaxDailyLog      ,
	@ThreadId			= @ThreadId         ,
    @AppId				= @AppId			,
    @LogDate			= @LogDate			,
    @LogType			= @LogType			,
    @OperationResult	= @OperationResult	,
    @Category			= @Category			,
    @File				= @File				,
    @Line				= @Line				,
    @MemberName			= @MemberName		,
    @User				= @User				,
    @Ip					= @Ip				,
    @Message			= @Message			,
    @StackTrace			= @StackTrace		,
    @Data				= @Data				,
	@BrowserVersionId	= @BrowserVersionId	,
	@Method				= @Method			,
	@ContentType		= @ContentType		,
	@Url				= @Url				,
	@Referrer			= @Referrer			,
	@Headers			= @Headers			,
	@Form				= @Form				,
	@Cookies			= @Cookies			,
	@Body				= @Body

END
go

----------------------------------------------------------------------------------------------------
