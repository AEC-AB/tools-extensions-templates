---
name: docs-routing
description: Use when extension framework guidance is needed before changing code.
---

# Docs routing

1. Open the active project's obj/project.assets.json. Restore the project if it is missing.
2. In libraries, find CW.Assistant.ExtensionDocs.Bundle/<version>. Use that resolved version.
3. Append cw.assistant.extensiondocs.bundle/<version>/contentFiles/any/any/Resources/ExtensionDocs to each local package root in packageFolders.
4. Use the first existing directory as ExtensionDocsRoot. If none exists, restore is incomplete.

Read Markdown directly from ExtensionDocsRoot. Do not substitute another version when restore has not produced the bundle.

After resolving `ExtensionDocsRoot`, read `ExtensionDocsRoot/AGENT.md` for the canonical reading order and platform guide selection.
