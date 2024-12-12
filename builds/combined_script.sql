-- Table: tblRoles
CREATE TABLE [dbo].[tblRoles] (
    [Id]            INT PRIMARY KEY,
    [RoleName]      NVARCHAR(50) NOT NULL,
    [Description]   NVARCHAR(255) NOT NULL -- Extended description length for more details
);

-- SEED DATA
INSERT INTO [dbo].[tblRoles] ([Id], [RoleName], [Description])
VALUES
    (1, 'Admin', 'Administrator with full permissions'),
    (2, 'Moderator', 'Moderator with limited management permissions'),
    (3, 'Member', 'Regular user with basic access')
GO
-- Table: tblFrens
CREATE TABLE [dbo].[tblFrens] (
    [Id]            INT PRIMARY KEY, -- Auto-increment can be added if needed (IDENTITY(1,1))
    [FirstName]     NVARCHAR(50) NOT NULL,
    [LastName]      NVARCHAR(50),
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
GO
-- Table: tblLoginProviders
CREATE TABLE [dbo].[tblLoginProviders] (
    [Id]            INT PRIMARY KEY, -- Auto-increment if needed
    [ProviderId]    NVARCHAR(50) NOT NULL,
    [ProviderName]  NVARCHAR(50) NOT NULL,
    [IsActive]      BIT NOT NULL DEFAULT 1
);

-- SEED DATA
INSERT INTO [dbo].[tblLoginProviders] ([Id], [ProviderId], [ProviderName], [IsActive])
VALUES
    (1, 'google', 'Google', 1),
    (2, 'facebook', 'Facebook', 1),
    (3, 'twitter', 'Twitter', 1),
    (4, 'microsoft', 'Microsoft', 1);

GO
CREATE TABLE [dbo].[tblLogins] (
    [Id]				INT PRIMARY KEY, -- Auto-increment if needed
    [FrenId]			INT NOT NULL, -- FK reference to tblFrens
    [ProviderId]		INT NOT NULL, -- FK reference to tblLoginProviders.ProviderId
    [ProviderUId]		NVARCHAR(256),
    [ProviderKey]       NVARCHAR(256),
    [DateAdded]			DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT		FK_tblLogins_Fren FOREIGN KEY ([FrenId]) REFERENCES [dbo].[tblFrens]([Id]),
    CONSTRAINT		FK_tblLogins_ProviderId FOREIGN KEY ([ProviderId]) REFERENCES [dbo].[tblLoginProviders]([Id])
);
GO
