using RevitAppFramework.Design;

namespace RevitAppExtension.CQRS;

/// <summary>
/// Query to set a comment on all selected Revit elements.
/// </summary>
/// <param name="Comment">The comment text to apply to selected elements.</param>
/// <remarks>
/// This query demonstrates:
/// 1. Using record types for queries with parameters
/// 2. Implementing IQuery interface as part of the CQRS pattern
/// </remarks>
public record SetCommentOnSelectedElementsQuery(string? Comment) : IQuery<SetCommentOnSelectedElementsQueryResult>;

/// <summary>
/// Result record for the SetCommentOnSelectedElementsQuery.
/// </summary>
/// <param name="Message">A message describing the result of the operation.</param>
public record SetCommentOnSelectedElementsQueryResult(string Message);

/// <summary>
/// Handler for the SetCommentOnSelectedElementsQuery, applying comments to selected Revit elements.
/// </summary>
/// <remarks>
/// This class demonstrates:
/// 1. Using primary constructor syntax for dependency injection
/// 2. Implementing the IQueryHandler interface for query handling
/// 3. Using RevitContext to access Revit API safely
/// 4. Working with Revit transactions
/// 5. Handling cancellation requests
/// </remarks>
internal class SetCommentOnSelectedElementsQueryHandler(RevitContext context) : IQueryHandler<SetCommentOnSelectedElementsQuery, SetCommentOnSelectedElementsQueryResult>
{
    /// <summary>
    /// Executes the query to set comments on all selected elements.
    /// </summary>
    /// <param name="input">The query input containing the comment text</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests</param>
    /// <returns>A result describing the outcome of the operation</returns>
    /// <remarks>
    /// This method demonstrates:
    /// 1. Checking for element selection
    /// 2. Creating and managing a Revit transaction
    /// 3. Accessing element parameters via the Revit API
    /// 4. Handling cancellation during long-running operations
    /// </remarks>
    public SetCommentOnSelectedElementsQueryResult Execute(SetCommentOnSelectedElementsQuery input, CancellationToken cancellationToken)
    {
        var selectedElements = context.UIDocument?.Selection.GetElementIds().Select(id => context.Document?.GetElement(id));
        if (selectedElements is null)
            return new("No elements selected");        // Create and start a transaction
        // Transactions are required for any modification to the Revit model
        using var trans = new Transaction(context.Document, "Set comments");
        trans.Start();

        foreach (var element in selectedElements)
        {
            // Apply the comment to the built-in comment parameter of each element
            element?.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).Set(input.Comment);
            
            // Check for cancellation after each element to allow for responsive termination
            if (cancellationToken.IsCancellationRequested)
                return new("Operation cancelled");
                
            Thread.Sleep(1000); // Simulate long operation to demonstrate cancellation
        }

        // Commit the transaction to save all changes to the model
        trans.Commit();

        // Return a success message
        return new("Comment set on selected elements");
    }
}

/// <summary>
/// Design-time handler for the SetCommentOnSelectedElementsQuery, providing mock response data.
/// </summary>
/// <remarks>
/// This class demonstrates:
/// 1. Implementing the IDesignQueryHandler interface for design-time testing
/// 2. Providing a consistent mock response for UI and integration testing
/// 3. Simulating Revit behavior without requiring a live Revit session
/// </remarks>
internal class SetCommentOnSelectedElementsDesignQueryHandler() : IDesignQueryHandler<SetCommentOnSelectedElementsQuery, SetCommentOnSelectedElementsQueryResult>
{
    /// <summary>
    /// Executes the query and returns a mock result for design-time testing.
    /// </summary>
    /// <param name="input">The query input containing the comment text</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests</param>
    /// <returns>A result with a fixed response message simulating successful comment application</returns>
    /// <remarks>
    /// This method simulates the execution of the query in a design-time environment.
    /// It does not interact with the actual Revit API and is used for testing purposes.
    /// </remarks>
    public SetCommentOnSelectedElementsQueryResult Execute(SetCommentOnSelectedElementsQuery input, CancellationToken cancellationToken)
    {
        return new("Comment set on selected elements");
    }
}