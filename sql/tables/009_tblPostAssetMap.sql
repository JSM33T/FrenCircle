CREATE TABLE [dbo].[tblPostAssetMap] (

    [Id]            INT PRIMARY KEY,            

    PostId			INT NOT NULL FOREIGN KEY(PostId) REFERENCES		tblPosts(Id),    

	AssetId			INT NOT NULL FOREIGN KEY(AssetId) REFERENCES		tblAssets(Id),   

	DateAdded		NVARCHAR(50) NOT NULL,    
    
);