namespace AutoCADAutomationExtension;

public class AutoCADAutomationExtensionCommand : IAutoCADExtension<AutoCADAutomationExtensionArgs>
{
    // Entry point for AutoCAD extensions. Validate document, run a transaction, then return a user-facing result.
    public IExtensionResult Run(IAutoCADExtensionContext context, AutoCADAutomationExtensionArgs args, CancellationToken cancellationToken)
    {
        // Get the active document.
        var doc = Application.DocumentManager.MdiActiveDocument;
        
        if (doc is null)
            return Result.Text.Failed("AutoCAD has no active model open");

        var db = doc.Database;

        var selectedObjects = doc.Editor.SelectImplied();

        // Use a transaction for model edits.
        using var transaction = db.TransactionManager.StartTransaction();

        // Lock while editing document state.
        using var documentLock = doc.LockDocument();

        if (transaction.GetObject(db.BlockTableId, OpenMode.ForWrite) is not BlockTable blockTable)
            return Result.Text.Failed("Cannot access to the BlockTable");

        if (transaction.GetObject(blockTable[BlockTableRecord.ModelSpace],
                OpenMode.ForWrite) is not BlockTableRecord blockTableRecord)
            return Result.Text.Failed("Cannot access to the BlockTableRecord");

        // Create simple demo entities.
        var myCircle = new Circle
        {
            Center = new Point3d(0, 0, 0),
            Radius = 50
        };

        var myText = new DBText
        {
            Height = 20,
            TextString = args.TextInput,
            Justify = AttachmentPoint.MiddleCenter
        };
        myText.SetDatabaseDefaults();

        blockTableRecord.AppendEntity(myCircle);
        transaction.AddNewlyCreatedDBObject(myCircle, true);
        blockTableRecord.AppendEntity(myText);
        transaction.AddNewlyCreatedDBObject(myText, true);

        transaction.Commit();

        var message = $"Input = {args.TextInput}";
        return Result.Text.Succeeded(message);
    }
}