# AGENTS.md

This file gives reusable guidance to coding agents working in the generated Revit Automation Extension project.

## Project shape

- `*Args.cs` defines the Assistant configuration model.
- `*Command.cs` executes the work and returns `IExtensionResult`.
- Use this template only when the extension must access the Revit document, selection, transaction, or parameter APIs.

## Always-on rules

- Use `IRevitExtensionContext` for command execution and the base `IExtensionContext` helpers for `IsDryRun`, variables, and structured JSON output.
- Pass `cancellationToken` to cancellable work and check it between long-running steps.
- Use `Result.*` helpers for all outcomes. Prefer `Result.Markdown.*` for execution summaries, diagnostics, and multi-step results.
- Failure results should state what happened, why it happened when relevant, and exactly what the user should check next.
- Do not catch `Exception` or `OperationCanceledException`. Catch only expected platform exceptions you can convert into actionable failures.
- Start implementation tasks with `skills/docs-routing/SKILL.md`. It resolves the offline extension docs bundled with the project's resolved dependency version.
- Use the skills below when the task is focused on docs lookup, `*Args.cs` evolution, platform-specific runtime behavior, or README authoring.

## Skills

- `skills/docs-routing/SKILL.md` - resolve the offline NuGet documentation root and load the relevant guidance.
- `skills/args-evolution/SKILL.md` - apply when editing `*Args.cs`, upgrades, collectors, or field metadata.
- `skills/platform-guide/SKILL.md` - apply when changing `*Command.cs`, collector code, or platform API behavior.
- `skills/readme-help/SKILL.md` - apply before shipping `README.md` updates.
