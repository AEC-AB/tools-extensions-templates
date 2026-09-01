## name: platform-guide
description: Use this skill when editing `*Command.cs`, collector code, or Navisworks-specific runtime logic


# Platform guide

Use this skill when editing `*Command.cs`, collector code, or Navisworks-specific runtime logic.

## Resolve documentation first

Run `skills/docs-routing/SKILL.md` to locate `ExtensionDocsRoot`, then read the relevant Markdown file from that directory. The resolved bundle is available offline after restore.
## API context

- Access the active document through `Autodesk.Navisworks.Api.Application.ActiveDocument`.
- Work with the current selection through `document.CurrentSelection.SelectedItems`.
- Guard null or empty context before processing.

## Platform rules

- Use `INavisworksAutoFillCollector<TArgs>` when option values should come from the active document or selection.
- Prefer read-only traversal for review and analysis workflows.
- Catch only known collector exceptions when you can provide a safe fallback.
- Provide clear result messages for user feedback.
- Prefer Navisworks terminology in labels and results.

