CREATE OR ALTER PROCEDURE sproc_AddFrenAndLogin
    @FirstName      NVARCHAR(100),
    @Username       NVARCHAR(100),
    @Email          NVARCHAR(100),
    @PasswordHash   NVARCHAR(256),
    @Salt           NVARCHAR(256),

    @ProviderUId    NVARCHAR(256),
    @ProviderKey    NVARCHAR(256)

    @LoginProvider  INT = 0
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION;

    BEGIN TRY

        DECLARE @FrenID INT;
        DECLARE @LoginID INT;

        SELECT @FrenID = ISNULL(MAX([Id]), 0) + 1 FROM tblFrens;
        SELECT @LoginID = ISNULL(MAX([Id]), 0) + 1 FROM tblLogins;

        -- Insert into tblFrens
        INSERT INTO tblFrens 
            (
                Id,
                FirstName,
                Username,
                Email,
                [Key],
                PasswordHash,
                Salt
            )
        VALUES 
            (
                @FrenID,
                @FirstName,
                @Username,
                @Email,
                NEWID(),
                CASE WHEN @LoginProvider = 0 THEN @PasswordHash ELSE NULL END,
                CASE WHEN @LoginProvider = 0 THEN @Salt ELSE NULL END
            );

        INSERT INTO tblLogins
            (
                Id,
                FrenId,
                ProviderId,
                ProviderUId,
                ProviderKey,
                DateAdded
            )
        VALUES
            (
                @LoginID,
                @FrenID,
                @LoginProvider,
                @Email,
                @ProviderKey ,
                GETDATE()
            );

        COMMIT TRANSACTION;

    END TRY

    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
