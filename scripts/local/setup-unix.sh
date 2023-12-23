#!/bin/bash

# Stop the script on the first error and treat unset variables as an error
set -eu
set -o pipefail

# Function to exit with a message
exit_with_message() {
    local message="$1"
    echo "$message"
    read -p "Press Enter to exit"
    exit 1
}

# Check if SQL Server tools are installed
if ! command -v sqlcmd &> /dev/null; then
    echo "Installing SQL Server command-line tools..."
    # TODO: Add commands to install SQL Server tools for Unix
    # This can vary based on the Unix distribution
    echo "SQL Server command-line tools installation completed."
fi

# Define database server
DB_SERVER="localhost"  # or ".\\SQLEXPRESS"

# Path to the database initialization script
DB_INIT_SCRIPT_PATH="../../data/init/CreateDatabase.sql"

# Create a connection string for Windows Authentication with TrustServerCertificate
# Note: Authentication might differ in Unix environments
CONNECTION_STRING="Server=${DB_SERVER};Database=master;Integrated Security=True;TrustServerCertificate=True;"

# Run the database initialization script with Windows Authentication
if ! sqlcmd -S "${DB_SERVER}" -d "master" -i "${DB_INIT_SCRIPT_PATH}"; then
    exit_with_message "An error occurred during database initialization"
fi

# Baseline the database (only needed if the schema is not empty)
# Uncomment the next line if you need to baseline
# flyway -configFiles="../../data/flyway/flyway.toml" -environment=local baseline

# Run Flyway migrations
if ! flyway -configFiles="../../data/flyway/flyway.toml" -environment=local migrate; then
    exit_with_message "An error occurred during Flyway migration"
fi

echo "Setup completed successfully. Press Enter to exit."
read -p "Press Enter to exit"