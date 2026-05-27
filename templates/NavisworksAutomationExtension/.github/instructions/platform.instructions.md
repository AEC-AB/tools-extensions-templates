---
applyTo: '**/*.cs'
---
# Navisworks Automation Platform Instructions

Use this template when the extension must access the Navisworks active document, model items, or current selection.

Use `ui-common.instructions.md` for Args behavior. Use the `extension-docs` MCP tool with `content navisworks` when you need the Navisworks platform guide.

## Navisworks API Context

- Access the active document through `Autodesk.Navisworks.Api.Application.ActiveDocument`.
- Work with the current selection through `document.CurrentSelection.SelectedItems`.
- Guard null or empty context before processing.

```csharp
var document = Autodesk.Navisworks.Api.Application.ActiveDocument;
if (document is null)
{
    return Result.Text.Failed("Navisworks has no active model open");
}

var selectedItems = document.CurrentSelection.SelectedItems;
foreach (var item in selectedItems)
{
    var name = item.DisplayName;
}
```

## Navisworks AutoFill

Use `INavisworksAutoFillCollector<TArgs>` when option values should come from the active document or selection.

## Best Practices

1. Check the active document before all API access.
2. Prefer read-only traversal for review and analysis workflows.
3. Catch only known collector exceptions when you can provide a safe fallback.
4. Provide clear result messages for user feedback.
5. Prefer Navisworks terminology in labels and results.

## Exception Management

1. Do not use `catch (Exception)` in command or collector code.
2. Catch only expected exceptions you can handle and convert to clear failure results.
3. Do not catch `OperationCanceledException` and do not convert cancellation to failed results.
4. Let unexpected exceptions bubble up; Assistant handles unhandled exceptions.
