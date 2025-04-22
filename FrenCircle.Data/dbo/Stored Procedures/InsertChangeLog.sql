CREATE PROCEDURE [dbo].[usp_InsertChangeLog]
    @Version       NVARCHAR(50),
    @Title         NVARCHAR(255),
    @Description   NVARCHAR(MAX),
    @ChangeType    NVARCHAR(50),
    @Contributors  NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO ChangeLogs (
        Version,
        Title,
        Description,
        ChangeType,
        Contributors,
        ChangedAt,
        RowId
    )
    VALUES (
        @Version,
        @Title,
        @Description,
        @ChangeType,
        @Contributors,
        GETDATE(),
        NEWID()
    );

    SELECT SCOPE_IDENTITY() AS Id;
END
