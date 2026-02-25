using RevitAppFramework.Design;
using System.Windows;

namespace RevitAppExtension.CQRS;

/// <summary>
/// Command to delete selected elements in the Revit model.
/// </summary>
/// <remarks>
/// This is an example of a CQRS command with no parameters.
/// Unlike queries which return results, commands perform actions and don't return values.
/// This demonstrates using an empty class as a marker for a command.
/// </remarks>
public class DeleteSelectedElementsCommand;

/// <summary>
/// Handler for the DeleteSelectedElementsCommand, removing selected elements from the Revit model.
/// </summary>
/// <remarks>
/// This class demonstrates:
/// 1. Using primary constructor syntax for dependency injection
/// 2. Implementing the ICommandHandler interface for command handling
/// 3. Using RevitContext to access Revit API safely
/// 4. Working with Revit transactions for model modifications
/// </remarks>
public class DeleteSelectedElementsCommandHandler(RevitContext context) : ICommandHandler<DeleteSelectedElementsCommand>
{
    /// <summary>
    /// Executes the command to delete all selected elements.
    /// </summary>
    /// <param name="input">The command input (no parameters needed in this case)</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests</param>
    /// <remarks>
    /// This method demonstrates:
    /// 1. Checking for a valid document
    /// 2. Accessing the current selection
    /// 3. Creating and managing a Revit transaction
    /// 4. Using the Revit API to delete elements
    /// </remarks>
    public void Execute(DeleteSelectedElementsCommand input, CancellationToken cancellationToken)
    {
        var doc = context.Document;
        if (doc is null)
        {
            return;
        }        // Get the IDs of all currently selected elements
        var ids = context.UIDocument!.Selection.GetElementIds();
        
        // Create and start a transaction - required for model modifications
        using var transaction = new Transaction(doc, "Delete selected elements");
        transaction.Start();
        
        // Delete each selected element by its ID
        foreach (var id in ids)
        {
            doc.Delete(id);
        }
        
        // Commit the transaction to save changes to the model
        transaction.Commit();
    }
}

/// <summary>
/// Design handler for the DeleteSelectedElementsCommand, simulating the command execution in design mode.
/// </summary>
/// <remarks>
/// This class demonstrates how to implement a design-time command handler.
/// It can be used for testing or design-time scenarios where the actual Revit API is not available.
/// </remarks>
public class DeleteSelectedElementsDesignCommandHandler : IDesignCommandHandler<DeleteSelectedElementsCommand>
{
    /// <summary>
    /// Executes the command in design mode.
    /// </summary>
    /// <param name="input">The command input (no parameters needed in this case)</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests</param>
    /// <remarks>
    /// This method simulates the command execution without modifying any Revit model.
    /// It can be used for design-time testing or UI updates.
    /// </remarks>
    public void Execute(DeleteSelectedElementsCommand input, CancellationToken cancellationToken)
    {
        MessageBox.Show("Design mode: DeleteSelectedElementsCommand executed. No elements were deleted.");
    }
}
