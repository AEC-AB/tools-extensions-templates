## name: docs-routing
description: Use this skill when you need framework guidance before changing code in the Tekla App Extension template


# Docs routing

Use this skill when you need framework guidance before changing code in the Tekla App Extension template.

If the `extension-docs` assistant docs MCP tool is unavailable, use `../mcp-setup/SKILL.md` first to restore the assistant MCP server in the active agent framework.

## Load these docs first

Use the `extension-docs` MCP tool:

1. `operation=search` with query `APP_EXTENSION_DEVELOPER_GUIDE.md`
2. `operation=content` using the returned document id
3. `operation=search` with query `TEKLA_APP_EXTENSION.md`
4. `operation=content` using the returned document id

