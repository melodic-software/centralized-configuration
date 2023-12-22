#!/bin/bash
# Shell script for setting up the local development environment on Unix-like systems

# Define database server and credentials - update these variables
db_server="localhost"
db_user="yourUsername"
db_password="yourPassword"

# Path to the database initialization script
db_init_script_path="../data/init/CreateDatabase.sql"

# Run the database initialization script using SQL Server authentication
sqlcmd -S "$db_server" -U "$db_user" -P "$db_password" -i "$db_init_script_path"

# Run Flyway migrations
# Ensure that the Flyway CLI is installed and configured correctly
flyway migrate