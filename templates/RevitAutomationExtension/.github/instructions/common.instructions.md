---
applyTo: '**/*.cs'
---
# Revit Automation Extension Guide

This template is for extensions that execute inside Revit and need Revit document, selection, or transaction APIs.

Use the local extension docs through the `extension-docs` MCP tool:

- `operation=index` to list available extension docs
- `operation=search` for focused topics such as `revit transaction`, `valuecopy`, or `args visibility`
- `operation=content` with document ids such as `quick-start`, `args-developer-guide`, `cookbook`, `reference`, or `revit`

Suggested reading order:

1. `quick-start` for the template shape
2. `revit` for Revit-specific behavior
3. `args-developer-guide` when changing configuration classes
4. `cookbook` for patterns worth copying
5. `reference` for exact field syntax and validation rules

## Extension Shape

1. `*Args.cs` defines the configuration users edit in Assistant.
2. `*Command.cs` runs inside Revit and works with the Revit API.
3. Use this template only when host API access is required.

## Extension Results

Use `IExtensionResult` for all outcomes and use the built-in `Result` helper class to create consistent responses:

- `Result.Text.*` for short plain-text summaries
- `Result.Markdown.*` for structured, user-friendly output
- `Result.Empty.*` only when no user-facing content is needed

Prefer `Result.Markdown.*` when returning execution summaries, validation feedback, diagnostics, or multi-step outcomes.

Make results actionable and informative:

1. State what happened.
2. Explain why it happened when relevant.
3. List exactly what the user should change to fix configuration errors.
4. Include useful insights (counts, impacted items, skipped items, warnings) so users can make decisions.

## Cancellation and Context

Use `IRevitExtensionContext` in command execution. This context inherits `IExtensionContext` and adds Revit-specific members.

Base `IExtensionContext` capabilities available across platforms:

- `IsDryRun`
- `GetVariables()`
- `AddVariables(...)`
- `GetVariableValue(...)`
- `SetVariableValue(...)`
- `AsJson(...)`

Revit-specific context members:

- `UIApplication` for access to the active Revit UI/application session
- `GetHandler(ValueCopy)` for ValueCopy handling

Rules:

1. Pass `cancellationToken` to cancellable APIs and propagate it through the command flow.
2. Revit command execution is typically synchronous; still use `cancellationToken` for long operations such as iterating large collections or multi-step model processing.
3. For long-running loops or multi-step operations, check cancellation between steps.
4. Return early with an informative result when cancellation is requested.
5. Read Revit state through `context.UIApplication` and use base context methods for dry-run/variables/JSON.

## README Lifecycle

Update `README.md` to an end-user guide before delivery.

Use the `extension-docs` MCP tool to fetch `WRITING_EXTENSION_README_HELP_FILES.md` content:

1. `operation=search` with query `WRITING_EXTENSION_README_HELP_FILES.md`
2. `operation=content` using the returned document id