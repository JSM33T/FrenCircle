CREATE TABLE Messages (
    [Id]           INT              IDENTITY(1,1) PRIMARY KEY,

    [Name]         NVARCHAR(128)    NOT NULL DEFAULT 'anonymous',

    [Email]        NVARCHAR(256)    NOT NULL,

    [Text]         NVARCHAR(MAX)    NOT NULL,

    [DateAdded]    DATETIME         NOT NULL DEFAULT GETDATE(),

    CONSTRAINT     UC_Message_Email UNIQUE ([Email])
);

CREATE INDEX IX_Messages_Email ON Messages(Email);

CREATE TABLE Users (
    [Id]                INT                 IDENTITY(1,1)   PRIMARY KEY,

    [FirstName]         NVARCHAR(128)       NOT NULL,

    [LastName]          NVARCHAR(128)       NOT NULL    DEFAULT 'anonymous',

    [UserName]          NVARCHAR(128)       NOT NULL,

    [Email]             NVARCHAR(256)       NOT NULL,
    
    [Bio]               NVARCHAR(512)       NOT NULL    DEFAULT('')
    
    [PasswordHash]      NVARCHAR(256)       NOT NULL,
    
    [Salt]              NVARCHAR(256)       NOT NULL,
    
    [TimeSpent]         DATETIME            NOT NULL    DEFAULT(60),

    [DateUpdated]       DATETIME            NOT NULL    DEFAULT GETDATE(),

    [LastSeen]          DATETIME            NOT NULL    DEFAULT GETDATE(),

    [DateAdded]         DATETIME            NOT NULL    DEFAULT GETDATE(),

    CONSTRAINT          UC_Users_UserName   UNIQUE ([UserName]),

    CONSTRAINT          UC_Users_Email      UNIQUE ([Email])
    );

CREATE INDEX IX_Users_Email ON Users(Email);