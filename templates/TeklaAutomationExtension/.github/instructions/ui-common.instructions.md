---
applyTo: '**/*Args.cs'
---
# Tekla Automation UI Guide

Use this file when editing `*Args.cs` classes in the TeklaAutomationExtension template.

`*Args.cs` is parsed into the Assistant configuration UI. Before changing fields, use the `extension-docs` MCP tool to read `args-developer-guide`, `reference`, `cookbook`, or `tekla`.

## Scope

Keep this file focused on high-level `*Args.cs` guidance only.

For complete details, use the `extension-docs` MCP tool to fetch `ARGS_DEVELOPER_GUIDE.md`:

1. `operation=search` with query `ARGS_DEVELOPER_GUIDE.md`
2. `operation=content` using the returned document id

## AutoFill Collectors

Tekla template collector interface:

- `ITeklaAutoFillCollector<TArgs>`

Use collectors when values should come from the active model instead of free text.

## Special Controls

No Tekla-only controls are required at this level. Use shared field attributes and collector-backed options where needed.
