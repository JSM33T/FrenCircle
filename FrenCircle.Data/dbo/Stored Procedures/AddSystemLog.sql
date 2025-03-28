CREATE PROCEDURE AddSystemLog
    @Level VARCHAR(20),
    @Message VARCHAR(MAX),
    @Context VARCHAR(255) = NULL
AS
BEGIN
    INSERT INTO SystemLogs (RowId, Level, Message, Context, CreatedAt)
    VALUES (NEWID(), @Level, @Message, @Context, GETDATE());
END