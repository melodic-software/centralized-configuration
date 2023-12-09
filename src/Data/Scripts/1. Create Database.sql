USE [master];
GO

IF NOT EXISTS
(
    SELECT TOP (1)
           d.database_id
    FROM sys.databases d
    WHERE d.[name] = 'WachterConfiguration'
)
BEGIN
    CREATE DATABASE WachterConfiguration;
END;
GO