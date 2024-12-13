CREATE OR ALTER PROCEDURE sproc_AddFrenAndLogin

    @FirstName      NVARCHAR(100),
	@Username       NVARCHAR(100),
    @Email          NVARCHAR(100),
    @PasswordHash   NVARCHAR(256)
    @LoginProvider  INT = 1

AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION;

    BEGIN TRY

        DECLARE @FrenID INT;

        DECLARE @LoginID INT;

        SELECT @FrenID = ISNULL(MAX([Id]), 0) + 1 FROM tblFrens;
        SELECT @LoginID = ISNULL(MAX([Id]), 0) + 1 FROM tblLogins;

        -- Insert into tblFren
        INSERT INTO tblFrens    (   Id,        FirstName,  Username,   Email,  [Key]       )
        VALUES                  (   @FrenID,   @FirstName, @Username,  @Email, NEWID()     );
        
        INSERT INTO tblLogins   (   Id,        FrenId,     [ProviderId],   ProviderUId,    ProviderKey,    DateAdded    )
        VALUES                  (   @LoginID,  @FrenID,    1,              @Email,         @PasswordHash,  GETDATE()    );

        COMMIT TRANSACTION;
    
    END TRY
    
    BEGIN CATCH

        ROLLBACK TRANSACTION;
        THROW;
    
    END CATCH
END;

-- Sample Execution
/*
EXEC sproc_AddFrenAndLogin 
    @FirstName = N'John', 
    @Username = N'john_doe', 
    @Email = N'john.doe@example.com', 
    @PasswordHash = N'hashed_password_value';
*/
GO
