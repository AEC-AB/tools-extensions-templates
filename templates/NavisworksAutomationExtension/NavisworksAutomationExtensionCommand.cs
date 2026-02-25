namespace NavisworksAutomationExtension;

public class NavisworksAutomationExtensionCommand : INavisworksExtension<NavisworksAutomationExtensionArgs>
{
    public IExtensionResult Run(INavisworksExtensionContext context, NavisworksAutomationExtensionArgs args, CancellationToken cancellationToken)
    {
        var document = Application.ActiveDocument;

        if (document is null)
            return Result.Text.Failed("Navisworks has no active model open");

        // Get selected element ids in model
        var selectedObjects = document.CurrentSelection.SelectedItems;

        // Loop through selected elements
        foreach (var selectedObject in selectedObjects)
        {
            // Get the element name
            var elementName = selectedObject.DisplayName;
        }

        // Create a message with the input text
        var message = $"Input = {args.TextInput}";

        // Return a result with the message
        return Result.Text.Succeeded(message);
    }
}