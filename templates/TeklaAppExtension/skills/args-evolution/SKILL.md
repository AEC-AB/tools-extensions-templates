## name: args-evolution
description: Use this skill when editing `*Args.cs` in the Tekla App Extension template


# Args evolution

Use this skill when editing `*Args.cs` in the Tekla App Extension template.

## Resolve documentation first

Run `skills/docs-routing/SKILL.md` to locate `ExtensionDocsRoot`, then read the relevant Markdown file from that directory. The resolved bundle is available offline after restore.
## Upgrade rules

1. Add or bump `[ArgsVersion(N)]` when you change the persisted Args shape.
2. Implement `IArgsUpgrade<TOldArgs, TNewArgs>` for structural changes.
3. Migrate existing values into the new model instead of silently dropping user data.
4. Use defaults only for genuinely new values.

## Collectors and field guidance

- Collectors: `ITeklaAutoFillCollector<TArgs>`
- Use collector-backed fields for model-driven values.

