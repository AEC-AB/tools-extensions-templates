namespace TeklaAutomationExtension;

public class TeklaAutomationExtensionCommand : ITeklaExtension<TeklaAutomationExtensionArgs>
{
    // Entry point for Tekla extensions. Verify model connectivity, then run your workflow.
    public IExtensionResult Run(ITeklaExtensionContext context, TeklaAutomationExtensionArgs args, CancellationToken cancellationToken)
    {
        // Open connection to the active Tekla model.
        var model = new Model();

        if (!model.GetConnectionStatus())
            return Result.Text.Failed("Tekla has no active model open");

        // Access model metadata/services when needed.
        var modelHandler = new ModelHandler();

        // Read the current selection (replace with your real processing).
        var selectedObjects = new Tekla.Structures.Model.UI.ModelObjectSelector().GetSelectedObjects();

        var message = $"Input = {args.TextInput}";
        return Result.Text.Succeeded(message);
    }
}