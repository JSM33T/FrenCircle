-- Table: tblRoles
-- Description: Defines user roles within the application, including role names and their descriptions.

CREATE TABLE [dbo].[tblRoles] (

    [Id]            INT PRIMARY KEY,             -- Unique identifier for the role.

    [RoleName]      NVARCHAR(50) NOT NULL,       -- Name of the role (e.g., Admin, Moderator).

    [Description]   NVARCHAR(255) NOT NULL       -- Detailed description of the role's permissions and scope.
);

-- Seed Data
-- Adds default roles to the tblRoles table.
INSERT INTO [dbo].[tblRoles] ([Id], [RoleName], [Description])
VALUES
    (1, 'Admin', 'Administrator with full permissions'),                -- Full access role.

    (2, 'Moderator', 'Moderator with limited management permissions'),  -- Management role with restrictions.

    (3, 'Member', 'Regular user with basic access');                    -- Basic role for standard users.

GO
-- Table: tblFrens
-- Description: Stores user information including personal details, authentication, and activity stats.
-- References: tblRoles via FK_tblFrens_Role.

CREATE TABLE [dbo].[tblFrens] (         
    [Id]            INT             PRIMARY KEY,                    -- Unique identifier for the user.

    [FirstName]     NVARCHAR(50)    NOT NULL,                       -- First name of the user.

    [LastName]      NVARCHAR(50),                                   -- Last name of the user (optional).

    [Username]      NVARCHAR(50)    NOT NULL UNIQUE,                -- Unique username for the user.

    [Email]         NVARCHAR(100)   NOT NULL UNIQUE,                -- Unique email address for the user.
    
    [Avatar]        NVARCHAR(255),                                  -- URL or path to the user's avatar (optional).

    [Role]          INT             NOT NULL DEFAULT(3),            -- Role ID (default: 3 - standard user).

    [Key]           NVARCHAR(255)   NOT NULL DEFAULT(NEWID()),      -- Unique key for user (GUID).

    [IsActive]      BIT             NOT NULL DEFAULT 0,             -- Status of the account (active/inactive).

    [ExpPoints]     INT             NOT NULL DEFAULT 100,           -- Experience points earned by the user.

    [TimeSpent]     INT             NOT NULL DEFAULT 100,           -- Time spent in minutes (default value).

    [DateAdded]     DATETIME        NOT NULL DEFAULT GETDATE(),     -- Record creation timestamp.

    [DateEdited]    DATETIME        NOT NULL DEFAULT GETDATE(),     -- Last modified timestamp.

    [Bio]           NVARCHAR(1000)  NOT NULL DEFAULT('Hey there!'), -- User biography (default text).

    -- Foreign Key Constraints
    CONSTRAINT FK_tblFrens_Role FOREIGN KEY ([Role]) REFERENCES [dbo].[tblRoles]([Id])
);

GO
-- Table: tblLoginProviders
-- Description: Stores information about login providers for authentication mechanisms.

CREATE TABLE [dbo].[tblAuthProviders] (

    [Id]            INT PRIMARY KEY,           -- Unique identifier for the login provider.

    [ProviderName]  NVARCHAR(50) NOT NULL,     -- Display name of the provider (e.g., 'Google').

    [IsActive]      BIT NOT NULL DEFAULT 1     -- Status indicating whether the provider is active (1 = active, 0 = inactive).
);

-- Seed Data
-- Adds initial login providers to the tblLoginProviders table.

INSERT INTO [dbo].[tblAuthProviders] ([Id], [ProviderName], [IsActive])
VALUES
    (1, 'Default Email Provider', 1),    -- Default FC login provider.

    (2, 'Google', 1)  -- Google login provider.

GO
-- Table: tblLogins
-- Description: Stores login details for users, linking them to specific login providers.

CREATE TABLE [dbo].[tblLogins] (
    [Id]            INT PRIMARY KEY,                                -- Unique identifier for the login record.

    [FrenId]        INT                 NOT NULL,                   -- Foreign key referencing tblFrens(Id).

    [ProviderId]    INT                 NOT NULL,                   -- Foreign key referencing tblLoginProviders(Id).

    [ProviderUId]   NVARCHAR(256),                                  -- Unique identifier provided by the login provider.

    [ProviderKey]   NVARCHAR(256),                              -- Key or token for the provider.

    [SessionKey]    UNIQUEIDENTIFIER    NOT NULL DEFAULT(NEWID()),        

    [DateAdded]     DATETIME            NOT NULL DEFAULT GETDATE(), -- Record creation timestamp.

    [IsActive]      BIT                 NOT NULL DEFAULT 0,         -- Status indicating active login (1 = active, 0 = inactive).

    -- Foreign Key Constraints
    
    CONSTRAINT FK_tblLogins_Fren FOREIGN KEY ([FrenId]) REFERENCES [dbo].[tblFrens]([Id]),

    CONSTRAINT FK_tblLogins_ProviderId FOREIGN KEY ([ProviderId]) REFERENCES [dbo].[tblAuthProviders]([Id])
);
GO
