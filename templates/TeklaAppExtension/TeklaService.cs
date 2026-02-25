// filepath: c:\Users\mnli\source\repos\Assistant.Extensions.Templates\templates\TeklaAppExtension\TeklaService.cs
// This file implements services for interacting with the Tekla Structures API
// Tekla Structures is a BIM software for structural engineering and construction modeling
using Tekla.Structures.Model;

namespace TeklaAppExtension;

/// <summary>
/// Service class that provides operations for interacting with the Tekla Structures model.
/// Implements the ITeklaService interface to provide model management functionality.
/// </summary>
public class TeklaService : ITeklaService
{
    /// <summary>
    /// Retrieves the name of the currently active Tekla Structures model.
    /// </summary>
    /// <returns>The name of the current model as a string.</returns>
    public string GetModelName()
    {
        // Create a new Model object to access the current Tekla model
        // The Model class is the main entry point to the Tekla Structures model
        var model = new Model();

        // Return the model name from the model information
        // GetInfo() returns ModelInfo object which contains various properties about the current model
        return model.GetInfo().ModelName;
    }

    /// <summary>
    /// Deletes all currently selected objects in the Tekla Structures model.
    /// </summary>
    /// <returns>The number of objects successfully deleted.</returns>
    /// <exception cref="Exception">Throws any exceptions encountered during the deletion process.</exception>
    public int DeleteSelectedObjects()
    {
        // Create a new model instance to access the current Tekla model
        // Each new Model() instance connects to the same active model in Tekla Structures
        var model = new Model();

        // Get a ModelObjectSelector instance from UI namespace to access the selection
        // ModelObjectSelector provides methods to interact with the user's current selection in Tekla UI
        var selector = new Tekla.Structures.Model.UI.ModelObjectSelector();

        // Get the currently selected objects in Tekla
        // GetSelectedObjects returns an IEnumerator containing all selected model objects
        var selectedObjects = selector.GetSelectedObjects();

        // Counter for tracking the number of successfully deleted objects
        // This will help report how many objects were affected by the operation
        int objectCount = 0;

        // Iterate through all selected objects
        // We use MoveNext() rather than foreach because we're working with an IEnumerator
        while (selectedObjects.MoveNext())
        {
            if (selectedObjects.Current != null)
            {
                // Delete the selected object and get the result
                // The Delete() method returns a boolean indicating success or failure
                bool success = selectedObjects.Current.Delete();

                // Increment counter if deletion was successful
                // This ensures we only count objects that were actually deleted
                if (success)
                {
                    objectCount++;
                }
            }
        }

        // Commit the changes to the model to make them permanent
        // Without CommitChanges(), the changes would only be temporary and not saved
        model.CommitChanges();

        // Return the count of deleted objects
        // This allows the caller to know how many objects were affected
        return objectCount;


    }
    /// <summary>
    /// Sets a comment as a user-defined attribute on all currently selected objects in the Tekla model.
    /// </summary>
    /// <param name="comment">The comment text to set on each selected object.</param>
    /// <returns>The number of objects successfully modified with the comment.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the comment parameter is null or empty.</exception>
    /// <exception cref="Exception">Throws any exceptions encountered during the comment setting process.</exception>
    public int SetCommentOnSelectedObjects(string comment, CancellationToken cancellationToken)
    {

        // Create a new model instance to access the current Tekla model
        // This gives us access to the currently open model in Tekla Structures
        var model = new Model();

        // Get a ModelObjectSelector instance from UI namespace to access the selection
        // This connects to the user's current selection in the Tekla UI
        var selector = new Tekla.Structures.Model.UI.ModelObjectSelector();

        // Get the currently selected objects in Tekla
        // This returns an enumerator for all objects the user has selected
        var selectedObjects = selector.GetSelectedObjects();

        // Counter for tracking the number of successfully modified objects
        // Used to report how many objects were affected by this operation
        int objectCount = 0;

        // Iterate through all selected objects
        // Using the IEnumerator pattern for traversing the selection
        while (selectedObjects.MoveNext())
        {
            if (selectedObjects.Current != null)
            {
                cancellationToken.ThrowIfCancellationRequested();
                // Set the comment as a user-defined attribute (UDA) for each object
                // "comment" is the name of the UDA in Tekla Structures
                // User-defined attributes allow storing custom metadata on model objects
                bool success = selectedObjects.Current.SetUserProperty("comment", comment);

                // If setting the property was successful, modify the object to save the changes
                // The Modify() method must be called to register changes with the model
                if (success)
                {
                    selectedObjects.Current.Modify();
                    objectCount++;
                }
            }
        }

        // Commit the changes to the model to make them permanent
        // CommitChanges() ensures the modifications are saved to the model database
        model.CommitChanges();

        // Return the count of modified objects
        // This allows the caller to know how many objects were affected
        return objectCount;

    }
}

/// <summary>
/// Interface defining the operations that can be performed on a Tekla Structures model.
/// Provides methods for retrieving model information and manipulating model objects.
/// </summary>
public interface ITeklaService
{
    /// <summary>
    /// Retrieves the name of the currently active Tekla Structures model.
    /// </summary>
    /// <returns>The name of the current model as a string.</returns>
    string GetModelName();
    
    /// <summary>
    /// Deletes all currently selected objects in the Tekla Structures model.
    /// </summary>
    /// <returns>The number of objects successfully deleted.</returns>
    int DeleteSelectedObjects();
    
    /// <summary>
    /// Sets a comment as a user-defined attribute on all currently selected objects in the Tekla model.
    /// </summary>
    /// <param name="comment">The comment text to set on each selected object.</param>
    /// <returns>The number of objects successfully modified with the comment.</returns>
    int SetCommentOnSelectedObjects(string comment, CancellationToken cancellationToken);
}
