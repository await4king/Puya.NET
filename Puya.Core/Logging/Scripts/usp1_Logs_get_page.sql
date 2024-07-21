GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
if exists(select 1 from sys.procedures where name = 'usp1_Logs_get_page')
	drop proc dbo.usp1_Logs_get_page
GO
CREATE PROCEDURE [dbo].[usp1_Logs_get_page]
(
	@Page				int out,
	@PageSize			int out,
	@RecordCount		int out,
	@PageCount			int out,
	@AppId				int,
	@LogType			tinyint,
	@OperationResult	tinyint,
	@Category			nvarchar(500),
	@MemberName			nvarchar(500),
	@User				nvarchar(100),
	@Ip					nvarchar(50),
	@Message			nvarchar(max),
	@Data				nvarchar(max),
	@FromDate			datetime,
	@ToDate				datetime,
	@OrderBy			nvarchar(100),
	@OrderDir			varchar(10)
)
as
begin
	declare @from_row	int
	declare @to_row		int
	declare @query		nvarchar(max)
	declare @where		nvarchar(1000)
	declare @ob			nvarchar(100)
	declare @od			varchar(10)

	set @where = N'
	where 1 = 1 ' +
		case when @AppId is null				then '' else ' and l.AppId = @AppId ' end +
		case when @LogType is null				then '' else ' and l.LogType = @LogType ' end +
		case when @OperationResult is null		then '' else ' and l.OperationResult = @OperationResult ' end +
		case when dbo.IsEmpty(@Category) = 1	then '' else ' and l.Category like @Category + N''%'' ' end +
		case when dbo.IsEmpty(@MemberName) = 1	then '' else ' and l.MemberName like @MemberName + N''%'' ' end +
		case when dbo.IsEmpty(@User) = 1		then '' else ' and l.User like @User + N''%'' ' end +
		case when dbo.IsEmpty(@IP) = 1			then '' else ' and l.Ip like @Ip + N''%'' ' end +
		case when dbo.IsEmpty(@Message) = 1		then '' else ' and l.Message like N''%''+ @Message + N''%'' ' end +
		case when dbo.IsEmpty(@Data) = 1		then '' else ' and l.Data like N''%''+ @Data + N''%'' ' end +
		case when @FromDate is null				then '' else ' and l.LogDate >= @FromDate ' end +
		case when @ToDate is null				then '' else ' and l.LogDate < @ToDate ' end

	set @query = N'
	SET @RecordCount = 
	(
		select count(Id) from [dbo].[Logs] l ' + @where +
	')'

	EXECUTE sp_executesql @query, N'@RecordCount		int out,
									@AppId				int,
									@LogType			tinyint,
									@OperationResult	tinyint,
									@Category			nvarchar(500),
									@MemberName			nvarchar(500),
									@User				nvarchar(100),
									@Ip					nvarchar(50),
									@Message			nvarchar(max),
									@Data				nvarchar(max),
									@FromDate			datetime,
									@ToDate				datetime',
									@RecordCount		= @RecordCount output	,
									@AppId				= @AppId				,
									@LogType			= @LogType				,
									@OperationResult	= @OperationResult		,
									@Category			= @Category				,
									@MemberName			= @MemberName			,
									@User				= @User					,
									@Ip					= @Ip					,
									@Message			= @Message				,
									@Data				= @Data					,
									@FromDate			= @FromDate				,
									@ToDate				= @ToDate

	exec [dbo].[usp0_Paging_calc] @Page	= @Page,
								  @PageSize		= @PageSize,
								  @RecordCount	= @RecordCount out,
								  @PageCount	= @PageCount out,
								  @fromrow		= @from_row out,
								  @torow		= @to_row out

	set @ob = TRIM(isnull(@OrderBy, ''))
	if @ob not in ('Id', 'LogDate', 'MemberName', 'Category', 'User', 'LogType', 'OperationResult')
		set @ob = 'LogDate'

	set @od = TRIM(isnull(@OrderDir, ''))
	if @od not in ('asc', 'desc', 'ascending', 'descending')
		set @od = 'desc'
	
	set @od =	case	when @od = 'ascending' then 'asc'
						when @od = 'descending' then 'desc'
						else @od
				end
	
	set @query = N'
	select * from
	(
		select
			ROW_NUMBER() over(order by ' + @ob + ' ' + @od + ') as [Row],
			*
		from [dbo].[Logs] l
		' + @where +
	') q where q.Row between @from_row and @to_row'

	EXECUTE sp_executesql @query, N'@AppId				int,
									@LogType			tinyint,
									@OperationResult	tinyint,
									@Category			nvarchar(500),
									@MemberName			nvarchar(500),
									@User				nvarchar(100),
									@Ip					nvarchar(50),
									@Message			nvarchar(max),
									@Data				nvarchar(max),
									@FromDate			datetime,
									@ToDate				datetime,
									@from_row			int,
									@to_row				int',
									@AppId				= @AppId				,
									@LogType			= @LogType				,
									@OperationResult	= @OperationResult		,
									@Category			= @Category				,
									@MemberName			= @MemberName			,
									@User				= @User					,
									@Ip					= @Ip					,
									@Message			= @Message				,
									@Data				= @Data					,
									@FromDate			= @FromDate				,
									@ToDate				= @ToDate				,
									@from_row			= @from_row				,
									@to_row				= @to_row
end
go
