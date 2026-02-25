namespace TeklaAutomationExtension;

public class TeklaAutomationExtensionCommand : ITeklaExtension<TeklaAutomationExtensionArgs>
{
    public IExtensionResult Run(ITeklaExtensionContext context, TeklaAutomationExtensionArgs args, CancellationToken cancellationToken)
    {
        // The Model class represents a single model open in Tekla Structures.
        // Before interaction with the model, the user will have to create one
        // instance of this class.
        var model = new Model();

        if (!model.GetConnectionStatus())
            return Result.Text.Failed("Tekla has no active model open");

        // The ModelHandler class provides information about the currently open Tekla Structures
        var modelHandler = new ModelHandler();

        // Get selected objects in model
        var selectedObjects = new Tekla.Structures.Model.UI.ModelObjectSelector().GetSelectedObjects();
        
        // Create a message with the input text
        var message = $"Input = {args.TextInput}";

        // Return a result with the message
        return Result.Text.Succeeded(message);
    }
}