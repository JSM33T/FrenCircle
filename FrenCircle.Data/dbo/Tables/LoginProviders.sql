CREATE TABLE LoginProviders
(
    Id              INT                 PRIMARY KEY IDENTITY(1,1),
    [Name]          NVARCHAR(128)       NOT NULL, -- e.g. 'Email', 'Google'
    [Description]   NVARCHAR(128)       NOT NULL,
    ProviderId      NVARCHAR(128)       NOT NULL, -- e.g. 'email', 'google'
    CreatedAt       DATETIME            NOT NULL DEFAULT GETDATE(),
    RowId           UNIQUEIDENTIFIER    NOT NULL DEFAULT NEWID()
);
