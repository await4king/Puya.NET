SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

if not exists (select 1 from sys.tables where name = 'Logs')
begin
	print 'Creating Logs table ...'

	CREATE TABLE [dbo].[Logs](
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
	 CONSTRAINT [PK_Logs] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	ALTER TABLE [dbo].[Logs] ADD  CONSTRAINT [DF_Logs_LogDate]  DEFAULT (getdate()) FOR [LogDate]

	print 'created'
end
else
	print 'table Logs already exists'
GO

