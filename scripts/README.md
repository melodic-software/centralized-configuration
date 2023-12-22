# Scripts Directory

This directory contains various scripts used for setting up and managing the development environment for the Centralized Configuration project.

## Local Setup Scripts

These scripts are intended for setting up the local development environment. They handle tasks like creating the necessary database and applying database migrations.

### Windows Setup

- **Script**: `local/setup-windows.ps1`
- **Purpose**: This script sets up the local development environment on Windows machines. It is designed to be run using PowerShell and utilizes Windows Authentication for connecting to SQL Server.
- **Requirements**:
  - PowerShell Core (or Windows PowerShell) should be installed on your system.
  - The execution policy in PowerShell needs to be adjusted to allow script execution, which can be done by setting it to `RemoteSigned`.
- **Execution Policy Setting**:
  - Open PowerShell as an Administrator.
  - Run the following command to allow script execution:
    ```powershell
    Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
    ```
  - This command allows the current user to run scripts that are signed or from the local machine.
- **How to Run**:
  1. Open PowerShell (not as an Administrator).
  2. Navigate to the `scripts/local/` directory in your repository.
  3. Execute the script:
     ```powershell
     ./setup-windows.ps1
     ```

### Unix-like Systems Setup

- **Script**: `local/setup-unix.sh`
- **Purpose**: This script is for setting up the local development environment on Unix-like systems, including Linux and macOS. It is tailored to connect to SQL Server using SQL Server authentication.
- **Requirements**:
  - A Unix-like operating system with a standard shell environment.
  - SQL Server command-line tools installed (like `sqlcmd` for SQL Server interactions).
- **How to Run**:
  1. Open a terminal window.
  2. Navigate to the `scripts/local/` directory in your repository.
  3. Make the script executable (only needed once):
     ```bash
     chmod +x setup-unix.sh
     ```
  4. Execute the script:
     ```bash
     ./setup-unix.sh
     ```

## Additional Scripts

Add documentation here IF/WHEN other scripts are added. Indicate if they are to be run locally or if they are only used by the pipeline.