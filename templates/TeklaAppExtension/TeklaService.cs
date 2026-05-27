using Tekla.Structures.Model;

namespace TeklaAppExtension;

public class TeklaService : ITeklaService
{
    public string GetModelName()
    {
        var model = new Model();
        return model.GetInfo().ModelName;
    }

    public int DeleteSelectedObjects()
    {
        var model = new Model();
        var selector = new Tekla.Structures.Model.UI.ModelObjectSelector();
        var selectedObjects = selector.GetSelectedObjects();

        int objectCount = 0;
        while (selectedObjects.MoveNext())
        {
            if (selectedObjects.Current != null)
            {
                bool success = selectedObjects.Current.Delete();
                if (success)
                {
                    objectCount++;
                }
            }
        }

        model.CommitChanges();
        return objectCount;
    }

    public int SetCommentOnSelectedObjects(string comment, CancellationToken cancellationToken)
    {
        var model = new Model();
        var selector = new Tekla.Structures.Model.UI.ModelObjectSelector();
        var selectedObjects = selector.GetSelectedObjects();

        int objectCount = 0;
        while (selectedObjects.MoveNext())
        {
            if (selectedObjects.Current != null)
            {
                cancellationToken.ThrowIfCancellationRequested();
                bool success = selectedObjects.Current.SetUserProperty("comment", comment);
                if (success)
                {
                    selectedObjects.Current.Modify();
                    objectCount++;
                }
            }
        }

        model.CommitChanges();
        return objectCount;
    }
}

public interface ITeklaService
{
    string GetModelName();

    int DeleteSelectedObjects();

    int SetCommentOnSelectedObjects(string comment, CancellationToken cancellationToken);
}
