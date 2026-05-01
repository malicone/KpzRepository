-- Create database
IF DB_ID(N'KpzRepositorySqlServerTest') IS NULL
BEGIN
    CREATE DATABASE KpzRepositorySqlServerTest;
END
GO

USE KpzRepositorySqlServerTest;
GO

-- Drop tables if they already exist
IF OBJECT_ID(N'dbo.TableWithLongId', N'U') IS NOT NULL
    DROP TABLE dbo.TableWithLongId;

-- Table with BIGINT primary key
CREATE TABLE dbo.TableWithLongId
(
    Id BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,

    -- Basic fields
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NULL,

    -- Numeric fields
    Quantity INT NOT NULL CONSTRAINT DF_TableWithLongId_Quantity DEFAULT 0,
    Price DECIMAL(18, 4) NOT NULL CONSTRAINT DF_TableWithLongId_Price DEFAULT 0,

    -- Boolean
    IsActive BIT NOT NULL CONSTRAINT DF_TableWithLongId_IsActive DEFAULT 1,

    -- Date/time fields
    CreatedAt DATETIME2(3) NOT NULL CONSTRAINT DF_TableWithLongId_CreatedAt DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2(3) NULL,

    -- GUID
    ExternalId UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_TableWithLongId_ExternalId DEFAULT NEWID(),

    -- JSON / flexible data
    Metadata NVARCHAR(MAX) NULL,

    -- RowVersion
    RowVersion ROWVERSION
);
GO


IF OBJECT_ID(N'dbo.TableWithStringId', N'U') IS NOT NULL
    DROP TABLE dbo.TableWithStringId;
GO

-- Table with string primary key
CREATE TABLE dbo.TableWithStringId
(
    Id NVARCHAR(64) NOT NULL PRIMARY KEY,

    -- Basic fields
    Title NVARCHAR(255) NOT NULL,
    Notes NVARCHAR(MAX) NULL,

    -- Numeric fields
    Amount FLOAT NULL,
    Balance DECIMAL(18, 2) NULL,

    -- Boolean
    IsDeleted BIT NOT NULL CONSTRAINT DF_TableWithStringId_IsDeleted DEFAULT 0,

    -- Date/time fields
    CreatedOn DATETIME2(3) NOT NULL CONSTRAINT DF_TableWithStringId_CreatedOn DEFAULT SYSUTCDATETIME(),
    DeletedOn DATETIME2(3) NULL,

    -- Reference-like field
    RelatedLongId BIGINT NULL,

    -- JSON / flexible structure
    Attributes NVARCHAR(MAX) NULL,

    -- Indexed field
    Category NVARCHAR(100) NULL
);
GO

-- Indexes
CREATE INDEX IX_TableWithLongId_Name
    ON dbo.TableWithLongId(Name);

CREATE INDEX IX_TableWithStringId_Category
    ON dbo.TableWithStringId(Category);

CREATE INDEX IX_TableWithStringId_RelatedLongId
    ON dbo.TableWithStringId(RelatedLongId);
GO

IF OBJECT_ID(N'dbo.LookupTable', N'U') IS NOT NULL
    DROP TABLE dbo.LookupTable;
GO

CREATE TABLE dbo.LookupTable
(
    -- Primary key (BIGINT)
    Id BIGINT IDENTITY(1,1) NOT NULL
        CONSTRAINT PK_LookupTable PRIMARY KEY,

    -- Fields from LookupEntity
    Name NVARCHAR(200) NULL,
    Code NVARCHAR(100) NULL,
    Description NVARCHAR(MAX) NULL,
    DisplayOrder BIGINT NULL,
    IsActive BIT NULL
        CONSTRAINT DF_LookupTable_IsActive DEFAULT (1)
);
GO

-- Optional indexes (typical for lookup tables)
CREATE UNIQUE INDEX UX_LookupTable_Code
    ON dbo.LookupTable(Code)
    WHERE Code IS NOT NULL;

CREATE INDEX IX_LookupTable_DisplayOrder
    ON dbo.LookupTable(DisplayOrder);

CREATE INDEX IX_LookupTable_IsActive
    ON dbo.LookupTable(IsActive);
GO

IF OBJECT_ID(N'dbo.TrackedTable', N'U') IS NOT NULL
    DROP TABLE dbo.TrackedTable;
GO

CREATE TABLE dbo.TrackedTable
(
    -- Primary key
    Id BIGINT IDENTITY(1,1) NOT NULL
        CONSTRAINT PK_TrackedTable PRIMARY KEY,

    -- Timestamps
    CreatedAt DATETIMEOFFSET(7) NULL
        CONSTRAINT DF_TrackedTable_CreatedAt DEFAULT SYSDATETIMEOFFSET(),

    UpdatedAt DATETIMEOFFSET(7) NULL,
    DeletedAt DATETIMEOFFSET(7) NULL,

    -- Audit users
    CreatedBy NVARCHAR(256) NULL,
    UpdatedBy NVARCHAR(256) NULL,
    DeletedBy NVARCHAR(256) NULL,
);
GO

-- Optional indexes (useful in real systems)

-- Active records (not deleted)
CREATE INDEX IX_TrackedTable_DeletedAt
    ON dbo.TrackedTable(DeletedAt);

-- Audit queries
CREATE INDEX IX_TrackedTable_CreatedAt
    ON dbo.TrackedTable(CreatedAt);

CREATE INDEX IX_TrackedTable_UpdatedAt
    ON dbo.TrackedTable(UpdatedAt);
GO