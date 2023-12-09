MIGRATION SCRIPT TEMPLATES

POWERSHELL SCRIPTS (PACKAGE MANAGER CONSOLE)

	Get-Help Add-Migration
	
	Add-Migration -Name InitialMigration -Context ConfigurationContext -Project Configuration.EntityFramework.Sqlite -StartupProject Configuration.API -verbose
	Update-Database -Context ConfigurationContext -Project Configuration.EntityFramework.Sqlite -StartupProject Configuration.API -verbose
	Script-Migration -Idempotent

DOT NET CORE CLI

	dotnet ef database update
	dotnet ef migrations script --idempotent