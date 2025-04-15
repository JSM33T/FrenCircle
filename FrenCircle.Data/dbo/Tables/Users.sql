CREATE TABLE Users
(
    Id                      INT                 PRIMARY KEY IDENTITY(1,1),
    FirstName               NVARCHAR(128)       NOT NULL,
    LastName                NVARCHAR(128)       NULL,
    UserName                NVARCHAR(128)       NOT NULL,
    Email                   NVARCHAR(256)       NOT NULL,
    RoleId                  INT                 NOT NULL FOREIGN KEY REFERENCES Roles(Id) DEFAULT(1),
    Avatar                  NVARCHAR(256)       NULL,
    Bio                     NVARCHAR(256)       NULL,
    Gender                  NVARCHAR(1)         NULL,
    IsEmailVerified         BIT                 NOT NULL DEFAULT 0,
    EmailVerificationToken  UNIQUEIDENTIFIER    NULL,
    CreatedAt               DATETIME            NOT NULL DEFAULT GETDATE(),
    UpdatedAt               DATETIME            NOT NULL DEFAULT GETDATE(),
    RowId                   UNIQUEIDENTIFIER    NOT NULL DEFAULT NEWID(),

);