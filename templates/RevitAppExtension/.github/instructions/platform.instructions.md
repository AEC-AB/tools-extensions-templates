---
applyTo: '**/*.cs'
---
# Revit App Platform Instructions

Use `ui-common.instructions.md` for Args behavior and `common.instructions.md` for shared app rules.

For Revit-specific implementation details, fetch docs:

1. `operation=search` with query `REVIT_APP_EXTENSION.md`
2. `operation=content` using the returned document id

Keep Revit-specific code rules:

1. Validate active document from `context.UIApplication.ActiveUIDocument?.Document`.
2. Keep model writes in transactions with tight scope.
3. Prefer CQRS handlers for model operations.
4. Use Revit collectors/ValueCopy only when workflows require model-driven values.
