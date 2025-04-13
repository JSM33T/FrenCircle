---------------------------------------
-- version	:	1
-- date		:	2025/03/28
-- comment	:	Seed roles
---------------------------------------
INSERT INTO [dbo].[Roles] ([Id], [Name], [Slug], [Description], [CreatedAt], [RowId])
VALUES 

(1, 'User', 'user', 'Standard registered user', GETDATE(), NEWID()),
(2, 'Moderator', 'moderator', 'Can manage posts and comments', GETDATE(), NEWID()),
(3, 'Admin', 'admin', 'Administrator with full access', GETDATE(), NEWID())
