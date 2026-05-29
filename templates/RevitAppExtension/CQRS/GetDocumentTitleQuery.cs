using RevitAppFramework.Design;

namespace RevitAppExtension.CQRS;

public class GetDocumentTitleQuery : IQuery<GetDocumentTitleQueryResult>;

public record GetDocumentTitleQueryResult(string Title);

public class GetDocumentTitleQueryHandler(RevitContext context) : IQueryHandler<GetDocumentTitleQuery, GetDocumentTitleQueryResult>
{
    public GetDocumentTitleQueryResult Execute(GetDocumentTitleQuery input, CancellationToken cancellationToken)
    {
        return new (context.Document?.Title ?? "No model is open");
    }
}

public class GetDocumentTitleDesignQueryHandler : IDesignQueryHandler<GetDocumentTitleQuery, GetDocumentTitleQueryResult>
{
    public GetDocumentTitleQueryResult Execute(GetDocumentTitleQuery input, CancellationToken cancellationToken)
    {
        return new("Design Model Title");
    }
}
