CREATE PROCEDURE [dbo].[usp_CreateLoginSession]
    @UserLoginId INT,
    @AccessToken NVARCHAR(512),
    @RefreshToken NVARCHAR(512),
    @ExpiresAt DATETIME,
    @IssuedAt DATETIME,
    @DeviceId NVARCHAR(256),
    @IPAddress NVARCHAR(64),
    @UserAgent NVARCHAR(512)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1 FROM LoginSessions
        WHERE DeviceId = @DeviceId AND UserLoginId = @UserLoginId
    )
    BEGIN
        UPDATE LoginSessions
        SET 
            AccessToken = @AccessToken,
            RefreshToken = @RefreshToken,
            IssuedAt = @IssuedAt,
            ExpiresAt = @ExpiresAt,
            IPAddress = @IPAddress,
            UserAgent = @UserAgent
        WHERE DeviceId = @DeviceId AND UserLoginId = @UserLoginId;

        SELECT Id
        FROM LoginSessions
        WHERE DeviceId = @DeviceId AND UserLoginId = @UserLoginId;
    END
    ELSE
    BEGIN
        INSERT INTO LoginSessions (
            UserLoginId, AccessToken, RefreshToken, IssuedAt, ExpiresAt,
            DeviceId, IPAddress, UserAgent
        )
        VALUES (
            @UserLoginId, @AccessToken, @RefreshToken, @IssuedAt, @ExpiresAt,
            @DeviceId, @IPAddress, @UserAgent
        );

        SELECT SCOPE_IDENTITY() AS SessionId;
    END
END
