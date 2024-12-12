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
