---
applyTo: '**/*.cs'
---
# Revit Platform Instructions

Use `ui-common.instructions.md` for all UI configuration and validation conventions.

## Revit API Context

- Access active document via `context.UIApplication.ActiveUIDocument?.Document`.
- Return a failure result if no active document exists.
- Use `Transaction` for model modifications.

```csharp
var document = context.UIApplication.ActiveUIDocument?.Document;
if (document is null)
    return Result.Text.Failed("Revit has no active model open");

using var transaction = new Transaction(document, "My Extension");
transaction.Start();
// Modify elements
transaction.Commit();
```

## ValueCopy

Use ValueCopy for parameter copy workflows.

```csharp
[ValueCopyCollector(typeof(ValueCopyRevitCollector))]
public ValueCopy ValueCopy { get; set; }


public class ValueCopyRevitCollector : IValueCopyRevitCollector<RevitExtensionDemoArgs>
{
    public ValueCopyRevitSources GetSources(IValueCopyRevitContext context, RevitExtensionDemoArgs args)
    {  
        if (args.FilterControl is not null)
        {
            return new ValueCopyRevitSources(args.FilterControl);
        }

        var filter = new FilteredElementCollector(context.Document).OfCategory(BuiltInCategory.OST_GenericModel).WhereElementIsElementType();
        return new ValueCopyRevitSources(filter);
    }

    public ValueCopyRevitTargets GetTargets(IValueCopyRevitContext context, RevitExtensionDemoArgs args)
    {
        if (args.FilterControlWithSelectedCategories is not null)
        {
            return new ValueCopyRevitTargets(args.FilterControlWithSelectedCategories);
        }

        var filter = new FilteredElementCollector(context.Document).OfCategory(BuiltInCategory.OST_Walls).WhereElementIsElementType();
        return new ValueCopyRevitTargets(filter);
    }
}
```

Implement `IValueCopyRevitCollector<TArgs>` to provide sources and targets.

## Revit AutoFill

```csharp
[OptionsField(
    Label = "Custom Revit AutoFill Collector",
    ToolTip = "ComboBox control with custom Revit-specific autofill data collector implementation.",
    CollectorType = typeof(CustomRevitAutoFillCollector))]
public string? CustomRevitCollector { get; set; }

internal class CustomRevitAutoFillCollector : IRevitAutoFillCollector<RevitExtensionDemoArgs>
{
    public Dictionary<string, string> Get(UIApplication uiApplication, RevitExtensionDemoArgs args)
    {
        var result = new Dictionary<string, string>();

        try
        {
            var document = uiApplication.ActiveUIDocument?.Document;

            if (document is null)
                return result;

            using var element = new FilteredElementCollector(document).OfCategory(BuiltInCategory.OST_GenericModel).FirstElement();

            foreach (var parameter in element.GetOrderedParameters())
            {
                result.Add(parameter.Definition.Name, parameter.Definition.Name);
            }
        }
        catch
        {
            // ignore
        }

        return result;
    }
}
```

## Best Practices

1. Keep transactions short and focused.
2. Verify parameter existence and storage types before setting values.
3. Use filtered collectors to avoid full-model scans.
4. Guard against empty selections when required by command logic.
