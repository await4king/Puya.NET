IF (OBJECT_ID('dbo.usp0_Log_error') IS NOT NULL)
begin
	print '		Already exists. dropping it ...'
	
	drop procedure dbo.usp0_Log_error

	print '		Dropped'
end

GO
print 'Creating usp0_Log_error sproc ...'
go
/****** Object:  StoredProcedure [dbo].[usp0_Log_error]    Script Date: 5/13/2018 4:14:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp0_Log_error]
(
	@args		nvarchar(1000) out 
)
AS
BEGIN
	SET NOCOUNT ON;

	declare @error_line int
	declare @error_message nvarchar(2048)
	declare @error_number int
	declare @error_procedure nvarchar(500)
	declare @error_severity int
	declare @error_state int

	set @error_line			= ERROR_LINE()
	set @error_message		= ERROR_MESSAGE()
	set @error_number		= ERROR_NUMBER()
	set @error_procedure	= ERROR_PROCEDURE()
	set @error_severity		= ERROR_SEVERITY()
	set @error_state		= ERROR_STATE()

	declare @result nvarchar(1000)

	INSERT INTO [dbo].[DbErrorLogs]
    (
		 [Command]
        ,[Number]
        ,[Description]
        ,[Line]
        ,[State]
        ,[Severity]
        ,[Args]
	)
    VALUES
    (
		 @error_procedure
        ,@error_number
        ,@error_message
        ,@error_line
        ,@error_state
        ,@error_severity
        ,@args
	)

	set @result =
				'{' +
					 '"Line":'			+ cast(isnull(@error_line, 0) as varchar(20)) +
					',"Number":'		+ cast(isnull(@error_number, 0) as varchar(20)) +
					',"Message":"'		+ isnull(@error_message, '') + '"' +
					',"Procedure":"'	+ isnull(@error_procedure, '') + '"' +
					',"Severity":'		+ cast(isnull(@error_severity, 0) as varchar(20)) +
					',"Args":"'			+ isnull(@args, '') + '"' +
					',"State":'			+ cast(isnull(@error_state, 0) as varchar(20)) +
				'}'

	set @args = @result
END
go
