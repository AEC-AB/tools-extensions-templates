# AGENTS.md

This file gives reusable guidance to coding agents working in the generated Assistant Automation Extension project.

## Project shape

- `*Args.cs` defines the Assistant configuration model.
- `*Command.cs` executes the work and returns `IExtensionResult`.
- Use this template for host-agnostic workflow logic such as file processing, web/API orchestration, desktop automation, and Assistant variable exchange.

## Always-on rules

- In the extension `.csproj`, only update `<Title>` and `<Description>` when they still look like defaults (`<Title>` is just the project name without spaces, such as `MyExtension`, and `<Description>` is `MyExtension Description`). In that case, set `<Title>` to a user-friendly name and `<Description>` to a short summary of what the extension does, because this metadata is shown to end users when browsing Assistant extensions.
- Use `IAssistantExtensionContext` for command execution and the base `IExtensionContext` helpers for `IsDryRun`, variables, and structured JSON output.
- Pass `cancellationToken` to cancellable work and check it between long-running steps.
- Use `Result.*` helpers for all outcomes. Prefer `Result.Markdown.*` for execution summaries, diagnostics, and multi-step results.
- Failure results should state what happened, why it happened when relevant, and exactly what the user should check next.
- Do not catch `Exception` or `OperationCanceledException`. Catch only expected platform exceptions you can convert into actionable failures.
- Start docs and implementation tasks by using the `extension-docs` assistant docs MCP tool for current Assistant extension guidance.
- If the tool is unavailable, use `skills/mcp-setup/SKILL.md` to restore the assistant MCP server in the active agent framework before continuing.
- Use the skills below when the task is focused on docs lookup, `*Args.cs` evolution, platform-specific runtime behavior, or README authoring.

## Skills

- `skills/docs-routing/SKILL.md` - start here to load the right `extension-docs` content and reading order for this template.
- `skills/mcp-setup/SKILL.md` - restore the assistant MCP server when `extension-docs` is unavailable.
- `skills/args-evolution/SKILL.md` - apply when editing `*Args.cs`, upgrades, collectors, or field metadata.
- `skills/platform-guide/SKILL.md` - apply when changing `*Command.cs`, collector code, or platform API behavior.
- `skills/readme-help/SKILL.md` - apply before shipping `README.md` updates.
