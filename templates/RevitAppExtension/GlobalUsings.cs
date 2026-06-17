global using System;
global using System.Linq;
global using System.ComponentModel;
global using System.ComponentModel.DataAnnotations;
global using System.Threading;
global using System.Threading.Tasks;
global using System.Collections.Generic;

global using Result = CW.Assistant.Extensions.Contracts.Result;
global using Visibility = System.Windows.Visibility;

global using RevitAppFramework;
global using RevitAppFramework.CQRS;
global using RevitAppFramework.Mvvm;

global using CW.Assistant.Extensions.Contracts;
global using CW.Assistant.Extensions.Contracts.Enums;
global using CW.Assistant.Extensions.Contracts.Collectors;
global using CW.Assistant.Extensions.Contracts.Attributes;
global using CW.Assistant.Extensions.Contracts.Fields;

global using CW.Assistant.Extensions.Revit;
global using CW.Assistant.Extensions.Revit.Attributes;
global using CW.Assistant.Extensions.Revit.Collectors;

global using Autodesk.Revit.DB;
global using Autodesk.Revit.UI;

global using MVVMFluent;
global using Wpf.Ui;
global using Wpf.Ui.Extensions;