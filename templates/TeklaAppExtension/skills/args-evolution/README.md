# Args evolution

Use this skill when editing `*Args.cs` in the Tekla App Extension template.

## Load these docs first

Use the `extension-docs` MCP tool:

1. `operation=search` with query `ARGS_DEVELOPER_GUIDE.md`
2. `operation=content` using the returned document id
3. `operation=search` with query `Feature 4: Versioning & Upgrades`
4. `operation=content` using the returned document id

## Upgrade rules

1. Add or bump `[ArgsVersion(N)]` when you change the persisted Args shape.
2. Implement `IArgsUpgrade<TOldArgs, TNewArgs>` for structural changes.
3. Migrate existing values into the new model instead of silently dropping user data.
4. Use defaults only for genuinely new values.

## Collectors and field guidance

- Collectors: `ITeklaAutoFillCollector<TArgs>`
- Use collector-backed fields for model-driven values.
