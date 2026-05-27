---
applyTo: '**/*Args.cs'
---
# Assistant Automation UI Guide

Use this file when editing `*Args.cs` classes in the AssistantAutomationExtension template.

`*Args.cs` is parsed into the Assistant configuration UI. Before adding or changing fields, use the `extension-docs` MCP tool to read `args-developer-guide`, `reference`, or `cookbook`.

## Scope

Keep this file focused on high-level `*Args.cs` guidance only.

For complete details, use the `extension-docs` MCP tool to fetch `ARGS_DEVELOPER_GUIDE.md`:

1. `operation=search` with query `ARGS_DEVELOPER_GUIDE.md`
2. `operation=content` using the returned document id

## AutoFill Collectors

Assistant template collector interface:

- `IAsyncAutoFillCollector<TArgs>`

Use collectors when values should come from APIs, files, or other external systems rather than free text.

## Special Controls

No Assistant-only special controls are required at this level. Use shared field attributes and MCP docs for exact options.
