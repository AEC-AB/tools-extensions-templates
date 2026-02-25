using MVVMFluent;
using System.Diagnostics;
using Wpf.Ui;
using Wpf.Ui.Extensions;
using Wpf.Ui.Controls;

namespace TeklaAppExtension.ViewModels;

public class AboutViewModel(ISnackbarService snackbarService) : ViewModelBase
{
    public IFluentCommand ShowWikiCommand => Do(OpenWiki);

    private void OpenWiki()
    {
        var wikiUrl = "https://wiki.toolsbyaec.com/en/Assistant/Develop/DotnetExtension/TeklaAppExtension";
        var startInfo = new ProcessStartInfo
        {
            FileName = wikiUrl,
            UseShellExecute = true,
            Verb = "open"
        };

        try
        {
            Process.Start(startInfo);
        }
        catch (Exception ex)
        {
            // Display error notification using the snackbar service
            snackbarService.Show("Error", $"Failed to open wiki: {ex.Message}", ControlAppearance.Danger);
        }
    }
}
