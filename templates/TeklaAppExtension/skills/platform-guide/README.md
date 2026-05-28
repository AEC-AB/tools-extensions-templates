# Platform guide

Use this skill when editing startup flow, handlers/services, or Tekla-specific app behavior.

## Load these docs first

Use the `extension-docs` MCP tool:

1. `operation=search` with query `TEKLA_APP_EXTENSION.md`
2. `operation=content` using the returned document id

## Platform rules

- Verify `Model().GetConnectionStatus()` before model operations.
- Keep Tekla model logic in services and handlers, not views or windows.
- Use selectors with explicit null checks for selected objects.
- Commit model changes only after the full operation succeeds.
