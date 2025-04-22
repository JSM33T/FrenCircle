CREATE PROCEDURE [dbo].[usp_VerifyEmail]
    @Token UNIQUEIDENTIFIER
AS
BEGIN
    UPDATE Users
    SET IsEmailVerified = 1,
        EmailVerificationToken = NULL
    WHERE EmailVerificationToken = @Token;

    IF @@ROWCOUNT = 0
    BEGIN
        RAISERROR('Invalid or expired token.', 16, 1);
    END
END
