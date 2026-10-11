using MVVMFluent;
using Wpf.Ui;
using Wpf.Ui.Controls;
using Wpf.Ui.Extensions;

namespace TeklaAppExtension.ViewModels;

public class HomeViewModel : ViewModelBase
{
    private readonly IContentDialogService _contentDialogService;
    private readonly ISnackbarService _snackbarService;
    private readonly ITeklaService _teklaService;

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
        TeklaAppExtensionArgs args,
        IContentDialogService contentDialogService,
        ISnackbarService snackbarService,
        ITeklaService teklaService
        )
    {
        Comment = args.InitialComment;

        _contentDialogService = contentDialogService;
        _snackbarService = snackbarService;
        _teklaService = teklaService;
    }

    public IFluentCommand GetModelNameCommand =>
            Do(GetName);

    public IFluentCommand SetCommentOnSelectedElementsCommand =>
            Do(SetCommentOnSelected)
            .If(() => !string.IsNullOrEmpty(Comment));

    public IFluentCommand DeleteSelectedElementsCommand =>
            Do(DeleteSelected);

    private async Task DeleteSelected()
    {
        try
        {
            int objectCount = _teklaService.DeleteSelectedObjects();

            if (objectCount > 0)
            {
                _snackbarService.Show("Success", $"{objectCount} object(s) deleted.", ControlAppearance.Success);
            }
            else
            {
                _snackbarService.Show("Information", "No objects were selected.", ControlAppearance.Info);
            }
        }
        catch (Exception ex)
        {
            await _contentDialogService.ShowAlertAsync("Error",
             $"An error occurred while deleting objects: {ex.Message}",
             "OK");
        }
    }
    private async Task SetCommentOnSelected(CancellationToken cancellationToken)
    {
        try
        {
            string comment = Comment ?? string.Empty;
            int objectCount = _teklaService.SetCommentOnSelectedObjects(comment, cancellationToken);

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
            throw;
        }
        catch (Exception ex)
        {
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
