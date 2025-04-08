CREATE TABLE LoginSessions
(
    Id                  INT                 PRIMARY KEY IDENTITY(1,1),
    UserLoginId         INT                 NOT NULL FOREIGN KEY REFERENCES UserLogins(Id),
    AccessToken         NVARCHAR(512)       NOT NULL,
    RefreshToken        NVARCHAR(512)       NULL,
    IsActive            BIT                 NOT NULL DEFAULT 1,
    IssuedAt            DATETIME            NULL,
    ExpiresAt           DATETIME            NULL,
    LoggedOutAt         DATETIME            NULL,
    DeviceId            UNIQUEIDENTIFIER    NULL,
    IpAddress           NVARCHAR(64)        NULL,
    UserAgent           NVARCHAR(512)       NULL,
    RowId               UNIQUEIDENTIFIER    NOT NULL DEFAULT NEWID()
);
