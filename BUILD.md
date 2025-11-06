# Building and Running Digishop

This document provides instructions for compiling and running the Digishop application.

## Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later

## Build Instructions

1. **Clone the repository** (if not already done):
   ```bash
   git clone https://github.com/5enox/Digishop.git
   cd Digishop
   ```

2. **Restore NuGet packages**:
   ```bash
   dotnet restore
   ```

3. **Build the solution**:
   ```bash
   dotnet build
   ```

## Running the Application

1. **Navigate to the web application directory**:
   ```bash
   cd src/Digiseller.Engine.Core
   ```

2. **Run the application**:
   ```bash
   dotnet run
   ```

3. **Access the application**:
   - Open your web browser and navigate to: `http://localhost:5000`
   - You will be redirected to the installation page: `http://localhost:5000/Dashboard/Home/Install`

## Initial Setup

On first run, you'll need to complete the installation:

1. **Administrator Settings**:
   - Enter an admin login
   - Enter and confirm an admin password

2. **Digiseller Settings**:
   - Enter your Digiseller Seller ID
   - Enter your Digiseller UID

3. Click **Save** to complete the installation

## Build Output

- Build artifacts are located in: `src/Digiseller.Engine.Core/bin/Debug/net9.0/`
- The main executable is: `Digiseller.Engine.Core.dll`

## Troubleshooting

- If you encounter any build errors, ensure you have .NET 9.0 SDK installed
- Run `dotnet --version` to verify your .NET version
- Make sure all NuGet packages are restored with `dotnet restore`

## Project Structure

- `src/Digiseller.Client.Core/` - Core client library for Digiseller API
- `src/Digiseller.Engine.Core/` - Main web application (ASP.NET Core MVC)

## Notes

- The application has been upgraded from .NET Core 1.1 to .NET 9.0
- Some package versions were updated for security and compatibility
- The application uses ASP.NET Core MVC with Razor views
