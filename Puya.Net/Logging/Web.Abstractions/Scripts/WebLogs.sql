SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

if not exists (select 1 from sys.tables where name = 'Browsers')
begin
	print 'Creating Browsers table ...'

	CREATE TABLE [dbo].[Browsers](
		[Id]		[int] IDENTITY(1,1) NOT NULL,
		[Name]		[nvarchar](200)		NOT NULL,
	 CONSTRAINT [PK_Browsers] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	print 'created'
end
else
	print 'table Browsers already exists'
GO

if not exists (select 1 from sys.tables where name = 'BrowserVersions')
begin
	print 'Creating BrowserVersions table ...'

	CREATE TABLE [dbo].[BrowserVersions](
		[Id]			[int] IDENTITY(1,1) NOT NULL,
		[BrowserId]		[int]				NOT NULL,
		[Version]		[nvarchar](200)		NOT NULL,
	 CONSTRAINT [PK_BrowserVersions] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	print 'created'
end
else
	print 'table BrowserVersions already exists'
GO

if not exists (select 1 from sys.tables where name = 'WebLogs')
begin
	print 'Creating WebLogs table ...'

	CREATE TABLE [dbo].[WebLogs](
		[Id]				[int] IDENTITY(1,1) NOT NULL,
		[AppId]				[int]				NULL,
		[LogDate]			[datetime]			NOT NULL,
		[LogType]			[tinyint]			NULL,
		[OperationResult]	[tinyint]			NULL,
		[Category]			[nvarchar](500)		NULL,
		[File]				[nvarchar](1000)	NULL,
		[Line]				[int]				NULL,
		[MemberName]		[nvarchar](500)		NULL,
		[User]				[nvarchar](100)		NULL,
		[Ip]				[nvarchar](50)		NULL,
		[Message]			[nvarchar](max)		NULL,
		[StackTrace]		[nvarchar](max)		NULL,
		[Data]				[nvarchar](max)		NULL,
		[BrowserVersionId]	[int]				NULL,
		[Method]			[nvarchar](20)		NULL,
		[Url]				[nvarchar](1000)	NULL,
		[Referrer]			[nvarchar](1000)	NULL,
		[Headers]			[nvarchar](2000)	NULL,
		[Form]				[nvarchar](max)		NULL,
		[Cookies]			[nvarchar](2000)	NULL,
	 CONSTRAINT [PK_WebLogs] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	ALTER TABLE [dbo].[WebLogs] ADD  CONSTRAINT [DF_WebLogs_LogDate]  DEFAULT (getdate()) FOR [LogDate]

	print 'created'
end
else
	print 'table WebLogs already exists'
GO

