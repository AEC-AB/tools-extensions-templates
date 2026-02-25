using RevitAppExtension.CQRS;

namespace RevitAppExtension.ViewModels;

/// <summary>
/// ViewModel for the Home view, provides functionality for interacting with the current Revit document.
/// This is an example of using the MVVM pattern with Revit-dependent operations.
/// </summary>
/// <remarks>
/// This class demonstrates two key patterns used in RevitApp extensions:
/// 1. Property change notification using Get/Set methods from RevitViewModelBase
/// 2. Command implementation for Revit-dependent operations using the Send method
/// 
/// The ViewModel contains two core commands that interface with Revit:
/// - GetCurrentDocumentCommand: Retrieves the current document title
/// - SetCommentOnSelectedElementsCommand: Sets a comment value on selected elements
/// </remarks>
public class HomeViewModel : RevitViewModelBase
{
    /// <summary>
    /// Service for displaying dialog windows to the user
    /// </summary>
    private readonly IContentDialogService _contentDialogService;
    
    /// <summary>
    /// Service for displaying toast notifications to the user
    /// </summary>
    private readonly ISnackbarService _snackbarService;

    /// <summary>
    /// Gets or sets the current Revit document title.
    /// </summary>
    /// <remarks>
    /// Uses the Get/Set pattern from RevitViewModelBase for property change notification.
    /// </remarks>
    public string? CurrentDocumentTitle
    {
        get => Get<string?>();
        set => Set(value);
    }

    /// <summary>
    /// Gets or sets the comment text that will be applied to selected elements.
    /// </summary>
    /// <remarks>
    /// Uses the fluent When/Notify/Set pattern that:
    /// 1. Listens for value changes
    /// 2. Notifies the SetCommentOnSelectedElementsCommand to reevaluate its can-execute state
    /// 3. Sets the property value
    /// </remarks>
    public string? Comment
    {
        get => Get<string?>();
        set => When(value)
                .Notify(SetCommentOnSelectedElementsCommand)
               .Set();
    }    /// <summary>
    /// Initializes a new instance of the <see cref="HomeViewModel"/> class.
    /// </summary>
    /// <param name="dependencies">Base dependencies required by all view models</param>
    /// <param name="args">User arguments provided from the extension startup</param>
    /// <param name="contentDialogService">Service for displaying dialog windows</param>
    /// <param name="snackbarService">Service for displaying toast notifications</param>
    /// <remarks>
    /// The constructor:
    /// 1. Initializes the base view model with core dependencies
    /// 2. Sets the initial Comment value from user arguments
    /// 3. Stores required services for later use
    /// </remarks>
    public HomeViewModel(
        ViewModelBaseDeps dependencies, 
        RevitAppExtensionArgs args, 
        IContentDialogService contentDialogService,
        ISnackbarService snackbarService
        ) : base(dependencies)
    {
        Comment = args.InitialComment;
        _contentDialogService = contentDialogService;
        _snackbarService = snackbarService;
    }

    /// <summary>
    /// Command to retrieve the current Revit document title.
    /// </summary>
    /// <remarks>
    /// This command demonstrates the Send pattern for Revit-dependent operations.
    /// It executes the GetDocumentTitleQuery in the Revit API context and
    /// then updates the CurrentDocumentTitle property with the result.
    /// </remarks>
    public IFluentCommand GetCurrentDocumentCommand =>
            Send<GetDocumentTitleQuery, GetDocumentTitleQueryResult>()
            .Then(o => CurrentDocumentTitle = o.Title);

    /// <summary>
    /// Command to set a comment on all currently selected elements in Revit.
    /// </summary>
    /// <remarks>
    /// This command demonstrates the complete fluent command pattern:
    /// 1. Send - Executes a query in the Revit API context
    /// 2. If - Only allows execution when the comment is not empty
    /// 3. Handle - Provides error handling for failures
    /// 4. Then - Performs an action on success (showing a notification)
    /// </remarks>
    public IFluentCommand SetCommentOnSelectedElementsCommand =>
            Send<SetCommentOnSelectedElementsQuery, SetCommentOnSelectedElementsQueryResult>(() => new(Comment))
            .If(() => !string.IsNullOrEmpty(Comment))   // Only allow the user to click the button when the comment is not empty
            .Handle(OnSetCommentsFailed)                // Handle when the query throws an exception
            .Then(o =>                                  // After the comment is set, send a message to the snackbar
                _snackbarService.Show("Comments set", o.Message, Wpf.Ui.Controls.ControlAppearance.Primary));  

    /// <summary>
    /// Error handler for the SetCommentOnSelectedElementsCommand.
    /// </summary>
    /// <param name="e">The exception that occurred during command execution</param>
    /// <returns>A task representing the asynchronous error handling operation</returns>
    private async Task OnSetCommentsFailed(Exception e)
    {
        await _contentDialogService.ShowAlertAsync("Error",
            "An error occurred while setting the comment.",
            "OK");
    }

    public IFluentCommand DeleteSelectedElementsCommand =>
            Send<DeleteSelectedElementsCommand>();
}