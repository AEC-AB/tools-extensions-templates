# RevitAppFramework

## Overview
RevitAppFramework is a robust framework for building WPF-based extensions for Autodesk Revit. It provides a structured, MVVM architecture with built-in support for executing commands within the Revit context, managing UI resources, and handling dependencies across multiple Revit versions (2019-2025).

## Features

- **Multi-version Support**: Compatible with Revit versions 2019 through 2025
- **MVVM Pattern Implementation**: Built on MVVMFluent for clean separation of concerns
- **CQRS Pattern**: Command and Query Responsibility Segregation for clear data operations
- **External Event Handler**: Safe execution of Revit operations from UI threads
- **Resource Management**: Global theme and resource handling
- **Dependency Injection**: Service registration and resolution
- **WPF UI Integration**: Modern UI components via WPF-UI library

## Architecture

### Core Components

- **RevitContext**: Wraps Revit's UIApplication for document and model access
- **ExternalEventExecutor**: Executes commands and queries within the Revit API context
- **AppExternalEventHandler**: Manages the queue of operations to be executed in Revit
- **ServiceFactory**: Creates and configures dependency injection services

### MVVM Components

- **RevitViewModelBase**: Base class for view models with Revit command execution
- **RevitCommandFluent**: Fluent API for defining and executing Revit operations
- **ResourceInjectionBehavior**: XAML behavior for applying global styles to UI elements

## Usage

### 1. Setup Project

Include the RevitAppFramework NuGet package in your project:

```xml
<PackageReference Include="RevitAppFramework" Version="0.0.1-alpha33">
  <PrivateAssets>all</PrivateAssets>
  <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
</PackageReference>
```

### 2. Initialize Services

Set up your service container and initialize the framework:

```csharp
var provider = ServiceFactory.Create(context.UIApplication, services =>
{
    services
        .RegisterAppServices() // Your custom services
        .AddSingleton(args);   // Command arguments
});
```

### 3. Create View Models

Derive view models from RevitViewModelBase to access Revit command execution:

```csharp
public class HomeViewModel(IExternalEventExecutor externalEventExecutor) 
    : RevitViewModelBase(externalEventExecutor)
{
    // Properties for data binding
    public string? DocumentTitle
    {
        get => Get<string?>();
        set => Set(value);
    }
    
    // Commands that execute in the Revit context
    public IAsyncFluentCommand GetDocumentTitleCommand =>
        Send<GetDocumentTitleQuery, string>()
        .Then(title => DocumentTitle = title);
}
```

### 4. Define Commands and Queries

Implement the CQRS pattern for Revit operations:

```csharp
public class GetDocumentTitleQuery : IQuery<string>
{
    // Query parameters if needed
}

public class GetDocumentTitleQueryHandler : IQueryHandler<GetDocumentTitleQuery, string>
{
    private readonly RevitContext _revitContext;
    
    public GetDocumentTitleQueryHandler(RevitContext revitContext)
    {
        _revitContext = revitContext;
    }
    
    public string Execute(GetDocumentTitleQuery query, CancellationToken cancellationToken)
    {
        return _revitContext.Document?.Title ?? "No document open";
    }
}
```

### 5. Show the Main Window

Launch your application window:

```csharp
WindowHandler.ShowWindow<MainWindow>(provider, IntPtr.Zero);
```

## Resource Management

Apply global styles and resources to your XAML:

```xml
<UserControl
    resources:ResourceInjectionBehavior.InjectResources="True"
    xmlns:resources="clr-namespace:RevitAppFramework.Resources">
    <!-- Your UI elements -->
</UserControl>
```

## Versioning Support

RevitAppFramework supports multiple Revit versions by using conditional compilation:

- Set the build configuration to target a specific Revit version (e.g., "Debug 2022")
- Use conditional symbols for version-specific code (e.g., `#if R2022`)
- Reference version-specific Revit API assemblies automatically

## Dependencies

- WPF-UI 3.0.5+
- MVVMFluent 0.0.2
- Microsoft.Extensions.DependencyInjection

## License

MIT License

---

For more information, visit [Tools by AEC Wiki](https://wiki.toolsbyaec.com)
