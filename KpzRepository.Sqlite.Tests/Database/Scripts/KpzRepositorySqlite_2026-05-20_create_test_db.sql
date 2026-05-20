-- SQLite Test Database Creation Script
-- Note: SQLite automatically creates the database file when you connect to it
-- This script creates the tables and indexes needed for testing

-- Drop tables if they already exist
DROP TABLE IF EXISTS table_with_long_id;
DROP TABLE IF EXISTS table_with_string_id;

-- Table with INTEGER primary key (AUTOINCREMENT)
-- In SQLite, INTEGER PRIMARY KEY is automatically AUTOINCREMENT
CREATE TABLE table_with_long_id
(
	id INTEGER PRIMARY KEY AUTOINCREMENT,

	-- Basic fields
	name TEXT NOT NULL,
	description TEXT NULL,

	-- Numeric fields
	quantity INTEGER NOT NULL DEFAULT 0,
	price REAL NOT NULL DEFAULT 0.0,

	-- Boolean (stored as INTEGER: 0 = false, 1 = true)
	is_active INTEGER NOT NULL DEFAULT 1,

	-- Date/time fields (stored as TEXT in ISO 8601 format)
	created_at TEXT NOT NULL DEFAULT (datetime('now', 'utc')),
	updated_at TEXT NULL,

	-- UUID (stored as TEXT)
	external_id TEXT NOT NULL DEFAULT (lower(hex(randomblob(16)))),

	-- JSON / flexible data (stored as TEXT)
	metadata TEXT NULL
);

-- Create indexes for table_with_long_id
CREATE INDEX ix_table_with_long_id_name 
	ON table_with_long_id(name);

-- Table with string primary key
CREATE TABLE table_with_string_id
(
	id TEXT PRIMARY KEY,

	-- Basic fields
	title TEXT NOT NULL,
	notes TEXT NULL,

	-- Numeric fields
	amount REAL NULL,
	balance REAL NULL,

	-- Boolean
	is_deleted INTEGER NOT NULL DEFAULT 0,

	-- Date/time fields
	created_on TEXT NOT NULL DEFAULT (datetime('now', 'utc')),
	deleted_on TEXT NULL,

	-- Reference-like field
	related_long_id INTEGER NULL,

	-- JSON / flexible structure (stored as TEXT)
	attributes TEXT NULL,

	-- Indexed field
	category TEXT NULL
);

-- Create indexes for table_with_string_id
CREATE INDEX ix_table_with_string_id_category 
	ON table_with_string_id(category);

CREATE INDEX ix_table_with_string_id_related_long_id 
	ON table_with_string_id(related_long_id);