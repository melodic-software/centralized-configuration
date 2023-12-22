# PowerShell script for setting up the local development environment on Windows

# Define database server - adjust as needed
$DbServer = "localhost"  # or ".\SQLEXPRESS" if using SQL Server Express

# Path to the database initialization script
$DbInitScriptPath = "..\data\init\CreateDatabase.sql"

# Run the database initialization script with Windows Authentication
Invoke-Sqlcmd -ServerInstance $DbServer -IntegratedSecurity -InputFile $DbInitScriptPath

# Run Flyway migrations
# Ensure that the Flyway CLI is installed and configured correctly
flyway migrate