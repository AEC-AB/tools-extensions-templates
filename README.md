# Assistant Extension Templates for Tools by AEC

This package contains `dotnet new` templates for creating extensions and automation scripts for Assistant by AEC.

## What this package is for

Use these templates when you want to quickly scaffold a new Assistant extension with the expected project layout, configuration files, and build setup for supported platforms.

## Install and use

1. Install the template package:

```bash
dotnet new install CW.Assistant.Extensions.Templates
```

2. List available templates:

```bash
dotnet new list cw
```

3. Create a template project (example):

```bash
dotnet new cwrvtae -n MyRevitAutomationExtension
```

## Included templates

- `cwae` - Assistant Automation Extension
- `cwacadae` - AutoCAD Automation Extension for Assistant
- `cwnwae` - Navisworks Automation Extension for Assistant
- `cwrvtapp` - Revit App Extension for Assistant
- `cwrvtae` - Revit Automation Extension for Assistant
- `cwtapp` - Tekla App Extension for Assistant
- `cwtae` - Tekla Automation Extension for Assistant
- `cwpyas` - Assistant Python Automation Script
- `cwpyrvtas` - Revit Python Automation Script

## Documentation

- Assistant extension docs: [tools wiki](https://toolswiki.aec.se/en/Assistant/Develop/Extensions)
- Open source extensions and examples: https://github.com/AEC-AB/tools-extensions-public

## Template maintenance

Use `Assistant.Extensions.Templates.EditTemplates.sln` to edit templates and dependencies together.
