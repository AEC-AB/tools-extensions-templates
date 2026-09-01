## name: args-evolution
description: Use this skill when editing `*Args.cs` in the AutoCAD Automation Extension template


# Args evolution

Use this skill when editing `*Args.cs` in the AutoCAD Automation Extension template.

## Resolve documentation first

Run `skills/docs-routing/SKILL.md` to locate `ExtensionDocsRoot`, then read the relevant Markdown file from that directory. The resolved bundle is available offline after restore.
## Upgrade rules

Before changing the `*Args.cs` structure, ask whether the Args class may already be used in production workflows.

If the answer is yes:

1. Add or bump `[ArgsVersion(N)]` on the current Args class.
2. Implement `IArgsUpgrade<TOldArgs, TNewArgs>` to map the old structure into the new one.
3. Preserve existing user data when fields are renamed, moved, split, or removed.
4. Use defaults only for truly new values, not as a replacement for migrated data.

## Collectors and field guidance

- Collector interface: `IAutoCADAutoFillCollector<TArgs>`
- Use collectors when values should come from the active drawing instead of free text.

## Special controls

No AutoCAD-only special controls are required here. Use shared field attributes and the docs for exact options.

