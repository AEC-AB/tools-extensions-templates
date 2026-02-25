""" This is the main script to be executed """
from python_revit_automation_script_args import args
from assistant import variables
from assistant.integrations.revit import revit
from Autodesk.Revit.DB import Transaction
from Autodesk.Revit.Exceptions import ApplicationException

# Get Revit document
doc = revit.doc

# Get control values
control_value = args.control.value
print(control_value)

combo_box_value = args.combo_box.value
combo_box_key = args.combo_box.getKey()
print(combo_box_value)
print(combo_box_key)

# Get Assistant variables
variable1 = variables.get('VariableKey1')
print(variable1)

try:
    trans = Transaction(doc, 'Run python_revit_automation_script')
    trans.Start()

    # Todo write script here

    trans.Commit()
except ApplicationException as e:
    trans.RollBack()
finally:
    trans.Dispose()

# Set Assistant variables
variables.set('VariableKey2', control_value)
