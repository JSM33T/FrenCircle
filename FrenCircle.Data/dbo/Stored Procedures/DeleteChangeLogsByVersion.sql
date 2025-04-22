CREATE PROCEDURE [dbo].[usp_DeleteChangeLogsByVersion]
    @Version NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM ChangeLogs WHERE Version = @Version;
END
