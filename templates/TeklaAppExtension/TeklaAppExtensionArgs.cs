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
    [TextField(Label = "Initial comment", ToolTip = "Initial comment text shown in the app")]
    [Required(ErrorMessage = "Initial comment is required.")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Initial comment must be between 3 and 200 characters.")]
    public string InitialComment { get; set; } = "Default comment";

    [BooleanField(Label = "Show advanced options")]
    public bool ShowAdvanced { get; set; }

    [IntegerField(Label = "Retry count", ToolTip = "Number of retries for transient failures", MinimumValue = 0, MaximumValue = 10, Visibility = nameof(ShowAdvanced))]
    [Range(0, 10, ErrorMessage = "Retry count must be between 0 and 10.")]
    public int RetryCount { get; set; } = 3;
}