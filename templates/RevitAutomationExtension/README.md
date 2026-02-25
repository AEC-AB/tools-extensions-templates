# RevitAutomationExtension Developer Guide

## Introduction

The RevitAutomationExtension is a framework for developing extensions that automate tasks within Autodesk Revit. This guide provides a comprehensive overview of how to develop, customize, and deploy Revit extensions using this template.

## Core Components

A Revit extension consists of three key components:

1. **Args Class**: Defines input parameters and UI controls
2. **Command Class**: Implements the extension logic (IRevitExtension interface)
3. **Result Class**: Standardizes the output format

## Implementation Pattern

### 1. Define the Args Class

The Args class defines the input parameters and UI controls for your extension:

```csharp
public class RevitAutomationExtensionArgs
{
    [Description("Parameter Name")]
    [ControlData(ToolTip = "Enter parameter description")]
    public string ParameterName { get; set; } = "Default value";
    
    // Add ValueCopy functionality when needed
    [ValueCopyCollector(typeof(ValueCopyRevitCollector))]
    public ValueCopy ValueCopy { get; set; }
}
```

### 2. Implement the Command Class

The Command class contains the core logic for your extension:

```csharp
public class RevitAutomationExtensionCommand : IRevitExtension<RevitAutomationExtensionArgs>
{
    public IExtensionResult Run(IRevitExtensionContext context, RevitAutomationExtensionArgs args, 
        CancellationToken cancellationToken)
    {
        var document = context.UIApplication.ActiveUIDocument?.Document;
        if (document is null)
            return Result.Text.Failed("Revit has no active model open");
            
        // Basic implementation pattern:
        // 1. Access Revit document and selected elements
        // 2. Start a transaction
        // 3. Perform model modifications
        // 4. Commit the transaction
        // 5. Return a success/failure result
        
        using var transaction = new Transaction(document, "RevitExtension");
        transaction.Start();
        
        // Extension-specific logic here
        
        transaction.Commit();
        return Result.Text.Succeeded("Operation completed successfully");
    }
}
```

### 3. Return a Result

All extensions should return an `IExtensionResult` to indicate success or failure:

```csharp
// Success result
return Result.Text.Succeeded("Operation completed successfully");

// Failure result
return Result.Text.Failed("Error message explaining what went wrong");
```

## Working with Revit Elements

### Accessing the Document

```csharp
var document = context.UIApplication.ActiveUIDocument?.Document;
if (document is null)
    return Result.Text.Failed("Revit has no active model open");
```

### Getting Selected Elements

```csharp
var selectedIds = context.UIApplication.ActiveUIDocument.Selection.GetElementIds();
if (!selectedIds.Any())
    return Result.Text.Failed("No elements selected");
```

### Element Collection

```csharp
var collector = new FilteredElementCollector(document);
var walls = collector.OfCategory(BuiltInCategory.OST_Walls).WhereElementIsNotElementType();
```

### Transactions

Always use transactions for model modifications:

```csharp
using var transaction = new Transaction(document, "Description");
transaction.Start();
// Modify elements
transaction.Commit();
```

### Parameter Access

```csharp
var parameter = element.LookupParameter("ParameterName");
var value = parameter.AsString(); // or AsDouble(), AsInteger(), etc.
parameter.Set(newValue);
```

## UI Controls and Attributes

### Basic Control Attributes

- `[Description("Label")]`: Sets field label
- `[ControlData(ToolTip = "Help text")]`: Adds tooltip
- `[Required]`: Makes input mandatory
- `[DefaultValue("Default")]`: Sets default value
- `[ControlSettings("PropertyName", "Value")]`: Configure control properties

### Control Types

- `[ControlType(ControlType.ComboBox)]`: Dropdown selection
- `[ControlType(ControlType.ListBox)]`: Multi-selection list
- `[ControlType(ControlType.Option)]`: Single-option selection
- `[ControlType(ControlType.RadioButton)]`: Radio button group
- `[ControlType(ControlType.Browse)]`: File browser dialog
- `[ControlType(ControlType.Save)]`: File save dialog

### Text Input Customization

- `[ControlSettings("IsMultiline", "True")]`: Enable multi-line text input
- `[ControlSettings("MinLines", "5")]`: Set minimum lines for text area
- `[ControlSettings("MaxLines", "10")]`: Set maximum lines for text area
- `[ControlSettings("Foreground", "Red")]`: Change text color

### Auto-Fill Sources

- `[CustomRevitAutoFill(typeof(CustomCollectorClass))]`: Custom Revit data collector
- `[RevitAutoFill(RevitAutoFillSource.Phases)]`: Use built-in Revit phases
- `[RevitAutoFill(RevitAutoFillSource.Categories)]`: Use Revit categories
- `[RevitAutoFill(RevitAutoFillSource.FamilyAndType)]`: Use family types

