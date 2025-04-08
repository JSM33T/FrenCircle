CREATE TABLE UserLogins
(
    Id              INT                 PRIMARY KEY IDENTITY(1,1),
    UserId          INT                 NOT NULL FOREIGN KEY REFERENCES Users(Id),
    ProviderId      INT                 NOT NULL FOREIGN KEY REFERENCES LoginProviders(Id),
    ProviderUserId  NVARCHAR(256)       NULL,  -- for OAuth providers
    Email           NVARCHAR(256)       NULL,  -- only for email provider
    PasswordHash    NVARCHAR(512)       NULL,  -- only for email provider
    Salt            NVARCHAR(256)       NULL,  -- only for email provider
    IsPasswordLogin BIT                 NOT NULL DEFAULT 0,
    LinkedAt        DATETIME            NOT NULL DEFAULT GETDATE(),
    RowId           UNIQUEIDENTIFIER    NOT NULL DEFAULT NEWID(),
    CONSTRAINT UQ_User_Provider UNIQUE(UserId, ProviderId)
);
