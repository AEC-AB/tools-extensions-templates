---
applyTo: '**/*Args.cs'
---
# Revit Automation UI Guide

Use this file when editing `*Args.cs` classes in the RevitAutomationExtension template.

`*Args.cs` is parsed into the Assistant configuration UI. Before changing field shape, use the `extension-docs` MCP tool to read `args-developer-guide`, `reference`, `cookbook`, or `revit`.

## Scope

Keep this file focused on high-level `*Args.cs` guidance only.

For complete details, use the `extension-docs` MCP tool to fetch `ARGS_DEVELOPER_GUIDE.md`:

1. `operation=search` with query `ARGS_DEVELOPER_GUIDE.md`
2. `operation=content` using the returned document id

## Versioning and Upgrades

Before changing `*Args.cs` structure, ask: "Has this Args class already been used in production workflows?"

If yes, treat the change as a data migration task:

1. Add or bump `[ArgsVersion(N)]` on the current Args class.
2. Implement `IArgsUpgrade<TOldArgs, TNewArgs>` to map old data into the new structure.
3. Preserve existing user data when fields are renamed, moved, split, or removed.
4. Keep defaults only for truly new values, not as a replacement for migrated user input.

Use MCP docs for exact upgrade patterns and chaining details:

1. `operation=search` with query `Feature 4: Versioning & Upgrades`
2. `operation=content` using the returned document id

## AutoFill Collectors

Revit template collector interfaces:

- `IRevitAutoFillCollector<TArgs>`
- `IValueCopyRevitCollector<TArgs>` (when ValueCopy is used)

Use collectors when values must match the active Revit model exactly.

## Special Controls

Revit can use platform-specific controls at high level:

- `ElementSelectorField`
- `FilterField`
- `ValueCopyField` / `ValueCopy` workflows

Use MCP docs for exact attribute usage and constraints.
