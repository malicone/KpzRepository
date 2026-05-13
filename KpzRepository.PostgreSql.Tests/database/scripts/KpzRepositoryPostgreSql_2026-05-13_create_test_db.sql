-- PostgreSQL 18 Test Database Creation Script
-- Create database
-- Note: This must be run by a user with CREATEDB privilege
-- If the database already exists, you can skip this step or drop it first

-- Connect to default database (e.g., postgres) to create the test database
-- \c postgres

-- Drop database if it exists (optional, use with caution)
-- DROP DATABASE IF EXISTS kpz_repository_postgresql_test;

-- Create database
CREATE DATABASE kpz_repository_postgresql_test
	WITH 
	ENCODING = 'UTF8'
	LC_COLLATE = 'en_US.UTF-8'
	LC_CTYPE = 'en_US.UTF-8'
	TEMPLATE = template0;

-- Connect to the newly created database
\c kpz_repository_postgresql_test

-- Drop tables if they already exist
DROP TABLE IF EXISTS table_with_long_id CASCADE;
DROP TABLE IF EXISTS table_with_string_id CASCADE;
DROP TABLE IF EXISTS lookup_table CASCADE;
DROP TABLE IF EXISTS tracked_table CASCADE;

-- Table with BIGINT primary key (equivalent to SQL Server BIGINT IDENTITY)
CREATE TABLE table_with_long_id
(
	id BIGSERIAL PRIMARY KEY,

	-- Basic fields
	name VARCHAR(200) NOT NULL,
	description TEXT NULL,

	-- Numeric fields
	quantity INTEGER NOT NULL DEFAULT 0,
	price NUMERIC(18, 4) NOT NULL DEFAULT 0,

	-- Boolean
	is_active BOOLEAN NOT NULL DEFAULT true,

	-- Date/time fields (TIMESTAMP WITH TIME ZONE for UTC)
	created_at TIMESTAMP(3) WITH TIME ZONE NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC'),
	updated_at TIMESTAMP(3) WITH TIME ZONE NULL,

	-- UUID (equivalent to UNIQUEIDENTIFIER)
	external_id UUID NOT NULL DEFAULT gen_random_uuid(),

	-- JSON / flexible data (JSONB for better performance)
	metadata JSONB NULL

	-- Note: PostgreSQL doesn't have ROWVERSION; use updated_at with triggers or application logic
);

-- Create indexes for table_with_long_id
CREATE INDEX ix_table_with_long_id_name 
	ON table_with_long_id(name);

-- Table with string primary key
CREATE TABLE table_with_string_id
(
	id VARCHAR(64) PRIMARY KEY,

	-- Basic fields
	title VARCHAR(255) NOT NULL,
	notes TEXT NULL,

	-- Numeric fields
	amount DOUBLE PRECISION NULL,
	balance NUMERIC(18, 2) NULL,

	-- Boolean
	is_deleted BOOLEAN NOT NULL DEFAULT false,

	-- Date/time fields
	created_on TIMESTAMP(3) WITH TIME ZONE NOT NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC'),
	deleted_on TIMESTAMP(3) WITH TIME ZONE NULL,

	-- Reference-like field
	related_long_id BIGINT NULL,

	-- JSON / flexible structure
	attributes JSONB NULL,

	-- Indexed field
	category VARCHAR(100) NULL
);

-- Create indexes for table_with_string_id
CREATE INDEX ix_table_with_string_id_category 
	ON table_with_string_id(category);

CREATE INDEX ix_table_with_string_id_related_long_id 
	ON table_with_string_id(related_long_id);

-- Lookup table (equivalent to SQL Server LookupTable)
CREATE TABLE lookup_table
(
	-- Primary key (BIGINT)
	id BIGSERIAL PRIMARY KEY,

	-- Fields from LookupEntity
	name VARCHAR(200) NULL,
	code VARCHAR(100) NULL,
	description TEXT NULL,
	display_order BIGINT NULL,
	is_active BOOLEAN NULL DEFAULT true
);

-- Create indexes for lookup_table
-- Unique index with partial index (WHERE clause) for non-null codes
CREATE UNIQUE INDEX ux_lookup_table_code 
	ON lookup_table(code) 
	WHERE code IS NOT NULL;

CREATE INDEX ix_lookup_table_display_order 
	ON lookup_table(display_order);

CREATE INDEX ix_lookup_table_is_active 
	ON lookup_table(is_active);

-- Tracked table (equivalent to SQL Server TrackedTable)
CREATE TABLE tracked_table
(
	-- Primary key
	id BIGSERIAL PRIMARY KEY,

	-- Timestamps (using TIMESTAMP WITH TIME ZONE for proper timezone handling)
	created_at TIMESTAMP(7) WITH TIME ZONE NULL DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC'),
	updated_at TIMESTAMP(7) WITH TIME ZONE NULL,
	deleted_at TIMESTAMP(7) WITH TIME ZONE NULL,

	-- Audit users
	created_by VARCHAR(256) NULL,
	updated_by VARCHAR(256) NULL,
	deleted_by VARCHAR(256) NULL
);

-- Create indexes for tracked_table
-- Active records (not deleted)
CREATE INDEX ix_tracked_table_deleted_at 
	ON tracked_table(deleted_at);

-- Audit queries
CREATE INDEX ix_tracked_table_created_at 
	ON tracked_table(created_at);

CREATE INDEX ix_tracked_table_updated_at 
	ON tracked_table(updated_at);

-- Optional: Create comments on tables and columns for documentation
COMMENT ON TABLE table_with_long_id IS 'Test table with BIGINT primary key';
COMMENT ON TABLE table_with_string_id IS 'Test table with string primary key';
COMMENT ON TABLE lookup_table IS 'Lookup table for reference data';
COMMENT ON TABLE tracked_table IS 'Table with audit tracking fields';

-- Grant permissions (adjust as needed for your environment)
-- Example: GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO your_test_user;
-- Example: GRANT USAGE, SELECT ON ALL SEQUENCES IN SCHEMA public TO your_test_user;

-- Display created tables
SELECT 
	schemaname,
	tablename,
	tableowner
FROM pg_tables
WHERE schemaname = 'public'
ORDER BY tablename;
