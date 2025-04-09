CREATE TABLE JobHistory (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    JobName NVARCHAR(100) NOT NULL,
    [Status] NVARCHAR(20) NOT NULL,
    ScheduledAt DATETIME NOT NULL,
    StartedAt DATETIME NULL,
    CompletedAt DATETIME NULL,
    DurationMs INT NULL,
    Error NVARCHAR(MAX) NULL,
    TriggeredBy NVARCHAR(100) NULL,
    Metadata NVARCHAR(MAX) NULL -- for JSON or extra data
);
