CREATE TABLE [dbo].[tblMembers] (

	-- Max(Id) + 1
    [Id]			INT				PRIMARY KEY,
    
	[FirstName]		NVARCHAR(50)	NOT NULL,
    
	[LastName]		NVARCHAR(50)	NOT NULL,
    
	[Username]		NVARCHAR(50)	NOT NULL	UNIQUE,
    
	[Email]			NVARCHAR(100)	NOT NULL	UNIQUE,
    
	[Avatar]		NVARCHAR(255),
    
	[Role]			NVARCHAR(50)	NOT NULL DEFAULT('member'),
    
	[GoogleId]		NVARCHAR(255),
    
	[Key]			NVARCHAR(255)	NOT NULL DEFAULT(NEWID()),    
    
	[IsActive]		BIT				NOT NULL	DEFAULT 0,
    
	[ExpPoints]		INT				NOT NULL	DEFAULT 100,
    
	[TimeSpent]		INT				NOT NULL	DEFAULT 100,
    
	[DateAdded]		DATETIME		NOT NULL	DEFAULT GETDATE(),
    
	[DateEdited]	DATETIME		NOT NULL	DEFAULT GETDATE(),

    [Bio]			NVARCHAR(1000)	NOT NULL	DEFAULT('Hey ther!')
	
);
