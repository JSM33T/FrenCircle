-- Table: tblLogins
CREATE TABLE [dbo].[tblLogins] (
    [Id]            INT PRIMARY KEY, -- Auto-increment if needed
    [FrenId]        INT NOT NULL, -- FK reference to tblFrens
    [AuthSource]    NVARCHAR(50) NOT NULL, -- FK reference to tblLoginProviders.ProviderId
    [AuthSourceUID] NVARCHAR(256) NOT NULL,
    [DateAdded]     DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_tblLogins_Fren FOREIGN KEY ([FrenId]) REFERENCES [dbo].[tblFrens]([Id]),
    CONSTRAINT FK_tblLogins_AuthSource FOREIGN KEY ([AuthSource]) REFERENCES [dbo].[tblLoginProviders]([ProviderId])
);