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
