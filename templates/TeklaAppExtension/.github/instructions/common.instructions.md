---
applyTo: '**/*.cs'
---
# Common Integration Notes

## Assistant Variables

- To set Assistant Variables, call `context.SetVariableValue("VariableName", "TheValue");`.
- The `context` parameter passed to your `Run` method exposes `SetVariableValue`.
- Use the exact method name `SetVariableValue` (not `SetVariable` or other variants).
- Minimal pattern:

```csharp
// Write a value the assistant can read later
context.SetVariableValue("OutputVariableName", myValue);
```

## Reading Assistant Variables

- To read an Assistant variable, call `context.GetVariableValue("variableName")`.
- Use the exact method name `GetVariableValue`.
- `GetVariableValue` returns `string?`; handle null when the variable is missing.
- Minimal pattern:

```csharp
string? value = context.GetVariableValue("variableName");
```