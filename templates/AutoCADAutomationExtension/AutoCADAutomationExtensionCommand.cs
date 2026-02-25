namespace AutoCADAutomationExtension;

public class AutoCADAutomationExtensionCommand : IAutoCADExtension<AutoCADAutomationExtensionArgs>
{
    public IExtensionResult Run(IAutoCADExtensionContext context, AutoCADAutomationExtensionArgs args, CancellationToken cancellationToken)
    {
        // Here we connect to the active AutoCAD Document
        var doc = Application.DocumentManager.MdiActiveDocument;
        
        if (doc is null)
            return Result.Text.Failed("AutoCAD has no active model open");

        // Get the database from the document
        var db = doc.Database;

        // Get selected element ids in model
        var selectedObjects = doc.Editor.SelectImplied();

        // Create a transaction to modify the model
        using var transaction = db.TransactionManager.StartTransaction();

        // Lock the document to prevent changes by other threads
        using var documentLock = doc.LockDocument();

        // Access to the Model Block table for write
        if (transaction.GetObject(db.BlockTableId, OpenMode.ForWrite) is not BlockTable blockTable)
            return Result.Text.Failed("Cannot access to the BlockTable");

        if (transaction.GetObject(blockTable[BlockTableRecord.ModelSpace],
                OpenMode.ForWrite) is not BlockTableRecord blockTableRecord)
            return Result.Text.Failed("Cannot access to the BlockTableRecord");

        // Create a Circle with its center in the coordinates origin and radius = 50.
        var myCircle = new Circle
        {
            Center = new Point3d(0, 0, 0),
            Radius = 50
        };

        // Create text with TextInput value in the middle of the circle
        var myText = new DBText
        {
            Height = 20,
            TextString = args.TextInput,
            Justify = AttachmentPoint.MiddleCenter
        };
        myText.SetDatabaseDefaults();

        // Append Circle and Text to the Block table record and Database
        blockTableRecord.AppendEntity(myCircle);
        transaction.AddNewlyCreatedDBObject(myCircle, true);
        blockTableRecord.AppendEntity(myText);
        transaction.AddNewlyCreatedDBObject(myText, true);

        // Finish Transaction
        transaction.Commit();

        // Create a message with the input text
        var message = $"Input = {args.TextInput}";

        // Return a result with the message
        return Result.Text.Succeeded(message);
    }
}