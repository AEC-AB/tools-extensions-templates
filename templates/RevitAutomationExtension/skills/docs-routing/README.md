# Docs routing

Use this skill when you need framework guidance before changing code in the Revit Automation Extension template.

If the `extension-docs` assistant docs MCP tool is unavailable, use `../mcp-setup/README.md` first to restore the assistant MCP server in the active agent framework.

## MCP entry points

Use the `extension-docs` MCP tool:

- `operation=index` to list available docs
- `operation=search` with focused queries such as `revit transaction`, `valuecopy`, or `args visibility`
- `operation=content` with document ids such as `quick-start`, `args-developer-guide`, `cookbook`, `reference`, or `revit`

## Suggested reading order

1. `quick-start` for the template shape
2. `revit` for platform-specific behavior
3. `args-developer-guide` when changing configuration classes
4. `cookbook` for reusable patterns worth copying
5. `reference` for exact field syntax and validation rules
