---
applyTo: '**/*.cs'
---
# Assistant Automation Extension Guide

Assistant extensions are used to run workflow automation in Assistant, such as file processing, API orchestration, desktop automation, and variable-based handoffs between steps.

Use the local extension docs through the `extension-docs` MCP tool:

- `operation=index` to list available extension docs
- `operation=search` with a focused query such as `args versioning`, `autofill`, or `assistant variables`
- `operation=content` with a document id such as `quick-start`, `args-developer-guide`, `cookbook`, `reference`, or `assistant`

Suggested reading order:

1. `quick-start` for the working template pattern
2. `cookbook` for copy-paste examples
3. `args-developer-guide` when changing configuration classes
4. `reference` when looking up field syntax or validation rules
5. `assistant` when the extension uses Assistant-only execution context

## Extension Shape

1. `*Args.cs` defines the configuration users edit in Assistant.
2. `*Command.cs` executes the work and returns `IExtensionResult`.
3. Keep this template host-agnostic: prefer files, HTTP APIs, desktop automation, and orchestration logic.

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

Use `IAssistantExtensionContext` in command execution. This context inherits `IExtensionContext`.

Base `IExtensionContext` capabilities available across platforms:

- `IsDryRun`
- `GetVariables()`
- `AddVariables(...)`
- `GetVariableValue(...)`
- `SetVariableValue(...)`
- `AsJson(...)`

Rules:

1. Pass `cancellationToken` to all cancellable async APIs (HTTP, file I/O, delays, external calls).
2. For long-running loops or multi-step workflows, check cancellation between steps.
3. Return early with a clear result when cancellation is requested.
4. Use context-provided base capabilities for dry-run logic, variable exchange, and structured JSON output.

## README Lifecycle

Update `README.md` to an end-user guide before delivery.

Use the `extension-docs` MCP tool to fetch `WRITING_EXTENSION_README_HELP_FILES.md` content:

1. `operation=search` with query `WRITING_EXTENSION_README_HELP_FILES.md`
2. `operation=content` using the returned document id