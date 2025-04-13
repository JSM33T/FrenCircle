CREATE PROCEDURE [dbo].[GetProfileDetailsById]
    @Id NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT RowId as Id, FirstName,LastName,UserName,CreatedAt,Email,RoleId,Bio FROM Users WHERE Id = @Id;
END
