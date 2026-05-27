---
applyTo: '**/*.cs'
---
# Assistant Automation Platform Instructions

Assistant extensions run in Assistant and focus on host-agnostic workflow logic such as files, web APIs, external tools, and Assistant variable exchange.

Use `ui-common.instructions.md` for Args and field behavior. Use the `extension-docs` MCP tool with `content assistant` when you need Assistant-specific execution details.

## Platform Scope

Assistant extensions run outside a host CAD/BIM process. Use them for:

- desktop automation
- file transformation workflows
- REST and cloud API orchestration
- integration logic that reads or writes Assistant variables

If the implementation must access a host model, selection, or UI API, switch to the corresponding host-specific template instead of extending this one.

## Command Contract

```csharp
public class MyExtensionCommand : IAssistantExtension<MyArgs>
{
    public async Task<IExtensionResult> RunAsync(
        IAssistantExtensionContext context,
        MyArgs args,
        CancellationToken cancellationToken)
    {
        await ExecuteAsync(cancellationToken);
        return Result.Text.Succeeded("Success");
    }
}
```

## Best Practices

1. Validate file paths, URLs, and external identifiers before execution.
2. Pass `CancellationToken` to every I/O-bound operation.
3. Keep logic host-agnostic and testable.
4. Return actionable failure messages with the next thing a user should check.
5. Use Assistant variables only for small workflow state, not as a substitute for durable storage.

## Exception Management

1. Do not use `catch (Exception)` in command or collector code.
2. Catch only expected exceptions you can recover from and return a user-facing failure message.
3. Do not catch `OperationCanceledException` and do not convert cancellation to failed results.
4. Let unexpected exceptions bubble up; Assistant handles unhandled exceptions.
