---
applyTo: '**/*.cs'
---
# Revit Automation Platform Instructions

Use this template when the extension must access the Revit document, selection, or transaction APIs.

Use `ui-common.instructions.md` for Args behavior. Use the `extension-docs` MCP tool with `content revit` when you need the Revit platform guide.

## Revit API Context

- Access the active document through `context.UIApplication.ActiveUIDocument?.Document`.
- Return a failure result if no active document is open.
- Wrap model changes in a `Transaction`.

```csharp
var document = context.UIApplication.ActiveUIDocument?.Document;
if (document is null)
{
    return Result.Text.Failed("Revit has no active model open");
}

using var transaction = new Transaction(document, "My Extension");
transaction.Start();
// Modify elements
transaction.Commit();
```

## ValueCopy

Use `ValueCopy` when the workflow copies parameter values between Revit element sets.

```csharp
[ValueCopyCollector(typeof(ValueCopyRevitCollector))]
public ValueCopy ValueCopy { get; set; }
```

Implement `IValueCopyRevitCollector<TArgs>` only when the source and target element logic is specific to the command.

## Revit AutoFill

Use `IRevitAutoFillCollector<TArgs>` or Revit-specific autofill attributes when values should be populated from the active model rather than typed manually.

## Best Practices

1. Keep transactions short and focused.
2. Verify parameter existence and storage types before setting values.
3. Use filtered collectors to avoid full-model scans.
4. Guard against empty selections when the command depends on user selection.
5. Prefer Revit terminology in labels and result messages.

## Exception Management

1. Do not use `catch (Exception)` in command or collector code.
2. Catch only specific Revit/API exceptions you can handle and return actionable failure results.
3. Do not catch `OperationCanceledException` and do not convert cancellation to failed results.
4. Let unexpected exceptions bubble up; Assistant handles unhandled exceptions.
