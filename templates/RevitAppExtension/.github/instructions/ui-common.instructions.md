---
applyTo: '**/*Args.cs'
---
# Revit App UI Guide

Use this file when editing `*Args.cs` classes in the RevitAppExtension template.

Use docs for full field behavior:

1. `operation=search` with query `ARGS_DEVELOPER_GUIDE.md`
2. `operation=content` using the returned document id

Revit app Args highlights:

- collectors: `IRevitAutoFillCollector<TArgs>`, `IValueCopyRevitCollector<TArgs>`
- controls: `ElementSelectorField`, `FilterField`, `ValueCopyField`
