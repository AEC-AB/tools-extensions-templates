# AGENTS.md

This file gives reusable guidance to coding agents working in the generated AutoCAD Automation Extension project.

## Project shape

- `*Args.cs` defines the Assistant configuration model.
- `*Command.cs` executes the work and returns `IExtensionResult`.
- Use this template only when the extension must access the AutoCAD document, database, or transaction APIs.

## Always-on rules

- Use `IAutoCADExtensionContext` for command execution and the base `IExtensionContext` helpers for `IsDryRun`, variables, and structured JSON output.
- Pass `cancellationToken` to cancellable work and check it between long-running steps.
- Use `Result.*` helpers for all outcomes. Prefer `Result.Markdown.*` for execution summaries, diagnostics, and multi-step results.
- Failure results should state what happened, why it happened when relevant, and exactly what the user should check next.
- Do not catch `Exception` or `OperationCanceledException`. Catch only expected platform exceptions you can convert into actionable failures.
- Use the skills below when the task is focused on docs lookup, `*Args.cs` evolution, platform-specific runtime behavior, or README authoring.

## Skills

- `skills/docs-routing/README.md` - load the right `extension-docs` content and reading order for this template.
- `skills/args-evolution/README.md` - apply when editing `*Args.cs`, upgrades, collectors, or field metadata.
- `skills/platform-guide/README.md` - apply when changing `*Command.cs`, collector code, or platform API behavior.
- `skills/readme-help/README.md` - apply before shipping `README.md` updates.
