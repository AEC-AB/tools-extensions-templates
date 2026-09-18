## name: args-evolution
description: Use this skill when editing `*Args.cs` in the Revit Automation Extension template


# Args evolution

Use this skill when editing `*Args.cs` in the Revit Automation Extension template.

## Resolve documentation first

Run `skills/docs-routing/SKILL.md` to locate `ExtensionDocsRoot`, then read the relevant Markdown file from that directory. The resolved bundle is available offline after restore.
## Upgrade rules

Newly generated, unshipped Args classes are exempt from the production-use
question and upgrade ceremony: make structural changes directly without a
version bump or upgrade mapping.

For an existing or production Args class, ask whether the Args class may
already be used in production workflows.

If the answer is yes:

1. Add or bump `[ArgsVersion(N)]` on the current Args class.
2. Implement `IArgsUpgrade<TOldArgs, TNewArgs>` to map the old structure into the new one.
3. Preserve existing user data when fields are renamed, moved, split, or removed.
4. Use defaults only for truly new values, not as a replacement for migrated data.

## Collectors and field guidance

- Collector interfaces: `IRevitAutoFillCollector<TArgs>`, `IValueCopyRevitCollector<TArgs>`
- Use collectors when values must match the active Revit model exactly.

## Special controls

- `ElementSelectorField`
- `FilterField`
- `ValueCopyField` / `ValueCopy` workflows

Use the docs for exact attribute usage and constraints.

