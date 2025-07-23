IF (OBJECT_ID('dbo.usp0_WebLogs_add') IS NOT NULL)
begin
	print '		Already exists. dropping it ...'
	
	drop procedure dbo.usp0_WebLogs_add

	print '		Dropped'
end

GO
print 'Creating usp0_WebLogs_add sproc ...'
go
/****** Object:  StoredProcedure [dbo].[usp0_WebLogs_add]    Script Date: 5/13/2018 4:14:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp0_WebLogs_add]
(
	@AppId				[int]				NULL,
	@LogDate			[datetime]			NULL,
	@LogType			[tinyint]			NULL,
	@OperationResult	[tinyint]			NULL,
	@Category			[nvarchar](500)		NULL,
	@File				[nvarchar](1000)	NULL,
	@Line				[int]				NULL,
	@MemberName			[nvarchar](500)		NULL,
	@User				[nvarchar](100)		NULL,
	@Ip					[nvarchar](50)		NULL,
	@Message			[nvarchar](max)		NULL,
	@StackTrace			[nvarchar](max)		NULL,
	@Data				[nvarchar](max)		NULL,
	@BrowserName		[nvarchar](100)		NULL,
	@BrowserVersion		[nvarchar](20)		NULL,
	@Method				[nvarchar](20)		NULL,
	@Url				[nvarchar](1000)	NULL,
	@Referrer			[nvarchar](1000)	NULL,
	@Headers			[nvarchar](2000)	NULL,
	@Form				[nvarchar](max)		NULL,
	@Cookies			[nvarchar](2000)	NULL
)
AS
BEGIN
	SET NOCOUNT ON;

	declare @BrowserId int
	declare @BrowserVersionId int

	select @BrowserId = Id from dbo.Browsers where Name = @BrowserName
	if @BrowserId is null
	begin
		insert into dbo.Browsers(Name) values (@BrowserName)

		set @BrowserId = scope_identity()
	end

	select @BrowserVersionId = Id from dbo.BrowserVersions where BrowserId = @BrowserId and [Version] = @BrowserVersion
	if @BrowserVersionId is null
	begin
		insert into dbo.BrowserVersions([BrowserId], [Version]) values (@BrowserId, @BrowserVersion)

		set @BrowserVersionId = scope_identity()
	end

	INSERT INTO [dbo].[WebLogs]
    (
		 AppId			
		,LogDate		
		,LogType		
		,OperationResult
		,Category		
		,[File]
		,Line			
		,MemberName		
		,[User]			
		,[Ip]				
		,[Message]		
		,StackTrace		
		,[Data]			
		,BrowserVersionId	
		,Method			
		,[Url]			
		,Referrer		
		,Headers		
		,Form			
		,Cookies		
	)
    VALUES
    (
		 @AppId			
		,@LogDate		
		,@LogType		
		,@OperationResult
		,@Category		
		,@File			
		,@Line			
		,@MemberName		
		,@User			
		,@Ip				
		,@Message		
		,@StackTrace		
		,@Data			
		,@BrowserVersionId	
		,@Method			
		,@Url			
		,@Referrer		
		,@Headers		
		,@Form			
		,@Cookies		
	)
END
go