## ValueCopy Functionality

ValueCopy enables parameter and property value copying between Revit elements:

### Setup in Args

```csharp
[ValueCopyCollector(typeof(ValueCopyRevitCollector))]
public ValueCopy ValueCopy { get; set; }
```

### Implementing ValueCopy Collector

```csharp
// Simplified ValueCopy collector implementation
public class ValueCopyRevitCollector : IValueCopyRevitCollector<RevitExtensionArgs>
{
    // Define source elements for value copy
    public ValueCopyRevitSources GetSources(IValueCopyRevitContext context, RevitExtensionArgs args)
    {
        // Filter elements that can be sources for parameter values
        var filter = new FilteredElementCollector(context.Document)
            .WhereElementIsElementType();
        return new ValueCopyRevitSources(filter);
    }

    // Define target elements for value copy
    public ValueCopyRevitTargets GetTargets(IValueCopyRevitContext context, RevitExtensionArgs args)
    {
        // Similar filtering for target elements
        return new ValueCopyRevitTargets(GetElementsFilter(context));
    }
}
```

### Using ValueCopy in Command

```csharp
// Get the handler for the ValueCopy functionality
var valueCopyHandler = context.GetHandler(args.ValueCopy);

// Three common usage patterns:
valueCopyHandler.Handle(sourceElement, targetElement);  // Copy between elements
valueCopyHandler.Handle(targetElement);                 // Copy within same element
valueCopyHandler.Handle(sourceElement, targetElements); // Copy to multiple targets
```

## Custom AutoFill Collectors

Implement intelligent parameter suggestions with a custom collector:

```csharp
// Custom collector for populating UI dropdowns with parameter names
public class ParameterAutoFillCollector : IRevitAutoFillCollector<ExtensionArgs>
{
    public Dictionary<string, string> Get(UIApplication uiApplication, ExtensionArgs args)
    {
        var result = new Dictionary<string, string>();
        
        try {
            // Collect relevant parameter names from the Revit model
            var document = uiApplication.ActiveUIDocument.Document;
            var parameterNames = GetRelevantParameterNames(document);
            
            // Add each parameter to the result dictionary
            foreach (var name in parameterNames)
                result.Add(name, name);
        }
        catch (Exception e) {
            result.Add(string.Empty, $"Error: {e.Message}");
        }
        
        return result;
    }
}
```

## Best Practices

1. **Check for null references**: Always verify document and elements exist before operating on them
2. **Use transactions**: Wrap all model modifications in transactions
3. **Error handling**: Provide clear error messages when operations fail
4. **Filter elements efficiently**: Use appropriate filters to improve performance
5. **Document your code**: Add comments to explain complex operations
6. **Validate input parameters**: Ensure required parameters are provided and valid

## Example: Parameter Copy Extension

Here's a simplified example of how to create a parameter copy extension:

```csharp
// Args class - defines input UI
public class ParameterCopyArgs
{
    [Description("Source Parameter")]
    [CustomRevitAutoFill(typeof(ParameterAutoFillCollector))]
    public string SourceParameter { get; set; }
    
    [Description("Target Parameter")]
    [CustomRevitAutoFill(typeof(ParameterAutoFillCollector))]
    public string TargetParameter { get; set; }
}

// Command class - key implementation points
public class ParameterCopyCommand : IRevitExtension<ParameterCopyArgs>
{
    public IExtensionResult Run(IRevitExtensionContext context, ParameterCopyArgs args, 
        CancellationToken cancellationToken)
    {
        // Get document and selected elements
        var document = context.UIApplication.ActiveUIDocument?.Document;
        var selectedIds = context.UIApplication.ActiveUIDocument.Selection.GetElementIds();
        
        // Execute with transaction
        using var transaction = new Transaction(document, "Parameter Copy");
        transaction.Start();
        
        // Core logic: copy parameter values between elements
        foreach (var id in selectedIds)
        {
            var element = document.GetElement(id);
            var sourceParam = element.LookupParameter(args.SourceParameter);
            var targetParam = element.LookupParameter(args.TargetParameter);
            
            // Copy value based on parameter storage type
            if (sourceParam != null && targetParam != null)
            {
                // Copy parameter value (simplified)
                CopyParameterValue(sourceParam, targetParam);
            }
        }
        
        transaction.Commit();
        return Result.Text.Succeeded("Parameters copied successfully");
    }
}
```

## Troubleshooting

- **Nothing happens**: Check for errors in exception handling
- **Transaction issues**: Ensure Start() and Commit() are properly paired
- **Element not found**: Verify element selection filters
- **Parameter problems**: Confirm parameter existence and type match
- **Compilation errors**: Check for missing references or namespaces