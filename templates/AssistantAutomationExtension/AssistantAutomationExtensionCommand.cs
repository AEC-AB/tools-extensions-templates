
namespace AssistantAutomationExtension;

public class AssistantAutomationExtensionCommand : IAssistantExtension<AssistantAutomationExtensionArgs>
{
    public async Task<IExtensionResult> RunAsync(IAssistantExtensionContext context, AssistantAutomationExtensionArgs args, CancellationToken cancellationToken)
    {
        // Create a message with the input text
        var message = $"Input = {args.TextInput}";

        // Await some delay to simulate async work
        await Task.Delay(300, cancellationToken);

        // Return a result with the message
        return Result.Text.Succeeded(message);
    }
}