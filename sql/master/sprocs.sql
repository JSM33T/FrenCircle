CREATE OR ALTER PROCEDURE sproc_AddFrenWithLogin
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50) = NULL,
    @Username NVARCHAR(50),
    @Avatar NVARCHAR(255) = NULL,
    @RoleId INT = 3, -- Default to Member
    @IsActive BIT = 0,
    @ProviderId INT,
    @ProviderUId NVARCHAR(256),
    @ProviderKey NVARCHAR(256),
    @LoginIsActive BIT = 0
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION;

    BEGIN TRY

	 DECLARE @FrenID INT;
        DECLARE @LoginID INT;

	 SELECT @FrenId = ISNULL(MAX([Id]), 0) + 1 FROM tblFrens;
	 SELECT @LoginId = ISNULL(MAX([Id]), 0) + 1 FROM tblLogins;

        -- Insert into tblFrens
        INSERT INTO [dbo].[tblFrens] (Id,
            [FirstName], [LastName], [Username], [Email], 
            [IsActive]
        )
       
        VALUES (@FrenId,
            @FirstName, @LastName, @Username, @ProviderUId,
            @IsActive
        );

        -- Insert into tblLogins
        INSERT INTO [dbo].[tblLogins] (
           Id, [FrenId], [ProviderId], [ProviderUId], [ProviderKey], [IsActive]
        )
        VALUES (
           @LoginId,@FrenID, @ProviderId, @ProviderUId, @ProviderKey, @LoginIsActive
        );

        -- Commit the transaction
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- Rollback the transaction if there is an error
        ROLLBACK TRANSACTION;

        -- Re-throw the error for handling outside
        THROW;
    END CATCH;
END;
GO



GO
CREATE OR ALTER PROCEDURE sproc_VerifyLogin
    @SessionKey UNIQUEIDENTIFIER,
    @Result INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Check if the SessionKey exists
        IF NOT EXISTS (SELECT 1 FROM [dbo].[tblLogins] WHERE [SessionKey] = @SessionKey)
        BEGIN
            SET @Result = 2; -- No record found
            RETURN;
        END

        -- Check if the login is already active
        IF EXISTS (SELECT 1 FROM [dbo].[tblLogins] WHERE [SessionKey] = @SessionKey AND [IsActive] = 1)
        BEGIN
            SET @Result = 1; -- Already active
            RETURN;
        END

        -- Update IsActive to 1 for the specified SessionKey
        UPDATE [dbo].[tblLogins]
        SET [IsActive] = 1
        WHERE [SessionKey] = @SessionKey;

        SET @Result = 0; -- Success
    END TRY
    BEGIN CATCH
        -- Handle errors if needed
        THROW;
    END CATCH;
END;
GO


--DECLARE @Result INT;

--EXEC sp_VerifyLogin 
--    @SessionKey = '1539BE81-98A6-4398-830E-CD50E990A5301', 
--    @Result = @Result OUTPUT;

--SELECT @Result AS VerificationResult;

GO
-- Login
GO
