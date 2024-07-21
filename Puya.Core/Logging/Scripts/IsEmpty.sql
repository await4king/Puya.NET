print 'Checking IsEmpty UDF existence ...'
go
IF (OBJECT_ID('dbo.IsEmpty') IS NOT NULL)
begin
	print '		Already exists. dropping it ...'
	
	drop function dbo.IsEmpty

	print '		Dropped'
end
go

print 'Creating IsEmpty UDF ...'

GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create function [dbo].[IsEmpty](@x nvarchar(max)) returns bit
as
begin
	declare @result bit = 0

	if len(ltrim(rtrim(ISNULL(@x, '')))) = 0
		set @result = 1

	return @result
end
GO

print '		Created'
