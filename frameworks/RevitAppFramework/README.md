# CW.RevitAppFramework

## Overview
`CW.RevitAppFramework` is a framework for building WPF-based extensions for Autodesk Revit. It provides a structured MVVM architecture with built-in support for executing commands in Revit context, managing UI resources, and handling dependencies across Revit versions.

## Features

- Multi-version support: compatible with Revit 2019 through 2026
- MVVM pattern implementation: built on MVVMFluent.WPF
- CQRS pattern: command and query segregation for clear operations
- External event handler: safe execution of Revit operations from UI threads
- Resource management: shared theme and resource handling
- Dependency injection: service registration and resolution
- WPF UI integration: modern UI components via WPF-UI

## Architecture

### Core Components

- `RevitContext`: wraps Revit's `UIApplication` for document/model access
- `ExternalEventExecutor`: executes commands and queries in Revit API context
- `AppExternalEventHandler`: manages queued operations executed in Revit
- `ServiceFactory`: creates and configures dependency injection services

### MVVM Components

- `RevitViewModelBase`: base class for view models with Revit command execution
- `RevitCommandFluent`: fluent API for defining and executing Revit operations
- `ResourceInjectionBehavior`: XAML behavior for applying global styles

## Installation

Add the package that matches your Revit version:

```xml
<PackageReference Include="CW.RevitAppFramework.<RevitVersion>" Version="26.*">
  <PrivateAssets>all</PrivateAssets>
  <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
</PackageReference>
```

Example:

```xml
<PackageReference Include="CW.RevitAppFramework.2026" Version="1.*" />
```

## Usage

### 1. Initialize Services

Set up your service container and initialize framework services:

```csharp
var provider = ServiceFactory.Create(context.UIApplication, services =>
{
	services
		.RegisterAppServices()
		.AddSingleton(args);
});
```

### 2. Create View Models

Derive view models from `RevitViewModelBase` to access Revit command execution:

```csharp
public class HomeViewModel(IExternalEventExecutor externalEventExecutor)
	: RevitViewModelBase(externalEventExecutor)
{
	public string? DocumentTitle
	{
		get => Get<string?>();
		set => Set(value);
	}

	public IAsyncFluentCommand GetDocumentTitleCommand =>
		Send<GetDocumentTitleQuery, string>()
		.Then(title => DocumentTitle = title);
}
```

### 3. Define Commands and Queries

Implement CQRS handlers for Revit operations:

```csharp
public class GetDocumentTitleQuery : IQuery<string>
{
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

### 4. Show the Main Window

Launch your application window:

```csharp
WindowHandler.ShowWindow<MainWindow>(provider, IntPtr.Zero);
```

## Resource Management

Apply global styles/resources in XAML:

```xml
<UserControl
	resources:ResourceInjectionBehavior.InjectResources="True"
	xmlns:resources="clr-namespace:RevitAppFramework.Resources">
	<!-- Your UI elements -->
</UserControl>
```

## Versioning Support

- Set build configuration per Revit version (for example `Debug 2022`)
- Use conditional symbols for version-specific code (for example `#if R2022`)
- Reference version-specific Revit API assemblies through package configuration

## Dependencies

- WPF-UI
- MVVMFluent.WPF
- Microsoft.Extensions.DependencyInjection

## License

MIT License

For more information, visit [Tools by AEC Wiki](https://toolswiki.aec.se)
