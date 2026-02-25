using RevitAppFramework.Design;

namespace RevitAppExtension.CQRS;

/// <summary>
/// Query to retrieve the title of the current Revit document.
/// </summary>
/// <remarks>
/// This is an example of a simple query with no parameters.
/// The query implements the IQuery interface which is part of the CQRS pattern.
/// </remarks>
public class GetDocumentTitleQuery : IQuery<GetDocumentTitleQueryResult>;

/// <summary>
/// Result record for the GetDocumentTitleQuery.
/// </summary>
/// <param name="Title">The title of the current Revit document.</param>
public record GetDocumentTitleQueryResult(string Title);

/// <summary>
/// Handler for the GetDocumentTitleQuery, retrieving the current Revit document title.
/// </summary>
/// <remarks>
/// This class demonstrates:
/// 1. Using primary constructor syntax for dependency injection
/// 2. Implementing the IQueryHandler interface for query handling
/// 3. Using RevitContext to access Revit's Document API safely
/// </remarks>
public class GetDocumentTitleQueryHandler(RevitContext context) : IQueryHandler<GetDocumentTitleQuery, GetDocumentTitleQueryResult>
{
    /// <summary>
    /// Executes the query and returns the document title result.
    /// </summary>
    /// <param name="input">The query input (no parameters needed in this case)</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests</param>
    /// <returns>A result containing the document title or a default message if no document is open</returns>
    public GetDocumentTitleQueryResult Execute(GetDocumentTitleQuery input, CancellationToken cancellationToken)
    {
        return new (context.Document?.Title ?? "No model is open");
    }
}

/// <summary>
/// Design model for the GetDocumentTitleQuery, used for design-time testing and development.
/// </summary>
/// <remarks>
/// This class demonstrates how to implement a design-time query handler.
/// It can be used for testing or design-time scenarios where the actual Revit API is not available.
/// </remarks>
public class GetDocumentTitleDesignQueryHandler : IDesignQueryHandler<GetDocumentTitleQuery, GetDocumentTitleQueryResult>
{
    /// <summary>
    /// Executes the query to retrieve the title of a document.
    /// </summary>
    /// <param name="input">The query containing the parameters required to retrieve the document title.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the operation before it completes.</param>
    /// <returns>A <see cref="GetDocumentTitleQueryResult"/> containing the title of the document.</returns>
    /// <remarks>
    /// This method simulates the execution of the query in a design-time environment.
    /// It does not interact with the actual Revit API and is used for testing purposes.
    /// </remarks>
    public GetDocumentTitleQueryResult Execute(GetDocumentTitleQuery input, CancellationToken cancellationToken)
    {
        return new("Design Model Title");
    }
}
