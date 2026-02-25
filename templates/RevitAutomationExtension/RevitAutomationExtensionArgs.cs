//-----------------------------------------------------------------------------
// RevitAutomationExtensionArgs.cs
//
// This file defines the input parameters for the RevitAutomationExtension.
// The properties defined here become UI controls in the Assistant application
// when configuring and running this extension.
//
// DEVELOPER GUIDE:
// 1. Add properties with appropriate attributes to create UI controls
// 2. Use attribute decorations to customize the appearance and behavior
// 3. Set default values where appropriate
//-----------------------------------------------------------------------------

namespace RevitAutomationExtension;

/// <summary>
/// Represents the inputs to the RevitAutomationExtension.
/// This class defines all parameters that can be configured by users through the UI.
/// Each property will be transformed into a corresponding UI control in the Assistant application.
/// </summary>
/// <remarks>
/// To add new inputs:
/// 1. Define a public property with getter and setter
/// 2. Add appropriate attributes like [Description], [Required], etc.
/// 3. Set a sensible default value if appropriate
/// 
/// Common attributes include:
/// - [TextField(Label = "Label")] - Sets the visible label in the UI
/// - [TextField(ToolTip = "Help text")] - Adds tooltip help text
/// - [Required] and [Range] - Adds value validation
/// - Visibility = nameof(ShowAdvanced) - Conditional visibility based on another field
/// - [ControlType(ControlType.ComboBox)] - Sets a specific control type
/// - [RevitAutoFill(RevitAutoFillSource.Categories)] - Adds Revit data auto-fill
/// - [ValueCopyCollector(typeof(ValueCopyRevitCollector))] - Enables value copy functionality
/// </remarks>
public class RevitAutomationExtensionArgs
{
    /// <summary>
    /// A basic text input example.
    /// </summary>
    /// <remarks>
    /// This is a simple example of a string property that creates a text input field.
    /// The [TextField] attribute sets the visible label in the UI and provides tooltip text.
    /// Validation attributes enforce allowed input ranges and required values.
    /// </remarks>
    [TextField(Label = "Text input", ToolTip = "Sample tooltip text that appears on hover")]
    [Required(ErrorMessage = "Text input is required.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Text input must be between 3 and 100 characters.")]
    public string TextInput { get; set; } = "Default input";

    [BooleanField(Label = "Show advanced options")]
    public bool ShowAdvanced { get; set; }

    [IntegerField(Label = "Retry count", ToolTip = "Number of retries for transient failures", MinimumValue = 0, MaximumValue = 10, Visibility = nameof(ShowAdvanced))]
    [Range(0, 10, ErrorMessage = "Retry count must be between 0 and 10.")]
    public int RetryCount { get; set; } = 3;
    
    // Examples of other common parameter types (commented out):
    
    /*
    /// <summary>
    /// Example of a dropdown selection for Revit categories.
    /// </summary>
    [TextField(Label = "Select Category")]
    [ControlType(ControlType.ComboBox)]
    [RevitAutoFill(RevitAutoFillSource.Categories)]
    public string SelectedCategory { get; set; }
    
    /// <summary>
    /// Example of a numeric input parameter.
    /// </summary>
    [DoubleField(Label = "Offset Distance", ToolTip = "Enter the offset distance in millimeters")]
    public double OffsetDistance { get; set; } = 100.0;
    
    /// <summary>
    /// Example of a parameter for ValueCopy functionality.
    /// </summary>
    [TextField(Label = "Parameter Value Copy")]
    [ValueCopyCollector(typeof(ValueCopyRevitCollector))]
    public ValueCopy ValueCopy { get; set; }
    */
}
