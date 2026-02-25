/// <summary>
/// Global using directives for the RevitAppExtension project.
/// This file simplifies imports across the project by making these namespaces available globally.
/// </summary>
/// <remarks>
/// The global usings are organized into logical sections:
/// 1. Standard .NET namespaces
/// 2. Assistant Extension Core namespaces
/// 3. Revit-specific namespaces
/// 4. UI and MVVM frameworks
/// 
/// When extending the project with new dependencies, consider whether they should be
/// added here for global availability or kept as local using statements in specific files.
/// </remarks>

// Standard .NET framework namespaces
global using System;
global using System.Linq;
global using System.ComponentModel;
global using System.ComponentModel.DataAnnotations;
global using System.Threading;
global using System.Threading.Tasks;
global using System.Collections.Generic;

// Type aliases
global using Result = CW.Assistant.Extensions.Contracts.Result;
global using Visibility = System.Windows.Visibility;

// RevitAppFramework core namespaces - the foundation of the Revit application framework
global using RevitAppFramework;
global using RevitAppFramework.CQRS;
global using RevitAppFramework.Mvvm;

// Assistant Extension core contract namespaces - for interfacing with the Assistant platform
global using CW.Assistant.Extensions.Contracts;
global using CW.Assistant.Extensions.Contracts.Enums;
global using CW.Assistant.Extensions.Contracts.Collectors;
global using CW.Assistant.Extensions.Contracts.Attributes;
global using CW.Assistant.Extensions.Contracts.Fields;

// Revit-specific Assistant Extension namespaces - for Revit-specific functionality
global using CW.Assistant.Extensions.Revit;
global using CW.Assistant.Extensions.Revit.Attributes;
global using CW.Assistant.Extensions.Revit.Collectors;

// Autodesk Revit API namespaces - core Revit functionality
global using Autodesk.Revit.DB;
global using Autodesk.Revit.UI;

// UI and MVVM framework namespaces - for the application's user interface
global using MVVMFluent;
global using Wpf.Ui;
global using Wpf.Ui.Extensions;