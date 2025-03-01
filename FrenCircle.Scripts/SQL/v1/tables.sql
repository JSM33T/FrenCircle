
CREATE TABLE Messages
(
    [Id ]			INT				NOT NULL	IDENTITY(1,1) PRIMARY KEY,
	
    [Name]      	NVARCHAR(128) 	NOT NULL	DEFAULT 'anonymous',
		
    [Email]     	NVARCHAR(256) 	NOT NULL,	
		
    [Text]      	NVARCHAR(MAX) 	NOT NULL,	
		
    [Origin]    	NVARCHAR(MAX) 	NOT NULL	DEFAULT 'na',
		
    [DateAdded] 	DATETIME      	NOT NULL	DEFAULT GETDATE(),
);

CREATE TABLE Roles
(
    [Id]          	INT 			PRIMARY KEY,
	
    [Name]        	NVARCHAR(128) 	NOT NULL,
	
    [Description] 	NVARCHAR(256) 	NOT NULL
);

	INSERT INTO Roles(Id,Name,Description)
	VALUES (1,'USER','Basic User Rights')

	INSERT INTO Roles(Id,Name,Description)
	VALUES (2,'MOD','Moderator')

	INSERT INTO Roles(Id,Name,Description)
	VALUES (3,'ADMIN','Supreme Overlord')

CREATE TABLE Users
(
    [Id]           	INT 				 IDENTITY(1,1) NOT NULL PRIMARY KEY,

    [FirstName]    	NVARCHAR(128)    	NOT NULL,

    [LastName]     	NVARCHAR(128)    	NOT NULL DEFAULT 'anonymous',
	
    [UserName]     	NVARCHAR(128)    	NOT NULL,
    
    [Avatar]        NVARCHAR(128),
	
    [Email]        	NVARCHAR(256)    	NOT NULL,
	
    [Bio]          	NVARCHAR(512)    	NOT NULL DEFAULT (''),

    [RoleId]        INT             	NOT NULL DEFAULT (1),

    [PasswordHash] 	NVARCHAR(256)    	NOT NULL,
		
    [Salt]         	NVARCHAR(256)    	NOT NULL,
	
    [UId]          	UNIQUEIDENTIFIER 	NOT NULL DEFAULT (NEWID()),
	
    [TimeSpent]    	DATETIME         	NOT NULL DEFAULT (60),
	
    [DateUpdated]  	DATETIME         	NOT NULL DEFAULT GETDATE(),
	
    [LastSeen]     	DATETIME         	NOT NULL DEFAULT GETDATE(),
	
    [DateAdded]    	DATETIME         	NOT NULL DEFAULT GETDATE(),
    
    IsActive        BIT     			NOT NULL DEFAULT (0),

    OTP            	INT,

    OTPTimeStamp	DATETIME,

    CONSTRAINT FK_Users_Roles_RoleId  FOREIGN KEY (RoleId) REFERENCES Roles(Id)  ON DELETE CASCADE,
    
    CONSTRAINT UC_Users_UserName UNIQUE ([UserName]),

    CONSTRAINT UC_Users_Email UNIQUE ([Email])
);

CREATE INDEX IX_Users_Email ON Users (Email);


CREATE TABLE [Logins] (
    [Id]                INT                 IDENTITY(1,1) NOT NULL PRIMARY KEY,
    
    [UserId]            INT                 NOT NULL,
    
    [UserAgent]         NVARCHAR(512)       NOT NULL DEFAULT ('NA'),
    
    [DeviceId]          UNIQUEIDENTIFIER    NOT NULL DEFAULT (NEWID()),
    
    [DateAdded]         DATETIME            NOT NULL DEFAULT (GETDATE()),
    
    [Latitude]          DECIMAL(9, 6)       NOT NULL DEFAULT (0.000000),
    
    [Longitude]         DECIMAL(9, 6)       NOT NULL DEFAULT (0.000000),
    
    [IpAddress]         NVARCHAR(45)        NOT NULL DEFAULT ('NA'),
    
    [SessionExpiry]     DATETIME            NOT NULL DEFAULT (DATEADD(DAY, 1, GETDATE())),
    
    [LoginMethod]       NVARCHAR(50)        NOT NULL DEFAULT ('Standard'),
    
    [IsLoggedIn]        BIT                 NOT NULL DEFAULT ((1)),

    -- CONSTRAINTS
    
    CONSTRAINT [UK_Logins_UserId_DeviceId] UNIQUE ([UserId], [DeviceId]),

    CONSTRAINT [FK_Logins_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id])  ON DELETE CASCADE;
                            
)

CREATE TABLE [RefreshTokens] (
    [Id]          INT                 IDENTITY(1,1) NOT NULL PRIMARY KEY CLUSTERED,
    [UserId]      INT                 NOT NULL,
	[DeviceId]		UNIQUEIDENTIFIER	NOT NULL,
    [Token]       NVARCHAR(500)       NOT NULL,
    [ExpiresAt]   DATETIME            NOT NULL,
    [CreatedAt]   DATETIME            NOT NULL DEFAULT (GETUTCDATE())
) ON [PRIMARY];

-- Foreign Key Constraint
ALTER TABLE [RefreshTokens]
    ADD CONSTRAINT [FK_RefreshTokens_Users] FOREIGN KEY ([UserId])
    REFERENCES [Users] ([Id])
    ON DELETE CASCADE;


-- Ensure the foreign key constraint is valid
ALTER TABLE [RefreshTokens] CHECK CONSTRAINT [FK_RefreshTokens_Users];


CREATE TABLE dbo.Cache (

    Id                          NVARCHAR(449)   PRIMARY KEY,

    [Value]                     VARBINARY(MAX)  NOT NULL,

    ExpiresAtTime               DATETIMEOFFSET  NOT NULL,

    SlidingExpirationInSeconds  BIGINT          NULL,

    AbsoluteExpiration          DATETIMEOFFSET  NULL

);
