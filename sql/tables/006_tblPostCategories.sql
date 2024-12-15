
CREATE TABLE [dbo].[tblPostCategories] (

    [Id]            INT PRIMARY KEY,            

    [Title]			NVARCHAR(50) NOT NULL,       

	[Description]   NVARCHAR(255) NOT NULL,   

	[DateAdded]		DATETIME NOT NULL DEFAULT(GETDATE()),

	IsActive		BIT NOT NULL DEFAULT(0)
);

INSERT INTO [dbo].[tblPostCategories] ([Id], [Title], [Description], [DateAdded], [IsActive])
VALUES 
(0, 'Untagged', 'Untagged posts', GETDATE(), 1),
(1, 'Tech', 'Technology-related posts', GETDATE(), 1),
(2, 'Lifestyle', 'Lifestyle tips and stories', GETDATE(), 1);