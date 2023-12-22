# PowerShell script for setting up the local development environment on Windows

# Stop the script on the first error
$ErrorActionPreference = "Stop"

# Require PowerShell Core
$MinPowerShellVersion = "7.0"
if ($PSVersionTable.PSVersion.Major -lt $MinPowerShellVersion) {
    Write-Error "This script requires PowerShell Core version $MinPowerShellVersion or higher."
    Read-Host -Prompt "Press Enter to exit"
    exit 1
}

# Stop the script on the first error
$ErrorActionPreference = "Stop"

# Check if the SQL Server module is installed and install it if not
if (-not (Get-Module -ListAvailable -Name SqlServer)) {
    Write-Host "Installing SQL Server PowerShell module..."
    Install-Module -Name SqlServer -Scope CurrentUser -Force -AllowClobber
	Write-Host "SQL Server PowerShell module install completed."
}

# Import SQL Server module
Import-Module -Name SqlServer

# Define database server
$DbServer = "localhost"  # or ".\SQLEXPRESS"

# Path to the database initialization script
$DbInitScriptPath = "..\data\init\CreateDatabase.sql"

try {
    # Run the database initialization script with Windows Authentication
    Invoke-Sqlcmd -ServerInstance $DbServer -IntegratedSecurity -InputFile $DbInitScriptPath
} catch {
    Write-Error "An error occurred: $_"
    Read-Host -Prompt "Press Enter to exit"
    exit 1
}

try {
    # Run Flyway migrations
    flyway migrate
} catch {
    Write-Error "An error occurred during Flyway migration: $_"
    Read-Host -Prompt "Press Enter to exit"
    exit 1
}

Write-Host "Setup completed successfully. Press Enter to exit."
Read-Host -Prompt "Press Enter to exit"