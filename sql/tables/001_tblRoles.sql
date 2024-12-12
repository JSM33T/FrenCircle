-- Table: tblRoles
CREATE TABLE [dbo].[tblRoles] (
    [Id]            INT PRIMARY KEY,
    [RoleName]      NVARCHAR(50) NOT NULL,
    [Description]   NVARCHAR(255) NOT NULL -- Extended description length for more details
);

-- SEED DATA
INSERT INTO [dbo].[tblRoles] ([Id], [RoleName], [Description])
VALUES
    (1, 'Admin', 'Administrator with full permissions'),
    (2, 'Moderator', 'Moderator with limited management permissions'),
    (3, 'Member', 'Regular user with basic access')