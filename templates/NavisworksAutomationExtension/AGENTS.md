# AGENTS.md

This file gives reusable guidance to coding agents working in the generated Navisworks Automation Extension project.

## Project shape

- `*Args.cs` defines the Assistant configuration model.
- `*Command.cs` executes the work and returns `IExtensionResult`.
- Use this template only when the extension must access the Navisworks active document, model items, or current selection.

## Always-on rules

- Use `INavisworksExtensionContext` for command execution and the base `IExtensionContext` helpers for `IsDryRun`, variables, and structured JSON output.
- Pass `cancellationToken` to cancellable work and check it between long-running steps.
- Use `Result.*` helpers for all outcomes. Prefer `Result.Markdown.*` for execution summaries, diagnostics, and multi-step results.
- Failure results should state what happened, why it happened when relevant, and exactly what the user should check next.
- Do not catch `Exception` or `OperationCanceledException`. Catch only expected platform exceptions you can convert into actionable failures.
- Start docs and implementation tasks by using the `extension-docs` assistant docs MCP tool for current Assistant extension guidance.
- If the tool is unavailable, use `.agents/skills/mcp-setup/SKILL.md` to restore the assistant MCP server in the active agent framework before continuing.
- Use the skills below when the task is focused on docs lookup, `*Args.cs` evolution, platform-specific runtime behavior, or README authoring.

## Skills

- `.agents/skills/docs-routing/SKILL.md` - start here to load the right `extension-docs` content and reading order for this template.
- `.agents/skills/mcp-setup/SKILL.md` - restore the assistant MCP server when `extension-docs` is unavailable.
- `.agents/skills/args-evolution/SKILL.md` - apply when editing `*Args.cs`, upgrades, collectors, or field metadata.
- `.agents/skills/platform-guide/SKILL.md` - apply when changing `*Command.cs`, collector code, or platform API behavior.
- `.agents/skills/readme-help/SKILL.md` - apply before shipping `README.md` updates.
