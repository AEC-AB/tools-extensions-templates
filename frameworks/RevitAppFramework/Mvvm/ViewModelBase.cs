namespace RevitAppFramework.Mvvm;

public abstract class RevitViewModelBase(global::RevitAppFramework.Mvvm.ViewModelBaseDeps dependencies) : global::MVVMFluent.ViewModelBase
{
    private global::RevitAppFramework.Mvvm.RevitCommandFluent<TOutput> CreateCommand<TInput, TOutput>(global::System.Func<TInput> getInput, string caller) where TInput : class
    {
        if (caller == ".ctor")
        {
            throw new global::System.ArgumentException("Command name must be provided when command is created in the constructor. Provide a value to the 'Caller' argument or use expression syntax (public IAsyncCommandFluent MyCommand => Send<...).");
        }
        var newCommand = new global::RevitAppFramework.Mvvm.RevitCommandFluent<TOutput>(dependencies.ExternalEventExecutor);
        newCommand.SetInput(() => getInput());
        newCommand.Handle(async e => await HandleException(e));
        _commandStore.Add(caller, newCommand);
        return newCommand;
    }

    private async global::System.Threading.Tasks.Task HandleException(System.Exception e)
    {
        string innerExceptionDetails = "";
        var currentException = e.InnerException;
        int depth = 1;

        while (currentException != null)
        {
            innerExceptionDetails += $"Inner Exception {depth}: \n{currentException.Message}\n\n";
            currentException = currentException.InnerException;
            depth++;
        }
        var content = new global::Wpf.Ui.Controls.TextBox { IsReadOnly = true, TextWrapping = System.Windows.TextWrapping.Wrap, Text = $"{e.Message}\n\n{e.StackTrace}\n\n{innerExceptionDetails}" };

        var dialog = new global::Wpf.Ui.SimpleContentDialogCreateOptions()
        {
            Title = "Operation Failed",
            Content = content,
            CloseButtonText = "Cancel"
        };

        try
        {
            await Wpf.Ui.Extensions.ContentDialogServiceExtensions.ShowSimpleDialogAsync(dependencies.ContentDialogService, dialog);
        }
        catch (System.Exception)
        {
            // Ignore
        }
    }

    protected global::RevitAppFramework.Mvvm.RevitCommandFluent<object?> Send<TInput>([global::System.Runtime.CompilerServices.CallerMemberName] string? caller = null) where TInput : class, new()
    {
        if (caller is null)
        {
            throw new global::System.ArgumentNullException(nameof(caller));
        }

        if (!_commandStore.TryGetValue(caller, out var relayCommand))
        {
            relayCommand = CreateCommand<TInput, object?>(() => new TInput(), caller);
        }

        return (RevitCommandFluent<object?>)relayCommand;
    }

    protected global::RevitAppFramework.Mvvm.RevitCommandFluent<TOutput> Send<TInput, TOutput>([global::System.Runtime.CompilerServices.CallerMemberName] string? caller = null) where TInput : class, global::RevitAppFramework.CQRS.IQuery<TOutput>, new()
    {
        if (caller is null)
        {
            throw new global::System.ArgumentNullException(nameof(caller));
        }

        if (!_commandStore.TryGetValue(caller, out var relayCommand))
        {
            relayCommand = CreateCommand<TInput, TOutput>(() => new TInput(), caller);
        }

        return (global::RevitAppFramework.Mvvm.RevitCommandFluent<TOutput>)relayCommand;
    }

    protected global::RevitAppFramework.Mvvm.RevitCommandFluent<object?> Send<TInput>(global::System.Func<TInput> getInput, [global::System.Runtime.CompilerServices.CallerMemberName] string? caller = null) where TInput : class
    {
        if (caller is null)
        {
            throw new global::System.ArgumentNullException(nameof(caller));
        }

        if (!_commandStore.TryGetValue(caller, out var relayCommand))
        {
            relayCommand = CreateCommand<TInput, object?>(getInput, caller);
        }

        return (RevitCommandFluent<object?>)relayCommand;
    }


    protected global::RevitAppFramework.Mvvm.RevitCommandFluent<TOutput> Send<TInput, TOutput>(global::System.Func<TInput> getInput, [global::System.Runtime.CompilerServices.CallerMemberName] string? caller = null) where TInput : class, global::RevitAppFramework.CQRS.IQuery<TOutput>
    {
        if (caller is null)
        {
            throw new global::System.ArgumentNullException(nameof(caller));
        }

        if (!_commandStore.TryGetValue(caller, out var relayCommand))
        {
            relayCommand = CreateCommand<TInput, TOutput>(getInput, caller);
        }

        return (global::RevitAppFramework.Mvvm.RevitCommandFluent<TOutput>)relayCommand;
    }
}
