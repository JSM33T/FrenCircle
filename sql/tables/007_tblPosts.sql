CREATE TABLE [dbo].[tblPosts] (

    [Id]            INT PRIMARY KEY,            

    [Title]			NVARCHAR(50) NOT NULL,

    [CategoryId]    INT DEFAULT(0) FOREIGN KEY(CategoryId) REFERENCES tblPostCategories(Id),

    [Slug]          NVARCHAR(255) NOT NULL DEFAULT(NEWID()),

    [Tags]          NVARCHAR(255),

	[Description]   NVARCHAR(255) NOT NULL,   

	[Content]		NVARCHAR(50) NOT NULL,

	[DateAdded]		DATETIME NOT NULL DEFAULT(GETDATE()),

	IsActive		BIT NOT NULL DEFAULT(0)
);



INSERT INTO [dbo].[tblPosts] ([Id], [Title], [CategoryId], [Slug], [Description], [Content], [DateAdded], [IsActive])
VALUES 
(0, 'Welcome Post', 0, NEWID(), 'This is the welcome post for the blog.', 'Welcome content goes here', GETDATE(), 1),
(1, 'Tech Trends 2024', 1, NEWID(), 'Discussing the latest in tech.', 'Tech article content', GETDATE(), 1),
(2, 'Healthy Living Tips', 2, NEWID(), 'How to live a healthy lifestyle.', 'Lifestyle content', GETDATE(), 1);
