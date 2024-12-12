-- Table: tblLoginProviders
-- Description: Stores information about login providers for authentication mechanisms.

CREATE TABLE [dbo].[tblLoginProviders] (

    [Id]            INT PRIMARY KEY,           -- Unique identifier for the login provider.

    [ProviderId]    NVARCHAR(50) NOT NULL,     -- Short identifier for the provider (e.g., 'google').

    [ProviderName]  NVARCHAR(50) NOT NULL,     -- Display name of the provider (e.g., 'Google').

    [IsActive]      BIT NOT NULL DEFAULT 1     -- Status indicating whether the provider is active (1 = active, 0 = inactive).
);

-- Seed Data
-- Adds initial login providers to the tblLoginProviders table.

INSERT INTO [dbo].[tblLoginProviders] ([Id], [ProviderId], [ProviderName], [IsActive])
VALUES
    (1, 'google', 'Google', 1),       -- Google login provider.

    (2, 'facebook', 'Facebook', 1),  -- Facebook login provider.

    (3, 'twitter', 'Twitter', 1),    -- Twitter login provider.

    (4, 'microsoft', 'Microsoft', 1) -- Microsoft login provider.
