using RevitAppFramework.Design;
using System.Windows;

namespace RevitAppExtension.CQRS;

public class DeleteSelectedElementsCommand;

public class DeleteSelectedElementsCommandHandler(RevitContext context) : ICommandHandler<DeleteSelectedElementsCommand>
{
    public void Execute(DeleteSelectedElementsCommand input, CancellationToken cancellationToken)
    {
        var doc = context.Document;
        if (doc is null)
        {
            return;
        }

        var ids = context.UIDocument!.Selection.GetElementIds();

        using var transaction = new Transaction(doc, "Delete selected elements");
        transaction.Start();

        foreach (var id in ids)
        {
            doc.Delete(id);
        }

        transaction.Commit();
    }
}

public class DeleteSelectedElementsDesignCommandHandler : IDesignCommandHandler<DeleteSelectedElementsCommand>
{
    public void Execute(DeleteSelectedElementsCommand input, CancellationToken cancellationToken)
    {
        MessageBox.Show("Design mode: DeleteSelectedElementsCommand executed. No elements were deleted.");
    }
}
