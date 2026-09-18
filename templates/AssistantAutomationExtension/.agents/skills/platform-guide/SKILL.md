## name: platform-guide
description: Use this skill when editing `*Command.cs`, collector code, or Assistant-specific runtime logic


# Platform guide

Use this skill when editing `*Command.cs`, collector code, or Assistant-specific runtime logic.

## Resolve documentation first

Run `skills/docs-routing/SKILL.md` to locate `ExtensionDocsRoot`, then read the relevant Markdown file from that directory. The resolved bundle is available offline after restore.
## Platform scope

Use this template for:

- desktop automation
- file transformation workflows
- REST and cloud API orchestration
- integration logic that reads or writes Assistant variables

If the implementation must access a host CAD/BIM model, selection, or UI API, switch to a host-specific template instead.

## Platform rules

- Implement the command through `IAssistantExtension<TArgs>.RunAsync(IAssistantExtensionContext, TArgs, CancellationToken)`.
- Validate file paths, URLs, and external identifiers before execution.
- Keep logic host-agnostic and testable.
- Use Assistant variables only for small workflow state, not as a substitute for durable storage.
- Return actionable failure messages that tell the user what to check next.

