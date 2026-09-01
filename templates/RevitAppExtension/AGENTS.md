# AGENTS.md

This file gives reusable guidance to coding agents working in the generated Revit App Extension project.

## Project shape

- `*Args.cs` defines the Assistant configuration model.
- `*AppExtensionCommand` should stay focused on startup and application wiring.
- Views and view models own UI concerns. Services, handlers, and CQRS flows own host API work.
- This template starts an interactive Revit app extension. Keep `*AppExtensionCommand` limited to startup and application wiring.

## Always-on rules

- Keep host API work out of windows and views.
- Use `Result.*` helpers for user-facing outcomes.
- Do not catch `Exception` or `OperationCanceledException`.
- Keep comments short and rely on clear naming plus docs for deeper explanations.
- Start implementation tasks with `skills/docs-routing/SKILL.md`. It resolves the offline extension docs bundled with the project's resolved dependency version.
- Use the skills below when the task is focused on docs lookup, `*Args.cs` evolution, or platform-specific app behavior.

## Skills

- `skills/docs-routing/SKILL.md` - resolve the offline NuGet documentation root and load the core app-extension guidance.
- `skills/args-evolution/SKILL.md` - apply when editing `*Args.cs`, upgrades, collectors, or field metadata.
- `skills/platform-guide/SKILL.md` - apply when changing startup flow, handlers/services, or platform API behavior.
