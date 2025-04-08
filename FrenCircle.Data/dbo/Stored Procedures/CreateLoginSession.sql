CREATE PROCEDURE CreateLoginSession
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

    INSERT INTO LoginSessions (
        UserLoginId, AccessToken, RefreshToken, IssuedAt ,ExpiresAt,
        DeviceId, IPAddress, UserAgent
    )
    VALUES (
        @UserLoginId, @AccessToken, @RefreshToken,@IssuedAt, @ExpiresAt,
        @DeviceId, @IPAddress, @UserAgent
    );

    SELECT SCOPE_IDENTITY() AS SessionId;
END


--EXEC CreateLoginSession 
--    @UserLoginId = 1,
--    @AccessToken = 'sample_access_token',
--    @RefreshToken = 'sample_refresh_token',
--    @ExpiresAt = DATEADD(HOUR, 1, GETDATE()),
--    @DeviceId = 'device-abc-123',
--    @IPAddress = '192.168.1.100',
--    @UserAgent = 'Chrome on Windows 10';
