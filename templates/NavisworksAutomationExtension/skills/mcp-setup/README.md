# MCP setup

Use this skill when the `extension-docs` assistant docs MCP tool is missing or the assistant MCP server is not configured in the workspace.

## Restore the assistant MCP server

1. Open `.vscode/mcp.json` in the project.
2. Ensure it contains an `assistant` server under `servers`.
3. If the file or entry is missing, recreate it from this template's `.vscode/mcp.json`.
4. Confirm the server uses `command` `assistant` with args `["mcp"]`.

## Expected config

```json
{
  "servers": {
    "assistant": {
      "type": "stdio",
      "command": "assistant",
      "args": [
        "mcp"
      ]
    }
  }
}
```

## Why this matters

The assistant MCP server exposes the `extension-docs` tool, which gives current guidance for building Assistant extensions.
