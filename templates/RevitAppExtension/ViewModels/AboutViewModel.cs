using System.Diagnostics;

namespace RevitAppExtension.ViewModels;

public class AboutViewModel(ViewModelBaseDeps dependencies, ISnackbarService snackbarService) : RevitViewModelBase(dependencies)
{
    public IFluentCommand ShowWikiCommand => Do(OpenWiki);

    private void OpenWiki()
    {
        var wikiUrl = "https://toolswiki.aec.se/en/Assistant/Develop/DotnetExtension/RevitAppExtension";
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
            snackbarService.Show("Error", $"Failed to open wiki: {ex.Message}", Wpf.Ui.Controls.ControlAppearance.Danger);
        }
    }
}
