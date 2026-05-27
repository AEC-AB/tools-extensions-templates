---
applyTo: '**/*Args.cs'
---
# Navisworks Automation UI Guide

Use this file when editing `*Args.cs` classes in the NavisworksAutomationExtension template.

`*Args.cs` is parsed into the Assistant configuration UI. Before changing fields, use the `extension-docs` MCP tool to read `args-developer-guide`, `reference`, `cookbook`, or `navisworks`.

## Scope

Keep this file focused on high-level `*Args.cs` guidance only.

For complete details, use the `extension-docs` MCP tool to fetch `ARGS_DEVELOPER_GUIDE.md`:

1. `operation=search` with query `ARGS_DEVELOPER_GUIDE.md`
2. `operation=content` using the returned document id

## AutoFill Collectors

Navisworks template collector interface:

- `INavisworksAutoFillCollector<TArgs>`

Use collectors when values should come from the active model context instead of free text.

## Special Controls

No Navisworks-only controls are required at this level. Use shared field attributes and collector-backed options where needed.
