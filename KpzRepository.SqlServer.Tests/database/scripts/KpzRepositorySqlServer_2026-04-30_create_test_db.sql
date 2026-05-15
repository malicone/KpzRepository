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