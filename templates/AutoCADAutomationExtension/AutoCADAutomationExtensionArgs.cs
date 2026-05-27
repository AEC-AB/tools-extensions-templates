namespace AutoCADAutomationExtension;

public class AutoCADAutomationExtensionArgs
{
    [TextField(Label = "Text input", ToolTip = "Sample tooltip")]
    [Required(ErrorMessage = "Text input is required.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Text input must be between 3 and 100 characters.")]
    public string TextInput { get; set; } = "Default input";

    [BooleanField(Label = "Show advanced options")]
    public bool ShowAdvanced { get; set; }

    [IntegerField(Label = "Retry count", ToolTip = "Number of retries for transient failures", MinimumValue = 0, MaximumValue = 10, Visibility = nameof(ShowAdvanced))]
    [Range(0, 10, ErrorMessage = "Retry count must be between 0 and 10.")]
    public int RetryCount { get; set; } = 3;
}
