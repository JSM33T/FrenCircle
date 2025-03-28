CREATE PROCEDURE AddAuditLog
    @UserId INT = NULL,
    @TableName VARCHAR(100),
    @RecordId VARCHAR(100),
    @Action VARCHAR(20),
    @OldData VARCHAR(MAX) = NULL,
    @NewData VARCHAR(MAX) = NULL
AS
BEGIN
    INSERT INTO AuditLogs (RowId, UserId, TableName, RecordId, Action, OldData, NewData, CreatedAt)
    VALUES (NEWID(), @UserId, @TableName, @RecordId, @Action, @OldData, @NewData, GETDATE());
END