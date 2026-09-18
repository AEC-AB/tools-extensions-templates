## name: platform-guide
description: Use this skill when editing `*Command.cs`, collector code, or Tekla-specific runtime logic


# Platform guide

Use this skill when editing `*Command.cs`, collector code, or Tekla-specific runtime logic.

## Resolve documentation first

Run `skills/docs-routing/SKILL.md` to locate `ExtensionDocsRoot`, then read the relevant Markdown file from that directory. The resolved bundle is available offline after restore.
## API context

- Create a `Model` and verify the connection with `GetConnectionStatus()`.
- Use `ModelObjectSelector` for selected-object workflows.
- Commit changes only when model updates are complete.

## Platform rules

- Use `ITeklaAutoFillCollector<TArgs>` when option values should come from the active model.
- Verify model connection before operations.
- Filter model objects for performance.
- Handle null and cast issues explicitly.
- Commit only after successful updates.
- Prefer Tekla terminology in labels and results.

