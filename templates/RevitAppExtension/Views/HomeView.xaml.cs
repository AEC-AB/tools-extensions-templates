using RevitAppExtension.ViewModels;
using System.Windows.Controls;

namespace RevitAppExtension.Views;

/// <summary>
/// Interaction logic for HomeView.xaml - The main content view displayed in the application.
/// </summary>
/// <remarks>
/// This view is responsible for displaying the home page of the application.
/// Following the MVVM pattern, this view:
/// 1. Defines the UI layout in XAML
/// 2. Binds to properties and commands from the HomeViewModel
/// 3. Contains no code-behind logic for Revit operations or business rules
/// 
/// The view includes UI elements for:
/// - Displaying the current document title
/// - Input field for entering comments
/// - Buttons for performing actions on the Revit model
///
/// All business logic and Revit API interactions are handled by the corresponding HomeViewModel.
/// </remarks>
public partial class HomeView : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HomeView"/> class.
    /// </summary>
    /// <remarks>
    /// The view automatically gets its DataContext set to the HomeViewModel
    /// through WPF-UI's automatic view resolution mechanism, which is configured
    /// in the Resources/ViewBindings.xaml file.
    /// </remarks>
    public HomeView()
    {
        InitializeComponent();
    }
}
