---
applyTo: '**/*.cs'
---
# AutoCAD Automation Platform Instructions

Use this template when the extension must access the AutoCAD document, database, or transaction APIs.

Use `ui-common.instructions.md` for Args behavior. Use the `extension-docs` MCP tool with `content autocad` when you need the AutoCAD platform guide.

## AutoCAD API Context

- Access the active document with `Application.DocumentManager.MdiActiveDocument`.
- Lock the document before write operations using `doc.LockDocument()`.
- Use database transactions for entity creation and modification.

```csharp
var doc = Application.DocumentManager.MdiActiveDocument;
if (doc is null)
{
    return Result.Text.Failed("AutoCAD has no active model open");
}

var db = doc.Database;
using var transaction = db.TransactionManager.StartTransaction();
using var documentLock = doc.LockDocument();

transaction.Commit();
```

## AutoCAD AutoFill

Use `IAutoCADAutoFillCollector<TArgs>` when the value list should come from the active drawing.

## Best Practices

1. Always lock the document before write operations.
2. Validate objects returned by `GetObject` casts.
3. Commit transactions only after successful model updates.
4. Return explicit failure messages when context is missing.
5. Prefer AutoCAD terminology in field labels and results.

## Exception Management

1. Do not use `catch (Exception)` in command or collector code.
2. Catch only known AutoCAD exceptions you can handle and convert to clear failure results.
3. Do not catch `OperationCanceledException` and do not convert cancellation to failed results.
4. Let unexpected exceptions bubble up; Assistant handles unhandled exceptions.
