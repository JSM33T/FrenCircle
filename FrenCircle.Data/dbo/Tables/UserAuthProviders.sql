-- AUTH PROVIDERS
CREATE TABLE UserAuthProviders (

    [Id]				INT					PRIMARY KEY IDENTITY(1,1),
    [RowId]				UNIQUEIDENTIFIER	NOT NULL DEFAULT NEWID(),
    [UserId]			INT					NOT NULL,
    [Provider]			NVARCHAR(50)		NOT NULL,
    [ProviderId]		NVARCHAR(255)		NOT NULL,
    [PasswordHash]		NVARCHAR(255)		NULL,
    [CreatedAt]			DATETIME			NOT NULL DEFAULT GETDATE(),
    
	CONSTRAINT FK_UserAuthProviders_UserId 
		FOREIGN KEY (UserId) 
		REFERENCES Users(Id)
);