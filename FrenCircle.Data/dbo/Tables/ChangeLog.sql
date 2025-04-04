CREATE TABLE ChangeLogs
(
    Id            INT              PRIMARY KEY IDENTITY(1,1),
    Version       NVARCHAR(50)     NOT NULL,
    Title         NVARCHAR(255)    NOT NULL,
    Description   NVARCHAR(MAX)    NULL,
    ChangeType    NVARCHAR(50)     NOT NULL,              -- e.g., 'Added', 'Removed', 'Changed'
    Contributors  NVARCHAR(500)    NULL,                  -- Comma-separated usernames
    ChangedAt     DATETIME         NOT NULL DEFAULT GETDATE(),
    RowId         UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
);