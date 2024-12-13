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

    (2, 'Google', 1),  -- Google login provider.
