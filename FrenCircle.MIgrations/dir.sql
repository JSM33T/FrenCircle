-- =======================================
-- tblFrens | Main Frens Table
-- =======================================

CREATE TABLE tblFrens(

	FrenId		INT					IDENTITY(1,1)	PRIMARY KEY,

	Username	NVARCHAR(128)		NOT NULL CONSTRAINT UK_tblFrens_Username UNIQUE ,

	FirstName	NVARCHAR(128)		NOT NULL,

	LastName	NVARCHAR(128),

	[Password]	NVARCHAR(128),

	Email		NVARCHAR(256)		NOT NULL CONSTRAINT UK_tblFrens_Email UNIQUE ,

	Bio			NVARCHAR(512),

	Avatar		NVARCHAR(512),

	NickName	NVARCHAR(128),

	DateBorn	DATETIME,

	OTP			NVARCHAR(6)			NOT NULL,

	Gender		NVARCHAR(1),

	IsVerified	BIT					NOT NULL		DEFAULT(0),

	CreatedAt	DATETIME			NOT NULL		DEFAULT(GETDATE()),

	TimeSpent	INT					NOT NULL		DEFAULT(100),

	[UID]		UNIQUEIDENTIFIER	NOT NULL		DEFAULT NEWID()

);

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
-- Sproc: Create User	EXEC sproc_InsertFren 'jsm33t','jassi','jskainthofficial@gmail.com','singh','bio here','password','avatar.png','jassi','2024/04/04','M',0,100,111111,'2024/12/12'
-- =======================================

CREATE OR ALTER PROCEDURE sproc_InsertFren
    @Username NVARCHAR(128),
    @FirstName NVARCHAR(128),
	@Email	NVARCHAR(256),
    @LastName NVARCHAR(128) = '',
    @Bio NVARCHAR(512) = '',
	@Password NVARCHAR(128),
    @Avatar NVARCHAR(512) = '',
    @NickName NVARCHAR(128) = '',
    @DateBorn DATETIME = NULL,
    @Gender NVARCHAR(1) = 'n',
    @IsVerified BIT = 0,
    @TimeSpent INT  = '100',
	@OTP INT,
	@ValidTill DATETIME

AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO tblFrens (Username, FirstName, LastName,[Password], Bio,Email, Avatar, NickName, DateBorn, Gender, IsVerified, TimeSpent, [UID], CreatedAt,OTP)
    VALUES (@Username, @FirstName, @LastName,@Password, @Bio,@Email, @Avatar, @NickName, @DateBorn, @Gender, @IsVerified, @TimeSpent, NEWID(), GETDATE(),@OTP);

    SELECT SCOPE_IDENTITY() AS FrenId; -- Return the ID of the newly inserted Fren
END

GO

-- =======================================
-- Fren : Login If Verified
-- =======================================

CREATE OR ALTER PROCEDURE sproc_FrenLogin
    @Username NVARCHAR(128),
	@Password	NVARCHAR(128)
AS
BEGIN
    SELECT	*
    FROM
		tblFrens  
	WITH(NOLOCK)
    WHERE 
		Username = @Username and Password = @Password;
END
GO

 --=======================================
 --Fren : VerifyFren	EXEC sproc_VerifyFren 'jsm33t','111111'
 --=======================================

CREATE OR ALTER PROCEDURE sproc_VerifyFren
    @Username NVARCHAR(128),
	@OTP	NVARCHAR(4)
AS
BEGIN

	DECLARE @FrenId INT;

	UPDATE	tblFrens
	SET IsVerified = 1
	WHERE FrenId = @FrenId
	AND OTP = @OTP

	IF @@ROWCOUNT = 0
    BEGIN  
        SELECT 0 AS Result;
    END
    ELSE
    BEGIN
        SELECT 1 AS Result;
    END
END