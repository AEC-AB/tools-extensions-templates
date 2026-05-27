---
applyTo: '**/*Args.cs'
---
# Revit App UI Guide

Use this file when editing `*Args.cs` classes in the RevitAppExtension template.

Use docs for full field behavior:

1. `operation=search` with query `ARGS_DEVELOPER_GUIDE.md`
2. `operation=content` using the returned document id

## Versioning and Upgrades

Before changing `*Args.cs` structure, ask: "Has this Args class already been used in production workflows?"

If yes, implement migrations:

1. Add or bump `[ArgsVersion(N)]`.
2. Implement `IArgsUpgrade<TOldArgs, TNewArgs>` for changed structure.
3. Migrate existing values into the new model; do not silently drop user data.

Use docs for exact patterns:

1. `operation=search` with query `Feature 4: Versioning & Upgrades`
2. `operation=content` using the returned document id

Revit app Args highlights:

- collectors: `IRevitAutoFillCollector<TArgs>`, `IValueCopyRevitCollector<TArgs>`
- controls: `ElementSelectorField`, `FilterField`, `ValueCopyField`
