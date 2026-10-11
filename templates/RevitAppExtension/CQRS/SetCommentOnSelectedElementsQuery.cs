using RevitAppFramework.Design;

namespace RevitAppExtension.CQRS;

public record SetCommentOnSelectedElementsQuery(string? Comment) : IQuery<SetCommentOnSelectedElementsQueryResult>;

public record SetCommentOnSelectedElementsQueryResult(string Message);

internal class SetCommentOnSelectedElementsQueryHandler(RevitContext context) : IQueryHandler<SetCommentOnSelectedElementsQuery, SetCommentOnSelectedElementsQueryResult>
{
    public SetCommentOnSelectedElementsQueryResult Execute(SetCommentOnSelectedElementsQuery input, CancellationToken cancellationToken)
    {
        var selectedElements = context.UIDocument?.Selection.GetElementIds().Select(id => context.Document?.GetElement(id));
        if (selectedElements is null)
            return new("No elements selected");

        using var trans = new Transaction(context.Document, "Set comments");
        trans.Start();

        foreach (var element in selectedElements)
        {
            element?.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).Set(input.Comment);

            if (cancellationToken.IsCancellationRequested)
                return new("Operation cancelled");

            Thread.Sleep(1000);
        }

        trans.Commit();
        return new("Comment set on selected elements");
    }
}

internal class SetCommentOnSelectedElementsDesignQueryHandler() : IDesignQueryHandler<SetCommentOnSelectedElementsQuery, SetCommentOnSelectedElementsQueryResult>
{
    public SetCommentOnSelectedElementsQueryResult Execute(SetCommentOnSelectedElementsQuery input, CancellationToken cancellationToken)
    {
        return new("Comment set on selected elements");
    }
}