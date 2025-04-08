---------------------------------------
-- version	:	1
-- date		:	2025/03/28
-- comment	:	Seed roles
---------------------------------------
SET IDENTITY_INSERT Roles ON;
GO

INSERT INTO Roles (Id, RowId, Name, CreatedAt) VALUES 
(1, NEWID(), 'User', GETDATE()),
(2, NEWID(), 'Moderator', GETDATE()),
(3, NEWID(), 'Admin', GETDATE());
GO

SET IDENTITY_INSERT Roles OFF;