---
applyTo: '**/*.cs'
---
# Tekla App Common Instructions

Use this file as a short routing layer. Read implementation details from docs.

Use the `extension-docs` MCP tool:

1. `operation=search` with query `APP_EXTENSION_DEVELOPER_GUIDE.md`
2. `operation=content` using the returned document id
3. `operation=search` with query `TEKLA_APP_EXTENSION.md`
4. `operation=content` using the returned document id

Keep these rules in code:

1. Keep `*AppExtensionCommand` startup-only.
2. Keep Tekla model work in services/handlers, not views/windows.
3. Use `IExtensionResult` via `Result.*` helpers.
4. Do not catch `Exception` or `OperationCanceledException`.
5. Keep comments short and rely on docs for deeper explanations.