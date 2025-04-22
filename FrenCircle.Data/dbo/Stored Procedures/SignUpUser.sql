CREATE PROCEDURE [dbo].[usp_SignupUser]
    @FirstName NVARCHAR(128),
    @LastName NVARCHAR(128),
    @UserName NVARCHAR(128),
    @Email NVARCHAR(256),
    @PasswordHash NVARCHAR(512),
    @Salt NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserId INT, @ProviderId INT;
    DECLARE @EmailToken UNIQUEIDENTIFIER = NEWID();

    -- Check if username or email already exists
    IF EXISTS (SELECT 1 FROM Users WHERE UserName = @UserName OR Email = @Email)
    BEGIN
        RAISERROR('Username or Email already exists.', 16, 1);
        RETURN;
    END

    -- Insert into Users with email token
    INSERT INTO Users (FirstName, LastName, UserName, Email, EmailVerificationToken)
    VALUES (@FirstName, @LastName, @UserName, @Email, @EmailToken);

    SET @UserId = SCOPE_IDENTITY();

    -- Get or create Email provider
    SELECT @ProviderId = Id FROM LoginProviders WHERE [Name] = 'Email';
    IF @ProviderId IS NULL
    BEGIN
        INSERT INTO LoginProviders ([Name], [Description], ProviderId)
        VALUES ('Email', 'Email and password login', 'email');

        SET @ProviderId = SCOPE_IDENTITY();
    END

    -- Create login entry
    INSERT INTO UserLogins (UserId, ProviderId, Email, PasswordHash, Salt, IsPasswordLogin)
    VALUES (@UserId, @ProviderId, @Email, @PasswordHash, @Salt, 1);

    SELECT 
    @UserId AS UserId,
    @EmailToken AS EmailVerificationToken,
    @FirstName AS FirstName,
    @LastName AS LastName,
    @UserName AS UserName,
    @Email AS Email

END
