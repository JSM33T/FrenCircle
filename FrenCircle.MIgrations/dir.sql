-- =======================================
-- tblFrens | Main Frens Table
-- =======================================

CREATE TABLE tblFrens(

	FrenId		INT					IDENTITY(1,1)	PRIMARY KEY,

	Username	NVARCHAR(128)		NOT NULL,

	FirstName	NVARCHAR(128)		NOT NULL,

	LastName	NVARCHAR(128),

	Bio			NVARCHAR(512),

	Avatar		NVARCHAR(512),

	NickName	NVARCHAR(128),

	DateBorN	DATETIME,

	Gender		NVARCHAR(1),

	IsVerified	BIT					NOT NULL		DEFAULT(0),

	CreatedAt	DATETIME			NOT NULL		DEFAULT(GETDATE()),

	TimeSpent	INT					NOT NULL		DEFAULT(100),

	[UID]		UNIQUEIDENTIFIER	NOT NULL		DEFAULT NEWID()

);


-- =======================================
-- Fren : Manage User Verifications
-- =======================================

CREATE TABLE tblAccountVerifications (

    Id			INT				IDENTITY(1,1)	PRIMARY KEY,      

    FrenId		INT					NOT NULL		FOREIGN KEY (FrenId)	REFERENCES tblFrens(FrenId)		ON DELETE CASCADE,
	
    OTP			NVARCHAR(6)			NOT NULL,

	UK			UNIQUEIDENTIFIER	NOT NULL DEFAULT(NEWID()),
	
    ValidTill	DATETIME			NOT NULL, 
	
    IsVerified	BIT					NOT NULL		DEFAULT 0, 
	
    CreatedAt	DATETIME			NOT NULL		DEFAULT GETDATE(),

    
);

GO

-- =======================================
-- Fren : Get Verification Data From FrenID
-- =======================================

CREATE PROCEDURE sproc_GetVerificationsByFrenId
    @FrenId INT
AS
BEGIN
    SELECT	
		FrenId, OTP, UK, ValidTill, IsVerified, CreatedAt
    FROM
		tblAccountVerifications  
	WITH(NOLOCK)
    WHERE 
		FrenId = @FrenId;
END

GO
-- =======================================
-- Fren : Global Settings | Key-Value
-- =======================================

CREATE TABLE tblGlobalSettings (

    Id				INT					IDENTITY(1,1)	PRIMARY KEY,      

    [Key]			NVARCHAR(128)		NOT NULL,
	
    [Value]			NVARCHAR(256)		NOT NULL,
	
    CreatedAt		DATETIME			NOT NULL		DEFAULT GETDATE()

);
GO

-- =======================================
-- Sproc: Create User
-- =======================================

CREATE PROCEDURE sproc_AddNewFren
    @Username NVARCHAR(128),
    @FirstName NVARCHAR(128),
    @LastName NVARCHAR(128),
    @Bio NVARCHAR(512),
    @Avatar NVARCHAR(512),
    @NickName NVARCHAR(128),
    @DateOfBirth DATETIME,
    @Gender NVARCHAR(1)
AS
BEGIN
    INSERT INTO tblFrens (Username, FirstName, LastName, Bio, Avatar, NickName, DateBorN, Gender)
    VALUES (@Username, @FirstName, @LastName, @Bio, @Avatar, @NickName, @DateOfBirth, @Gender);
END
