-- Table: tblRoles
-- Description: Defines user roles within the application, including role names and their descriptions.

CREATE TABLE [dbo].[tblRoles] (

    [Id]            INT PRIMARY KEY,             -- Unique identifier for the role.

    [RoleName]      NVARCHAR(50) NOT NULL,       -- Name of the role (e.g., Admin, Moderator).

    [Description]   NVARCHAR(255) NOT NULL       -- Detailed description of the role's permissions and scope.
);

-- Seed Data
-- Adds default roles to the tblRoles table.
INSERT INTO [dbo].[tblRoles] ([Id], [RoleName], [Description])
VALUES
    (1, 'Admin', 'Administrator with full permissions'),                -- Full access role.

    (2, 'Moderator', 'Moderator with limited management permissions'),  -- Management role with restrictions.

    (3, 'Member', 'Regular user with basic access');                    -- Basic role for standard users.
