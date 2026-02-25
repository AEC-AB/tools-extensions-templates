# RevitAppExtension

A template for creating Revit add-ins that integrate with Assistant, providing a foundation for building MVVM-based Revit applications with a modern UI using WPF-UI.

## Features

- Modern UI with WPF-UI library (Windows 11 design language)
- MVVM architecture with clean separation of concerns
- Command Query Responsibility Segregation (CQRS) pattern for Revit operations
- Fluent command API for elegant handling of user actions
- Built-in dependency injection
- Support for multiple Revit versions (2019-2025)
- Azure Pipelines configuration for automated builds
- Integration with Assistant

## Getting Started

1. Create a new project using this template
2. Update the project name and identifiers as needed
3. Build the solution for your target Revit version
4. Run the application from Revit


## Prerequisites

- Visual Studio Code 
- .NET Framework 4.8 for Revit 2019-2024, .NET 8 for Revit 2025
- Revit (any version from 2019 to 2025)

## Project Structure

The template follows the MVVM pattern with these key components:
- `CQRS/` - Commands and queries for Revit operations
- `ViewModels/` - View models that drive the UI behavior
- `Views/` - XAML views for the UI
- `Resources/` - Shared resources
- Root classes for application entry point and configuration


## Documentation

For detailed information about the project architecture, design patterns, and implementation details, see the [Developer Guide](docs/DeveloperGuide.md).

## AI-Assisted Development

This template includes instruction files in `.github/instructions/` for AI assistants like GitHub Copilot:

- `ui-common.instructions.md` for shared UI/framework guidance
- `platform.instructions.md` for Revit-specific guidance

These instructions help ground the AI with specific knowledge about:

- Assistant Extension development patterns
- Revit API best practices
- UI control configuration
- Common Revit operations
- Extension-specific implementation details

When using GitHub Copilot or other AI assistants, these instructions help generate more accurate and contextually relevant code suggestions.

## Sample Usage

The template includes sample features demonstrating:
- Document information retrieval
- Element comment management
- Element deletion
- Navigation between screens
- Progress indicators for long-running operations
- Initial user input handling

## Building and Deployment

Select the appropriate configuration for your target Revit version:
- Release 2019
- Release 2020
- Release 2021
- Release 2022
- Release 2023
- Release 2024
- Release 2025

The Azure Pipelines configuration automatically builds and packages the extension for all configured Revit versions.