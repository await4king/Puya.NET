GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
if exists(select 1 from sys.procedures where name = 'usp1_Paging_calc')
	drop proc dbo.usp1_Paging_calc
GO
CREATE PROCEDURE [dbo].[usp1_Paging_calc]
	@Page			INT OUT,
	@PageSize		INT OUT,
	@RecordCount	INT	OUT,
	@PageCount		INT OUT,
	@RowFrom		INT OUT,
	@RowTo			INT OUT
AS
BEGIN
	SET @Page = isnull(@Page, 1)
	SET @PageSize = isnull(@PageSize, 10)
	SET @RecordCount = isnull(@RecordCount, 0)

	if @Page <= 0
		set @Page = 1

	if @PageSize <= 0 or @PageSize > 100
		set @PageSize = 10

	if @RecordCount < 0
		set @RecordCount = 0

	set @PageCount = @RecordCount / @PageSize
	
	if @RecordCount > @PageCount * @PageSize
		set @PageCount = @PageCount + 1

	if @Page > @PageCount
		set @Page = @PageCount

	set @RowFrom = (@Page - 1) * @PageSize + 1
	set @RowTo = @RowFrom + @PageSize - 1
END