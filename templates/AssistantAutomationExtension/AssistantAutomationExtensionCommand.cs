
namespace AssistantAutomationExtension;

public class AssistantAutomationExtensionCommand : IAssistantExtension<AssistantAutomationExtensionArgs>
{
    // Entry point for Assistant extensions. Keep logic small and cancellation-aware.
    public async Task<IExtensionResult> RunAsync(IAssistantExtensionContext context, AssistantAutomationExtensionArgs args, CancellationToken cancellationToken)
    {
        var message = $"Input = {args.TextInput}";
        await Task.Delay(300, cancellationToken);
        return Result.Text.Succeeded(message);
    }
}