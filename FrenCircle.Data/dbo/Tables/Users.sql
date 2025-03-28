-- DEFINITIONS
----------------------------------------------------------------------------------

-- USERS

CREATE TABLE Users (
	Id				INT					PRIMARY KEY IDENTITY(1,1),
	RowId			UNIQUEIDENTIFIER	NOT NULL DEFAULT NEWID(),
	FirstName		NVARCHAR(128)		NOT NULL,
	LastName		NVARCHAR(128)		NULL,
	Username		NVARCHAR(128)		NOT NULL UNIQUE,
	Email			NVARCHAR(255)		NOT NULL UNIQUE,
	ProfilePicUrl	NVARCHAR(500)		NULL,

	CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);