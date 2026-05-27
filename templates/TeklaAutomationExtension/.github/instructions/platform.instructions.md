---
applyTo: '**/*.cs'
---
# Tekla Automation Platform Instructions

Use this template when the extension must access the Tekla model, selection, or commit APIs.

Use `ui-common.instructions.md` for Args behavior. Use the `extension-docs` MCP tool with `content tekla` when you need the Tekla platform guide.

## Tekla API Context

- Create a `Model` and verify connection with `GetConnectionStatus()`.
- Use `ModelObjectSelector` for selected object workflows.
- Commit changes only when model updates are complete.

```csharp
var model = new Model();
if (!model.GetConnectionStatus())
{
    return Result.Text.Failed("No active Tekla model");
}

var selector = new Tekla.Structures.Model.UI.ModelObjectSelector();
var selectedObjects = selector.GetSelectedObjects();

model.CommitChanges();
```

## Tekla AutoFill

Use `ITeklaAutoFillCollector<TArgs>` when option values should come from the active model.

## Best Practices

1. Verify model connection before operations.
2. Filter model objects for performance.
3. Handle null and cast issues explicitly.
4. Commit only after successful updates.
5. Prefer Tekla terminology in labels and results.

## Exception Management

1. Do not use `catch (Exception)` in command or collector code.
2. Catch only known Tekla/runtime exceptions you can handle and convert to clear failure results.
3. Do not catch `OperationCanceledException` and do not convert cancellation to failed results.
4. Let unexpected exceptions bubble up; Assistant handles unhandled exceptions.
