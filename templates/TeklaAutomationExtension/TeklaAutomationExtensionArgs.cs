namespace TeklaAutomationExtension;

/// <summary>
/// Represents the inputs to an Assistant extension.
/// This class is used for defining the inputs required by the extension.
/// The properties in this class are parsed into UI elements in the Extension Task configuration in Assistant.
/// </summary>
public class TeklaAutomationExtensionArgs
{
    [TextField(Label = "Text input", ToolTip = "Sample tooltip")]
    public string TextInput { get; set; } = "Default input";
}