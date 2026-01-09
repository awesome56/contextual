# Contextual - Student Management System

[![.NET Build](https://github.com/awesome56/contextual/actions/workflows/build.yml/badge.svg)](https://github.com/awesome56/contextual/actions/workflows/build.yml)

A Windows Forms application for managing student records, courses, and academic results.

## Features

- ?? Student management
- ?? Course management
- ?? Program administration
- ?? Excel data import
- ?? Result processing and reporting

## Technology Stack

- .NET 6 (Windows Forms)
- SQL Server LocalDB
- ExcelDataReader
- ClosedXML
- FontAwesome.Sharp
- Bunifu UI

## Getting Started

### Prerequisites

- Visual Studio 2022
- .NET 6 SDK
- SQL Server LocalDB

### Installation

1. Clone the repository
   ```bash
   git clone https://github.com/awesome56/contextual.git
   ```

2. Open the solution in Visual Studio 2022
   ```bash
   cd Contextual
   start Contextual.sln
   ```

3. Restore NuGet packages
   ```bash
   dotnet restore
   ```

4. Build the solution
   ```bash
   dotnet build
   ```

5. Run the application

### Database Setup

The application uses SQL Server LocalDB. Ensure LocalDB is installed with Visual Studio or install it separately.

## Project Structure

```
Contextual/
??? .github/
?   ??? workflows/
?       ??? build.yml       # CI build workflow
?       ??? release.yml     # Release workflow
??? Contextual/
?   ??? Forms/              # Windows Forms
?   ??? Globals.cs          # Global variables
?   ??? Contextual.csproj   # Project file
??? .gitignore
??? README.md
```

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## Creating a Release

To create a new release:

```bash
git tag -a v3.3.4 -m "Release version 3.3.4"
git push origin v3.3.4
```

This will trigger the release workflow and create a GitHub release with the built application.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Version

Current Version: 3.3.3.0
