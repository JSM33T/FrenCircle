CREATE TABLE [dbo].[GlobalSettings] (
    [Id]          INT              IDENTITY (1, 1) NOT NULL,
    [RowId]       UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [Name]        NVARCHAR (100)   NOT NULL,
    [Value]       NVARCHAR (MAX)   NOT NULL,
    [Description] NVARCHAR (256)   NULL,
    [CreatedAt]   DATETIME         DEFAULT (getdate()) NOT NULL,
    [UpdatedAt]   DATETIME         NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Name] ASC)
);

