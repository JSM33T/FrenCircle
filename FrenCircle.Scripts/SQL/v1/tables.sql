CREATE TABLE Messages
(
    [Id]        INT IDENTITY (1,1) PRIMARY KEY,

    [Name]      NVARCHAR(128) NOT NULL DEFAULT 'anonymous',

    [Email]     NVARCHAR(256) NOT NULL,

    [Text]      NVARCHAR(MAX) NOT NULL,

    [Origin]    NVARCHAR(MAX) NOT NULL DEFAULT 'na',

    [DateAdded] DATETIME      NOT NULL DEFAULT GETDATE(),

    CONSTRAINT UC_Message_Email UNIQUE ([Email])
);

CREATE TABLE Roles
(
    [Id]          INT PRIMARY KEY,

    [Name]        NVARCHAR(128) NOT NULL,

    [Description] NVARCHAR(256) NOT NULL
);

INSERT INTO Roles(Id,Name,Description)
VALUES (1,'USER','Basic User Rights')

INSERT INTO Roles(Id,Name,Description)
VALUES (2,'MOD','Moderator')

INSERT INTO Roles(Id,Name,Description)
VALUES (3,'ADMIN','Supreme Overlord')

CREATE INDEX IX_Messages_Email ON Messages (Email);

CREATE TABLE Users
(
    [Id]           INT IDENTITY (1,1) PRIMARY KEY,

    [FirstName]    NVARCHAR(128)    NOT NULL,

    [LastName]     NVARCHAR(128)    NOT NULL DEFAULT 'anonymous',

    [UserName]     NVARCHAR(128)    NOT NULL,

    [Email]        NVARCHAR(256)    NOT NULL,

    [Bio]          NVARCHAR(512)    NOT NULL DEFAULT (''),

    [RoleId]        INT             NOT NULL DEFAULT (1),

    [PasswordHash] NVARCHAR(256)    NOT NULL,

    [Salt]         NVARCHAR(256)    NOT NULL,

    [UId]          UNIQUEIDENTIFIER NOT NULL DEFAULT (NEWID()),

    [TimeSpent]    DATETIME         NOT NULL DEFAULT (60),

    [DateUpdated]  DATETIME         NOT NULL DEFAULT GETDATE(),

    [LastSeen]     DATETIME         NOT NULL DEFAULT GETDATE(),

    [DateAdded]    DATETIME         NOT NULL DEFAULT GETDATE(),
    
    IsActive        BIT     NOT NULL DEFAULT (0),

    OTP            INT,

    OTPTimeStamp        DATETIME,

    CONSTRAINT FK_Users_Roles_RoleId  FOREIGN KEY (RoleId) REFERENCES Roles(Id),
    
    CONSTRAINT UC_Users_UserName UNIQUE ([UserName]),

    CONSTRAINT UC_Users_Email UNIQUE ([Email])
);

CREATE INDEX IX_Users_Email ON Users (Email);
