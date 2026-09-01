---
name: docs-routing
description: Use when extension framework guidance is needed before changing code.
---

# Docs routing

1. If the active project's obj/project.assets.json is missing, restore the project first. Then search the file for the libraries entry CW.Assistant.ExtensionDocs.Bundle/<version> (for example, `rg -n '"CW\.Assistant\.ExtensionDocs\.Bundle/' obj/project.assets.json`) and capture the exact resolved version without reading the whole file.
2. Use that resolved version.
3. Append cw.assistant.extensiondocs.bundle/<version>/contentFiles/any/any/Resources/ExtensionDocs to each local package root in packageFolders.
4. Use the first existing directory as ExtensionDocsRoot. If none exists, restore is incomplete.

Read Markdown directly from ExtensionDocsRoot. Do not substitute another version when restore has not produced the bundle.

After resolving `ExtensionDocsRoot`, read `ExtensionDocsRoot/AGENT.md` for the canonical reading order and platform guide selection.
