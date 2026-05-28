## name: platform-guide
description: Replace with description of the skill and when Claude should use it.

# Insert skill instructions below

# Platform guide

Use this skill when editing startup flow, handlers/services, or Revit-specific app behavior.

## Load these docs first

Use the `extension-docs` MCP tool:

1. `operation=search` with query `REVIT_APP_EXTENSION.md`
2. `operation=content` using the returned document id

## Platform rules

- Validate the active document from `context.UIApplication.ActiveUIDocument?.Document` before model access.
- Keep model writes in short transactions with tight scope.
- Prefer CQRS handlers and services for model operations instead of pushing API logic into windows or view models.
- Use Revit collectors or ValueCopy only when the workflow requires model-driven values.
