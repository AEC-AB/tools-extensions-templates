""" The args module is used to define the input arguments for the script. """
from assistant import controls
from assistant.autofill import _AutofillBase


class MyAutofill(_AutofillBase):
    """A class to define the autofill collector."""

    def on_collect(self):
        # type: () -> dict

        result = {}
        result["Key1"] = "Value 1"
        result["Key2"] = "Value 2"

        return result


class Args:
    """A class to define the arguments for the script."""

    def __init__(self):
        self.text_input = controls.Text(
            "TextInput", defaultValue="Default value", tooltip="Control Tooltip"
        )
        self.combo_box = controls.ComboBox("My comboBox").itemsCollector(MyAutofill())


args = Args()
