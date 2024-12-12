-- Table: tblFrens
CREATE TABLE [dbo].[tblFrens] (
    [Id]            INT PRIMARY KEY, -- Auto-increment can be added if needed (IDENTITY(1,1))
    [FirstName]     NVARCHAR(50) NOT NULL,
    [LastName]      NVARCHAR(50) NOT NULL,
    [Username]      NVARCHAR(50) NOT NULL UNIQUE,
    [Email]         NVARCHAR(100) NOT NULL UNIQUE,
    [Avatar]        NVARCHAR(255),
    [Role]          INT NOT NULL DEFAULT(3), -- Default role, FK reference to tblRoles
    [Key]           NVARCHAR(255) NOT NULL DEFAULT(NEWID()),
    [IsActive]      BIT NOT NULL DEFAULT 0,
    [ExpPoints]     INT NOT NULL DEFAULT 100,
    [TimeSpent]     INT NOT NULL DEFAULT 100,
    [DateAdded]     DATETIME NOT NULL DEFAULT GETDATE(),
    [DateEdited]    DATETIME NOT NULL DEFAULT GETDATE(),
    [Bio]           NVARCHAR(1000) NOT NULL DEFAULT('Hey there!'),
    CONSTRAINT FK_tblFrens_Role FOREIGN KEY ([Role]) REFERENCES [dbo].[tblRoles]([Id])
);