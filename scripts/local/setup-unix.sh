#!/bin/bash

# Description: This script initializes a SQL Server database and runs migrations using Flyway.
# It checks for the presence of SQL Server tools, sets up the database, and performs migrations.

# Stop the script on the first error and treat unset variables as an error
set -eu
set -o pipefail

# Function to exit with a message
# Arguments:
#   1. Error message to display
exit_with_message() {
    local message="$1"
    echo "$message"
    read -p "Press Enter to exit"
    exit 1
}

# Check if SQL Server tools are installed
# TODO: Add commands to install SQL Server tools for Unix environments
if ! command -v sqlcmd &> /dev/null; then
    echo "Installing SQL Server command-line tools..."
    # This can vary based on the Unix distribution
    echo "SQL Server command-line tools installation completed."
fi

# TODO: Consider passing these as arguments or reading from a config file
DB_SERVER="localhost"  # or ".\\SQLEXPRESS"
DB_INIT_SCRIPT_PATH="../../data/init/CreateDatabase.sql"

# Create a connection string for Windows Authentication with TrustServerCertificate
# Note: Authentication might differ in Unix environments
CONNECTION_STRING="Server=${DB_SERVER};Database=master;Integrated Security=True;TrustServerCertificate=True;"

# Run the database initialization script with Windows Authentication
if ! sqlcmd -S "${DB_SERVER}" -d "master" -i "${DB_INIT_SCRIPT_PATH}"; then
    exit_with_message "Database initialization failed. Please check the script and SQL Server setup."
fi

# Baseline the database if needed
# Uncomment the next line to baseline the database
# flyway -configFiles="../../data/flyway/flyway.toml" -environment=local baseline

# Run Flyway migrations
if ! flyway -configFiles="../../data/flyway/flyway.toml" -environment=local migrate; then
    exit_with_message "Flyway migration failed. Please check the migration scripts and configurations."
fi

echo "Setup completed successfully. Press Enter to exit."
read -p "Press Enter to exit"