/****** Object:  StoredProcedure [dbo].[usp0_Log_insert]    Script Date: 5/13/2018 4:14:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
CREATE or ALTER PROCEDURE [dbo].[usp0_Log_insert]
(
	@MaxLog				[int]			,
	@MaxDailyLog		[int]           ,
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

	if @maxDailyLog > 0
    begin
        declare @fromdate date
        declare @todate date

        set @fromdate = isnull(@LogDate, getdate())
        set @todate = dateadd(day, 1, @fromdate)

        if (select count(*) from dbo.Logs where LogDate > @fromdate and LogDate < @todate) > @maxDailyLog - 1
            delete from dbo.Logs where LogDate > @fromdate and LogDate < @todate
    end

    if @maxLog > 0
        if (select count(*) from dbo.Logs) > @maxLog - 1
            truncate table dbo.Logs

    insert into dbo.Logs
    (
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
    )
END
go
