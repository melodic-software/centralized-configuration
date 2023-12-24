IF NOT EXISTS
(
    SELECT TOP (1)
           s.database_id
    FROM sys.databases s
    WHERE s.[name] = N'Configuration'
)
BEGIN
    CREATE DATABASE [Configuration];

    ALTER DATABASE [Configuration] SET RECOVERY SIMPLE;
END;
GO