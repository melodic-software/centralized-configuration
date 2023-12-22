IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = N'Configuration')
BEGIN
    CREATE DATABASE [Configuration];
	-- You can add additional configuration settings here if necessary
END;
GO