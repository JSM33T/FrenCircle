CREATE PROCEDURE [dbo].[usp_UpdateUserProfile]
    @Id INT,
    @FirstName NVARCHAR(128),
    @LastName NVARCHAR(128),
    @UserName NVARCHAR(128),
    @Gender NVARCHAR(1),
    @Bio NVARCHAR(256),
    @Avatar NVARCHAR(256),
    @ResultCode INT OUTPUT -- 0 = success, 1 = username conflict
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1 FROM Users
        WHERE UserName = @UserName AND Id <> @Id
    )
    BEGIN
        SET @ResultCode = 1;
        RETURN;
    END

    UPDATE Users
    SET
        FirstName = @FirstName,
        LastName = @LastName,
        UserName = @UserName,
        Gender = @Gender,
        Bio = @Bio,
        Avatar = @Avatar,
        UpdatedAt = GETDATE()
    WHERE Id = @Id;

    SET @ResultCode = 0;
END
