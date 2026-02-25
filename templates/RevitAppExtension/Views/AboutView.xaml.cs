using System.Windows.Controls;

namespace RevitAppExtension.Views;

/// <summary>
/// Interaction logic for AboutView.xaml - The about page of the application.
/// </summary>
/// <remarks>
/// This view is responsible for displaying information about the application such as:
/// - Version information
/// - Documentation links
/// - Copyright notices
/// - Developer information
/// 
/// Following the MVVM pattern, this view:
/// 1. Defines the UI layout in XAML
/// 2. Binds to properties and commands from the AboutViewModel
/// 3. Contains no business logic
///
/// This serves as an example of a simple view that doesn't require Revit API interaction.
/// </remarks>
public partial class AboutView : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AboutView"/> class.
    /// </summary>
    /// <remarks>
    /// The view automatically gets its DataContext set to the AboutViewModel
    /// through WPF-UI's automatic view resolution mechanism, which is configured
    /// in the Resources/ViewBindings.xaml file.
    /// </remarks>
    public AboutView()
    {
        InitializeComponent();
    }
}
