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
    /// </remarks>
    [TextField(Label = "Text input", ToolTip = "Sample tooltip text that appears on hover")]
    public string TextInput { get; set; } = "Default input";
    
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