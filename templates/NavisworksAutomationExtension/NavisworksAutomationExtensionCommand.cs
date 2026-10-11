namespace NavisworksAutomationExtension;

public class NavisworksAutomationExtensionCommand : INavisworksExtension<NavisworksAutomationExtensionArgs>
{
    // Entry point for Navisworks extensions. Read model state and return a concise result.
    public IExtensionResult Run(INavisworksExtensionContext context, NavisworksAutomationExtensionArgs args, CancellationToken cancellationToken)
    {
        var document = Application.ActiveDocument;

        if (document is null)
            return Result.Text.Failed("Navisworks has no active model open");

        var selectedObjects = document.CurrentSelection.SelectedItems;

        // Iterate selected items (replace with your real processing).
        foreach (var selectedObject in selectedObjects)
        {
            var elementName = selectedObject.DisplayName;
        }

        var message = $"Input = {args.TextInput}";
        return Result.Text.Succeeded(message);
    }
}