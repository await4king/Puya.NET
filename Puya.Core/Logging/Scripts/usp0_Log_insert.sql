/****** Object:  StoredProcedure [dbo].[usp0_Log_insert]    Script Date: 5/13/2018 4:14:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
CREATE or ALTER PROCEDURE [dbo].[usp0_Log_insert]
(
    @LogTable           [nvarchar](100) ,
	@MaxLog				[int]			,
	@MaxDailyLog		[int]           ,
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
	@Data				[nvarchar](max)
)
AS
BEGIN
	SET NOCOUNT ON;

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

    insert into ' + @LogTable + '
    (
        [ThreadId],
        [AppId],
        [LogDate],
        [LogType],
        [OperationResult],
        [Category],
        [File],
        [Line],
        [MemberName],
        [User],
        [Ip],
        [Message],
        [StackTrace],
        [Data]
    )
    values
    (
        @ThreadId,
        @AppId,
        @LogDate,
        @LogType,
        @OperationResult,
        @Category,
        @File,
        @Line,
        @MemberName,
        @User,
        @Ip,
        @Message,
        @StackTrace,
        @Data
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
	@Data				[nvarchar](max)',
    @MaxLog          = @MaxLog          ,
    @MaxDailyLog     = @MaxDailyLog     ,
    @ThreadId        = @ThreadId        ,
    @AppId			 = @AppId           ,
    @LogDate         = @LogDate         ,
    @LogType         = @LogType         ,
    @OperationResult = @OperationResult ,
    @Category        = @Category        ,
    @File            = @File            ,
    @Line            = @Line            ,
    @MemberName      = @MemberName      ,
    @User            = @User            ,
    @Ip              = @Ip              ,
    @Message         = @Message         ,
    @StackTrace      = @StackTrace      ,
    @Data            = @Data
END
go
