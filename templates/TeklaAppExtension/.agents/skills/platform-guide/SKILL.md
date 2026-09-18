## name: platform-guide
description: Use this skill when editing startup flow, handlers/services, or Tekla-specific app behavior


# Platform guide

Use this skill when editing startup flow, handlers/services, or Tekla-specific app behavior.

## Resolve documentation first

Run `skills/docs-routing/SKILL.md` to locate `ExtensionDocsRoot`, then read the relevant Markdown file from that directory. The resolved bundle is available offline after restore.
## Platform rules

- Verify `Model().GetConnectionStatus()` before model operations.
- Keep Tekla model logic in services and handlers, not views or windows.
- Use selectors with explicit null checks for selected objects.
- Commit model changes only after the full operation succeeds.

