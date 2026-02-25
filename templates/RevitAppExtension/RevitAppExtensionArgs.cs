namespace RevitAppExtension;

/// <summary>
/// Represents the inputs to an Assistant extension.
/// This class is used for defining the inputs required by the extension.
/// The properties in this class are parsed into UI elements in the Extension Task configuration in Assistant.
/// </summary>
public class RevitAppExtensionArgs
{
    [TextField(Label = "Initial comment", ToolTip = "Initial comment text shown in the app")]
    public string? InitialComment { get; set; }
}