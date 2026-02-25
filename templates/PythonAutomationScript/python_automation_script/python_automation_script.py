""" This is the script that will be executed by the Python Automation Script Assistant. """
from python_automation_script_args import args
from assistant import variables

# Todo write script here

# Get control values
control_value = args.text_input.value
print(control_value)

comboBox_value = args.combo_box.value
comboBox_key = args.combo_box.getKey()
print(comboBox_value)
print(comboBox_key)

# Get Assistant variables
variable1 = variables.get("VariableKey1")
print(variable1)

# Set Assistant variables
variables.set("VariableKey2", control_value)

print("Hello World!")
