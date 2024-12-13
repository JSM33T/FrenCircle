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