# AGENTS.md

This file gives reusable guidance to coding agents working in the generated Tekla App Extension project.

## Project shape

- `*Args.cs` defines the Assistant configuration model.
- `*AppExtensionCommand` should stay focused on startup and application wiring.
- Views and view models own UI concerns. Services, handlers, and CQRS flows own host API work.
- This template starts an interactive Tekla app extension. Keep `*AppExtensionCommand` limited to startup and application wiring.

## Always-on rules

- Keep host API work out of windows and views.
- Use `Result.*` helpers for user-facing outcomes.
- Do not catch `Exception` or `OperationCanceledException`.
- Keep comments short and rely on clear naming plus docs for deeper explanations.
- Start docs and implementation tasks by using the `extension-docs` assistant docs MCP tool for current Assistant extension guidance.
- If the tool is unavailable, use `skills/mcp-setup/README.md` to restore the assistant MCP server in the active agent framework before continuing.
- Use the skills below when the task is focused on docs lookup, `*Args.cs` evolution, or platform-specific app behavior.

## Skills

- `skills/docs-routing/README.md` - start here to load the core app-extension docs for this template.
- `skills/mcp-setup/README.md` - restore the assistant MCP server when `extension-docs` is unavailable.
- `skills/args-evolution/README.md` - apply when editing `*Args.cs`, upgrades, collectors, or field metadata.
- `skills/platform-guide/README.md` - apply when changing startup flow, handlers/services, or platform API behavior.
