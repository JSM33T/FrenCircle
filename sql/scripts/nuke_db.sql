DECLARE @sql NVARCHAR(MAX) = ''

-- Generate SQL to drop foreign key constraints
SELECT @sql += 'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(parent_object_id)) + '.' + QUOTENAME(OBJECT_NAME(parent_object_id)) +
                ' DROP CONSTRAINT ' + QUOTENAME(name) + ';' + CHAR(13)
FROM sys.foreign_keys

-- Execute the generated SQL
EXEC sp_executesql @sql
GO
DECLARE @sql NVARCHAR(MAX) = ''

-- Generate SQL to drop primary key constraints
SELECT @sql += 'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(parent_object_id)) + '.' + QUOTENAME(OBJECT_NAME(parent_object_id)) +
                ' DROP CONSTRAINT ' + QUOTENAME(name) + ';' + CHAR(13)
FROM sys.key_constraints
WHERE type = 'PK'

-- Execute the generated SQL
EXEC sp_executesql @sql
GO
DECLARE @sql NVARCHAR(MAX) = ''

-- Generate SQL to drop unique constraints
SELECT @sql += 'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(parent_object_id)) + '.' + QUOTENAME(OBJECT_NAME(parent_object_id)) +
                ' DROP CONSTRAINT ' + QUOTENAME(name) + ';' + CHAR(13)
FROM sys.key_constraints
WHERE type = 'UQ'

-- Execute the generated SQL
EXEC sp_executesql @sql

GO
-- Disable foreign key constraints
EXEC sp_msforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'

-- Drop all tables
EXEC sp_msforeachtable 'DROP TABLE ?'
go
-- Drop all stored procedures
DECLARE @sql NVARCHAR(MAX) = ''

SELECT @sql += 'DROP PROCEDURE IF EXISTS ' + QUOTENAME(SCHEMA_NAME(schema_id)) + '.' + QUOTENAME(name) + ';'
FROM sys.procedures

EXEC sp_executesql @sql
