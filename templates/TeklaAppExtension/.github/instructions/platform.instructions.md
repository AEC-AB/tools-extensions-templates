---
applyTo: '**/*.cs'
---
# Tekla App Platform Instructions

Use `ui-common.instructions.md` for Args behavior and `common.instructions.md` for shared app rules.

For Tekla-specific implementation details, fetch docs:

1. `operation=search` with query `TEKLA_APP_EXTENSION.md`
2. `operation=content` using the returned document id

Keep Tekla-specific code rules:

1. Verify `Model().GetConnectionStatus()` before model operations.
2. Keep model logic in services/handlers, not views/windows.
3. Use selectors with explicit null checks for selected objects.
4. Commit model changes only after successful operation scope.
