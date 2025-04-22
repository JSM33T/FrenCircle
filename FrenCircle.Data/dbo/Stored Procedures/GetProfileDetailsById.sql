CREATE PROCEDURE [dbo].[usp_GetProfileDetailsById]
    @Id NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT RowId as UserId, FirstName,LastName,UserName,CreatedAt,Avatar,Gender,Email,RoleId,Bio FROM Users WHERE Id = @Id;
END
