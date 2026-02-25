namespace RevitAppFramework;

public class RevitContext(global::Autodesk.Revit.UI.UIApplication uiApplication)
{
    public global::Autodesk.Revit.UI.UIApplication UIApplication => uiApplication;
    public bool HasModelOpen => uiApplication.ActiveUIDocument != null;
    public global::Autodesk.Revit.UI.UIDocument UIDocument => uiApplication.ActiveUIDocument ?? throw new global::System.InvalidOperationException("Revit has no active model open.");
    public global::Autodesk.Revit.DB.Document? Document => uiApplication.ActiveUIDocument?.Document ?? throw new global::System.InvalidOperationException("Revit has no active model open.");
}
