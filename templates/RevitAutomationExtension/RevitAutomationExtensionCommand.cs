namespace RevitAutomationExtension;

public class RevitAutomationExtensionCommand : IRevitExtension<RevitAutomationExtensionArgs>
{
    // Entry point for Revit extensions. Validate context, process selected elements, and honor cancellation.
    public IExtensionResult Run(IRevitExtensionContext context, RevitAutomationExtensionArgs args, CancellationToken cancellationToken)
    {
        var document = context.UIApplication.ActiveUIDocument?.Document;

        if (document is null)
            return Result.Text.Failed("Revit has no active model open");

        var selectedObjects = context.UIApplication.ActiveUIDocument!.Selection.GetElementIds();

        using var transaction = new Transaction(document, "RevitAutomationExtension");
        transaction.Start();

        // Iterate selected elements (replace with your real logic).
        foreach (var elementId in selectedObjects)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var element = document.GetElement(elementId);
        }

        transaction.Commit();

        var message = $"Operation completed successfully. Input text was: {args.TextInput}";
        return Result.Text.Succeeded(message);
    }
}