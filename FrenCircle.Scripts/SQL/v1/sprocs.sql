CREATE TABLE Messages (
    [Id]           INT              IDENTITY(1,1) PRIMARY KEY,

    [Name]         NVARCHAR(128)    NOT NULL DEFAULT 'anonymous',

    [Email]        NVARCHAR(256)    NOT NULL,

    [Text]         NVARCHAR(MAX)    NOT NULL,

    [DateAdded]    DATETIME         NOT NULL DEFAULT GETDATE(),

    CONSTRAINT     UC_Email UNIQUE ([Email])
);

CREATE INDEX IX_Email ON Messages(Email);