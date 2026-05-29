using RevitAppExtension.CQRS;

namespace RevitAppExtension.ViewModels;

public class HomeViewModel : RevitViewModelBase
{
    private readonly IContentDialogService _contentDialogService;

    private readonly ISnackbarService _snackbarService;

    public string? CurrentDocumentTitle
    {
        get => Get<string?>();
        set => Set(value);
    }

    public string? Comment
    {
        get => Get<string?>();
        set => When(value)
                .Notify(SetCommentOnSelectedElementsCommand)
               .Set();
    }

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

    public IFluentCommand GetCurrentDocumentCommand =>
            Send<GetDocumentTitleQuery, GetDocumentTitleQueryResult>()
            .Then(o => CurrentDocumentTitle = o.Title);

    public IFluentCommand SetCommentOnSelectedElementsCommand =>
            Send<SetCommentOnSelectedElementsQuery, SetCommentOnSelectedElementsQueryResult>(() => new(Comment))
            .If(() => !string.IsNullOrEmpty(Comment))
            .Handle(OnSetCommentsFailed)
            .Then(o =>
                _snackbarService.Show("Comments set", o.Message, Wpf.Ui.Controls.ControlAppearance.Primary));  

    private async Task OnSetCommentsFailed(Exception e)
    {
        await _contentDialogService.ShowAlertAsync("Error",
            "An error occurred while setting the comment.",
            "OK");
    }

    public IFluentCommand DeleteSelectedElementsCommand =>
            Send<DeleteSelectedElementsCommand>();
}