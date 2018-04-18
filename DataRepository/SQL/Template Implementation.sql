---------------------------------------------------------------------------------------------------
-- SEARCHING KEY
---------------------------------------------------------------------------------------------------
-- LOGINS				[#L]
-- DATABASE CONTEXT		[#C]
-- SCHEMA				[#S]
-- TABLES				[#T]
-- TRIGGERS				[#TR]
-- VIEWS				[#V]
-- STORED PROCEDURES	[#SP]
-- FUNCTIONS			[#F]
-- USERS/ROLES			[#UR]
-- PERMISSIONS			[#P]
-- METADATA				[#M]

---------------------------------------------------------------------------------------------------
-- LOGINS				[#L]
---------------------------------------------------------------------------------------------------
USE [master]
GO
PRINT N'Creating login for [ad_user] on [master]...';
GO

---------------------------------------------------------------------------------------------------
-- DATABASE CONTEXT		[#C]
---------------------------------------------------------------------------------------------------
USE [AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------------------------------------
-- SCHEMA				[#S]
---------------------------------------------------------------------------------------------------
PRINT N'Creating schema [dmd] on [DistributedMediaDelivery]...';
GO

---------------------------------------------------------------------------------------------------
-- TABLES				[#T]
---------------------------------------------------------------------------------------------------
PRINT N'Creating table [schema].[table_name]...';
GO

PRINT N'Dropping table [schema].[table_name]...';
GO

PRINT N'Altering table [schema].[table_name] by adding column [column_name]...';
GO

PRINT N'Altering table [schema].[table_name] by removing column [column_name]...';
GO

PRINT N'Restoring table [schema].[table_name] by re-adding column [column_name]...';
GO

PRINT N'Restoring table [schema].[table_name] by removing column [column_name]...';
GO

PRINT N'Altering table [schema].[table_name] by altering column [column_name]...';
GO

PRINT N'Restoring table [schema].[table_name] by altering column [column_name]...';
GO

PRINT N'Altering table [schema].[table_name] by adding [constraint_type] constraint [constraint_name]...';
GO

PRINT N'Altering table [schema].[table_name] by removing [constraint_type] constraint [constraint_name]...';
GO

PRINT N'Altering table [schema].[table_name] by adding [key_type] key [key_name]...';
GO

PRINT N'Altering table [schema].[table_name] by removing [key_type] key [key_name]...';
GO

---------------------------------------------------------------------------------------------------
-- TRIGGERS				[#TR]
---------------------------------------------------------------------------------------------------
PRINT N'Creating [trigger_type] trigger [schema].[trigger_name] on table [schema].[table_name]...';
GO

PRINT N'Dropping [trigger_type] trigger [schema].[trigger_name] from table [schema].[table_name]...';
GO

PRINT N'Disabling [trigger_type] trigger [schema].[trigger_name] on table [schema].[table_name]...';
GO

PRINT N'Re-enabling [trigger_type] trigger [schema].[trigger_name] on table [schema].[table_name]...';
GO

---------------------------------------------------------------------------------------------------
-- VIEWS				[#V]
---------------------------------------------------------------------------------------------------
PRINT N'Creating view [schema].[view_name]...';
GO

PRINT N'Restoring view [schema].[view_name]...';
GO

PRINT N'Dropping view [schema].[view_name]...';
GO

PRINT N'Altering view [schema].[view_name] by adding column [column_name]...';
GO

PRINT N'Altering view [schema].[view_name] by removing column [column_name]...';
GO

---------------------------------------------------------------------------------------------------
-- STORED PROCEDURES	[#SP]
---------------------------------------------------------------------------------------------------
PRINT N'Creating sproc [schema].[sproc_name]...';
GO

PRINT N'Dropping sproc [schema].[sproc_name]...';
GO

PRINT N'Altering sproc [schema].[sproc_name]...';
GO

PRINT N'Restoring sproc [schema].[sproc_name]...';
GO

---------------------------------------------------------------------------------------------------
-- FUNCTIONS			[#F]
---------------------------------------------------------------------------------------------------
PRINT N'Creating function [schema].[function_name]...';
GO

PRINT N'Dropping function [schema].[function_name]...';
GO

PRINT N'Altering function [schema].[function_name]...';
GO

PRINT N'Restoring function [schema].[function_name]...';
GO

---------------------------------------------------------------------------------------------------
-- USERS/ROLES			[#UR]
---------------------------------------------------------------------------------------------------
PRINT N'Creating login for [ad_user] on [database_name]...';
GO

PRINT N'Creating role [role_name] on [database_name]...';
GO

PRINT N'Creating user [sql_user] on [database_name]...';
GO

PRINT N'Adding user [sql_user] to the role [role_name]...';
GO

---------------------------------------------------------------------------------------------------
-- PERMISSIONS			[#P]
---------------------------------------------------------------------------------------------------
PRINT N'Granting [sql_action] on [sql_object] [schema].[object_name] for role [role_name]...';
GO

---------------------------------------------------------------------------------------------------
-- METADATA				[#M]
---------------------------------------------------------------------------------------------------
PRINT N'Inserting [description] metadata into table [schema].[table_name]...';
GO

PRINT N'Deleting [description] metadata from table [schema].[table_name]...';
GO

PRINT N'Removing [description] metadata from table [schema].[table_name]...';
GO

PRINT N'Updating [description] metadata in table [schema].[table_name]...';
GO