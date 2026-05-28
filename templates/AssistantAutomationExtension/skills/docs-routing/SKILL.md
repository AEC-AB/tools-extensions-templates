## name: docs-routing
description: Replace with description of the skill and when Claude should use it.

# Insert skill instructions below

# Docs routing

Use this skill when you need framework guidance before changing code in the Assistant Automation Extension template.

If the `extension-docs` assistant docs MCP tool is unavailable, use `../mcp-setup/SKILL.md` first to restore the assistant MCP server in the active agent framework.

## MCP entry points

Use the `extension-docs` MCP tool:

- `operation=index` to list available docs
- `operation=search` with focused queries such as `args versioning`, `autofill`, or `assistant variables`
- `operation=content` with document ids such as `quick-start`, `args-developer-guide`, `cookbook`, `reference`, or `assistant`

## Suggested reading order

1. `quick-start` for the template shape
2. `assistant` for platform-specific behavior
3. `args-developer-guide` when changing configuration classes
4. `cookbook` for reusable patterns worth copying
5. `reference` for exact field syntax and validation rules
