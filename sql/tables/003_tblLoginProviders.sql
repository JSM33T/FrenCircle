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
