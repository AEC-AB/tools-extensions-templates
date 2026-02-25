# Template Final Pass Review

Date: 2026-02-25
Repository: `tools-extensions-templates`
Scope: `templates/*`
Status: Open findings captured for final pre-ship pass

## How This Was Validated
- Packed and installed local template package from current source.
- Generated all template types with `dotnet new`.
- Performed smoke builds of generated C# templates.
- Searched templates for placeholder text, runtime stubs, and cross-template copy/paste artifacts.

## Findings (Ordered by Severity)

### 1) High - Revit App template cannot restore using its default nuget.config
- Impact: Freshly generated `cwrvtapp` fails package restore out-of-the-box.
- Evidence:
  - `templates/RevitAppExtension/nuget.config:5` (`<clear />`)
  - `templates/RevitAppExtension/nuget.config:6` (only `nuget.org`)
  - `templates/RevitAppExtension/Configurations.targets:122` (`RevitAppFramework.$(MainRevitVersion)`)
  - `templates/RevitAppExtension/Configurations.targets:127` (`RevitSeparationAnalyzer`)
- Smoke-test failure observed: `NU1101` for `RevitAppFramework.2024` and `RevitSeparationAnalyzer`.
- Suggested fix:
  - Add required package source(s) to `templates/RevitAppExtension/nuget.config`, or
  - Remove `<clear />`, or
  - Replace with publicly available packages, plus documentation update.
- Decision: Keep nuget.org-only source; expected to resolve once `RevitAppFramework.<version>` and `RevitSeparationAnalyzer` are published.
- Work status: [~] Deferred until package publish

### 2) High - Tekla App template fails compile due to missing compile assets for MVVM package
- Impact: Generated `cwtapp` restore succeeds but compile fails (`CS0246`).
- Evidence:
  - Updated to `MVVMFluent.Wpf` `0.0.2` with `compile` assets in `templates/TeklaAppExtension/Configurations.targets`.
- Validation:
  - `cwtapp` now restores and builds successfully in Debug.
- Suggested fix:
  - Include `compile` in `IncludeAssets`, or remove `IncludeAssets` override for `MVVMFluent`.
- Work status: [x] Fixed

### 3) Medium - Revit App template likely has same MVVM package compile-assets issue
- Impact: Potential compile failure once restore/source issue is resolved.
- Evidence:
  - Updated to `MVVMFluent.Wpf` `0.0.2` with `compile` assets in `templates/RevitAppExtension/Configurations.targets`.
- Work status: [x] Fixed

### 4) Medium - Release build path is fragile in multiple templates
- Impact: `dotnet build -c Release` can fail because main version properties are only defaulted for `Debug`.
- Evidence:
  - `templates/AutoCADAutomationExtension/AutoCADAutomationExtension.csproj:16`
  - `templates/NavisworksAutomationExtension/NavisworksAutomationExtension.csproj:16`
  - `templates/RevitAppExtension/RevitAppExtension.csproj:16`
  - `templates/RevitAutomationExtension/RevitAutomationExtension.csproj:16`
  - `templates/TeklaAppExtension/TeklaAppExtension.csproj:12`
  - `templates/TeklaAutomationExtension/TeklaAutomationExtension.csproj:16`
- Suggested fix:
  - Add a configuration-agnostic default for `Main*Version` or set defaults for both Debug/Release.
- Decision: Accepted as-is for this release.
- Work status: [~] Accepted risk

### 5) Low - Tekla App template ships NotImplementedException stubs
- Impact: Runtime failure if converter paths are used.
- Evidence:
  - `templates/TeklaAppExtension/Framework/Converters/BoolToVisibilityConverter.cs:18`
  - `templates/TeklaAppExtension/Framework/Converters/InvertedBoolToBoolConverter.cs:16`
  - `templates/TeklaAppExtension/Framework/Converters/InvertedBoolToVisibilityConverter.cs:16`
- Suggested fix:
  - Implement converters or remove unused converter classes/usages.
- Decision: Accepted as-is (`ConvertBack` stubs intentionally not used).
- Work status: [~] Accepted risk

### 6) Low - Tekla App has Revit-specific comments in UI file
- Impact: User confusion and template polish issue.
- Evidence:
  - `templates/TeklaAppExtension/Views/HomeView.xaml:29`
  - `templates/TeklaAppExtension/Views/HomeView.xaml:36`
  - `templates/TeklaAppExtension/Views/HomeView.xaml:53`
  - `templates/TeklaAppExtension/Views/HomeView.xaml:64`
- Suggested fix:
  - Replace Revit wording with Tekla-specific wording.
- Work status: [x] Fixed

### 7) Low - Python templates contain placeholder TODO text
- Impact: Shipped sample quality issue.
- Evidence:
  - `templates/PythonAutomationScript/python_automation_script/python_automation_script.py:5`
  - `templates/RevitPythonAutomationScript/python_revit_automation_script/python_revit_automation_script.py:28`
- Suggested fix:
  - Replace TODO placeholder with a minimal working sample or explicit instructional scaffold.
- Decision: Keep TODO call-to-action in script templates.
- Work status: [~] Accepted as intentional

### 8) Low - Obsolete ControlDataAttribute usage across automation templates
- Impact: Build warnings now; future compatibility risk.
- Evidence:
  - Migrated args classes to field attributes (`TextField`, `DoubleField`) and added `CW.Assistant.Extensions.Contracts.Fields` global using in C# templates.
- Validation:
  - `cwae` and `cwrvtae` now build with zero `ControlData` warnings.
- Work status: [x] Fixed

## Smoke Test Snapshot
- Generated successfully: all templates.
- Debug build succeeded: `cwae`, `cwacadae`, `cwnwae`, `cwrvtae`, `cwtae`.
- Debug build failed: `cwrvtapp`, `cwtapp`.
- Release build is fragile for most platform templates due to default-version logic.

## Working Plan
1. Fix High severity items (`cwrvtapp` restore path, `cwtapp` MVVMFluent compile assets).
2. Fix Medium severity build configuration robustness.
3. Fix Low severity runtime/polish items.
4. Re-run full generate + build smoke matrix.

## Sign-off Checklist
- [ ] All generated C# templates restore in a clean environment. (Blocked only by pending nuget.org publish for Revit app dependencies)
- [ ] All generated C# templates build in `Debug` and `Release`. (Release fragility accepted as known risk)
- [ ] Python templates have no placeholder TODOs. (Intentionally kept)
- [ ] No runtime `NotImplementedException` stubs in shipped template code paths. (Accepted `ConvertBack` stubs)
- [x] No obvious cross-platform copy/paste wording errors in README/XAML/instructions.
