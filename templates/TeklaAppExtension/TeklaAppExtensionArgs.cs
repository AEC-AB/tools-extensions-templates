namespace TeklaAppExtension;

/// <summary>
/// Represents the inputs to an Assistant extension.
/// This class is used for defining the inputs required by the extension.
/// The properties in this class are parsed into UI elements in the Extension Task configuration in Assistant.
/// </summary>
public class TeklaAppExtensionArgs
{
    /// <summary>
    /// Gets or sets the initial comment text that will be used in the application.
    /// This value is automatically populated from user input in the Extension Task UI.
    /// It will be used as the default comment when setting comments on Tekla objects.
    /// </summary>
    public string InitialComment { get; set; } = "Default comment";
}