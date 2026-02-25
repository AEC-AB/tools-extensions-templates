""" The args module is used to define the input arguments for the script. """
from assistant import controls
from assistant.integrations.revit.autofill import _RevitAutofillBase
from assistant.integrations.revit.wrapper import Wrapper

class MyAutofill(_RevitAutofillBase):
    """An example autofill collector to get parameter elements in current model.

    Notes:
        When collection is invoked on_collect() function is called.
        That function returns a dict with elements
        where the key with the UniqueId and a Value is the Name

    """

    def on_collect(self, revit):
        # type: (Wrapper) -> dict
        from Autodesk.Revit.DB import FilteredElementCollector, ParameterElement
        from Autodesk.Revit.Exceptions import ApplicationException

        result = {}
        doc = revit.doc
        if doc is None:
            return result

        collector = FilteredElementCollector(doc).OfClass(ParameterElement)
        with collector.GetElementIterator() as element_iterator:
            while element_iterator.MoveNext():
                try:
                    with element_iterator.Current as parameter_element:
                        if parameter_element is None:
                            continue

                        definition = parameter_element.GetDefinition()
                        result[
                            parameter_element.Id.IntegerValue.ToString()
                        ] = definition.Name
                except ApplicationException:
                    continue
        return result


class Args(object):
    """The Args class is used to define the input arguments for the script."""

    def __init__(self):
        self.control = controls.Text(
            "Control name", defaultValue="Default value", tooltip="Control ToolTip"
        )
        self.combo_box = controls.ComboBox("My comboBox").itemsCollector(MyAutofill())


args = Args()
