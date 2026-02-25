using MVVMFluent;
using Wpf.Ui;
using Wpf.Ui.Controls;
using Wpf.Ui.Extensions;

namespace TeklaAppExtension.ViewModels;

/// <summary>
/// ViewModel for the Home view, providing data binding and command functionality.
/// Implements MVVM pattern using the ViewModelBase from MVVMFluent.
/// </summary>
public class HomeViewModel : ViewModelBase
{
    // Service dependencies injected via constructor
    private readonly IContentDialogService _contentDialogService;
    private readonly ISnackbarService _snackbarService;
    private readonly ITeklaService _teklaService;

    /// <summary>
    /// Gets or sets the current Tekla document title (model name).
    /// This property is bound to the UI to display the active model name.
    /// </summary>
    public string? CurrentDocumentTitle
    {
        get => Get<string?>();
        set => Set(value);
    }

    /// <summary>
    /// Gets or sets the comment text to be applied to selected Tekla objects.
    /// When this property changes, it notifies the SetCommentOnSelectedElementsCommand.
    /// </summary>
    public string? Comment
    {
        get => Get<string?>();
        set => When(value)
                .Notify(SetCommentOnSelectedElementsCommand) // Notify the command when the comment changes
               .Set();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HomeViewModel"/> class.
    /// </summary>
    /// <param name="args">Application arguments containing initial settings.</param>
    /// <param name="contentDialogService">Service for displaying dialogs.</param>
    /// <param name="snackbarService">Service for displaying notification snackbars.</param>
    /// <param name="teklaService">Service for interacting with Tekla Structures.</param>
    public HomeViewModel(
        TeklaAppExtensionArgs args,
        IContentDialogService contentDialogService,
        ISnackbarService snackbarService,
        ITeklaService teklaService
        )
    {
        // Initialize the comment from the input arguments
        Comment = args.InitialComment;
        
        // Store service references for later use
        _contentDialogService = contentDialogService;
        _snackbarService = snackbarService;
        _teklaService = teklaService;
    }
    /// <summary>
    /// Command to get the name of the current Tekla model.
    /// Bound to a button in the UI.
    /// </summary>
    public IFluentCommand GetModelNameCommand =>
            Do(GetName);

    /// <summary>
    /// Command to set a comment on selected Tekla objects.
    /// This command is only enabled when there is a non-empty comment.
    /// </summary>
    public IFluentCommand SetCommentOnSelectedElementsCommand =>
            Do(SetCommentOnSelected)
            .If(() => !string.IsNullOrEmpty(Comment)); // Command is enabled only if Comment is not empty

    /// <summary>
    /// Command to delete selected Tekla objects.
    /// Bound to a button in the UI.
    /// </summary>
    public IFluentCommand DeleteSelectedElementsCommand =>
            Do(DeleteSelected);
            
    /// <summary>
    /// Deletes the currently selected objects in the Tekla model.
    /// Shows appropriate notification messages based on the result.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task DeleteSelected()
    {
        try
        {
            // Use the TeklaService to delete selected objects and get the count
            int objectCount = _teklaService.DeleteSelectedObjects();

            // Show appropriate success message based on the result
            if (objectCount > 0)
            {
                // Objects were deleted successfully
                _snackbarService.Show("Success", $"{objectCount} object(s) deleted.", ControlAppearance.Success);
            }
            else
            {
                // No objects were selected or deletion didn't succeed
                _snackbarService.Show("Information", "No objects were selected.", ControlAppearance.Info);
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions that occurred during deletion
            // Show an error dialog with the exception message
            await _contentDialogService.ShowAlertAsync("Error",
             $"An error occurred while deleting objects: {ex.Message}",
             "OK");
        }
    }
    private async Task SetCommentOnSelected(CancellationToken cancellationToken)
    {
        try
        {
            // Ensure Comment is not null before passing to the service
            string comment = Comment ?? string.Empty;
            int objectCount = _teklaService.SetCommentOnSelectedObjects(comment, cancellationToken);

            // Show success message
            if (objectCount > 0)
            {
                _snackbarService.Show("Success", $"Comment set on {objectCount} object(s).", ControlAppearance.Success);
            }
            else
            {
                _snackbarService.Show("Information", "No objects were selected.", ControlAppearance.Info);
            }
        }
        catch (OperationCanceledException)
        {
            // User clicked the Cancel button. Re-throw OperationCanceledException exception to be handled by the MVVMFluent framework.
            throw;
        }
        catch (Exception ex)
        {
            // Handle the exception and show an error message
            await _contentDialogService.ShowAlertAsync("Error",
             $"An error occurred while setting the comment: {ex.Message}",
             "OK");
        }
    }

    private void GetName()
    {
        try
        {
            CurrentDocumentTitle = _teklaService.GetModelName();
        }
        catch(Exception)
        {
            _snackbarService.Show("Error", "Unable to communicate with Tekla", ControlAppearance.Danger);
        }
    }
}
