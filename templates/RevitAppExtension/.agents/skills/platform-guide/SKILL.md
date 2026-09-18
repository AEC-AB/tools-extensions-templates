## name: platform-guide
description: Use this skill when editing startup flow, handlers/services, or Revit-specific app behavior


# Platform guide

Use this skill when editing startup flow, handlers/services, or Revit-specific app behavior.

## Resolve documentation first

Run `skills/docs-routing/SKILL.md` to locate `ExtensionDocsRoot`, then read the relevant Markdown file from that directory. The resolved bundle is available offline after restore.
## Platform rules

- Validate the active document from `context.UIApplication.ActiveUIDocument?.Document` before model access.
- Keep model writes in short transactions with tight scope.
- Prefer CQRS handlers and services for model operations instead of pushing API logic into windows or view models.
- Use Revit collectors or ValueCopy only when the workflow requires model-driven values.

