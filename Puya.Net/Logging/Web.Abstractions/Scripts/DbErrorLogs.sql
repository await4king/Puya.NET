SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

if not exists (select 1 from sys.tables where name = 'DbErrorLogs')
begin
	print 'Creating DbErrorLogs table ...'

	CREATE TABLE [dbo].[DbErrorLogs](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Command] [nvarchar](200) NULL,
		[Number] [int] NULL,
		[Description] [nvarchar](2048) NULL,
		[Line] [int] NULL,
		[State] [int] NULL,
		[Severity] [int] NULL,
		[Args] [nvarchar](2000) NULL,
		[ErrorLogDate] [datetime] NULL,
		[Procedure] [nvarchar](200) NULL,
	 CONSTRAINT [PK_DbErrorLogs] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	
	ALTER TABLE [dbo].[DbErrorLogs] ADD CONSTRAINT [DF_DbErrorLogs_ExceptionDate]  DEFAULT (getdate()) FOR [ErrorLogDate]

	print 'created'
end
else
	print 'table DbErrorLogs already exists'
	
GO
