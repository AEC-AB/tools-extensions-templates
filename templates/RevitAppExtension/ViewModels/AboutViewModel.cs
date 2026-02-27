using System.Diagnostics;

namespace RevitAppExtension.ViewModels;

/// <summary>
/// ViewModel for the About view, providing information and functionality not requiring Revit API access.
/// This class demonstrates using the MVVM pattern for non-Revit-dependent operations.
/// </summary>
/// <remarks>
/// This ViewModel uses primary constructor syntax and demonstrates the simpler Do() pattern
/// for commands that don't require Revit API context, unlike the HomeViewModel which uses Send.
/// </remarks>
public class AboutViewModel(ViewModelBaseDeps dependencies, ISnackbarService snackbarService) : RevitViewModelBase(dependencies)
{
    /// <summary>
    /// Command to open the wiki documentation website.
    /// </summary>
    /// <remarks>
    /// This command demonstrates the Do() pattern for operations that don't require Revit context.
    /// Since opening a URL doesn't need Revit's API, we use a direct method call pattern
    /// rather than the Send pattern used for Revit operations.
    /// </remarks>
    public IFluentCommand ShowWikiCommand => Do(OpenWiki);
    
    /// <summary>
    /// Opens the wiki documentation in the default web browser.
    /// </summary>
    /// <remarks>
    /// This method shows how to:
    /// 1. Launch an external process from within a Revit extension
    /// 2. Handle exceptions using the snackbar service
    /// 3. Perform operations that don't require Revit API access
    /// </remarks>
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
            // Display error notification using the snackbar service
            snackbarService.Show("Error", $"Failed to open wiki: {ex.Message}", Wpf.Ui.Controls.ControlAppearance.Danger);
        }
    }
}
