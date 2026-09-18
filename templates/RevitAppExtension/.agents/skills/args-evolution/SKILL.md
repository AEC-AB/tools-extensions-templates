## name: args-evolution
description: Use this skill when editing `*Args.cs` in the Revit App Extension template


# Args evolution

Use this skill when editing `*Args.cs` in the Revit App Extension template.

## Resolve documentation first

Run `skills/docs-routing/SKILL.md` to locate `ExtensionDocsRoot`, then read the relevant Markdown file from that directory. The resolved bundle is available offline after restore.
## Upgrade rules

Newly generated, unshipped Args classes are exempt from the production-use
question and upgrade ceremony: make structural changes directly without a
version bump or upgrade mapping.

For an existing or production Args class, ask whether the Args class may
already be used in production workflows.

If the answer is yes:

1. Add or bump `[ArgsVersion(N)]` when you change the persisted Args shape.
2. Implement `IArgsUpgrade<TOldArgs, TNewArgs>` for structural changes.
3. Migrate existing values into the new model instead of silently dropping user data.
4. Use defaults only for genuinely new values.

If the answer is no: make the structural change directly without adding a
version bump or upgrade mapping.

## Collectors and field guidance

- Collectors: `IRevitAutoFillCollector<TArgs>`, `IValueCopyRevitCollector<TArgs>`
- Controls: `ElementSelectorField`, `FilterField`, `ValueCopyField`
- Use model-driven fields only when the workflow truly depends on active-model values.

