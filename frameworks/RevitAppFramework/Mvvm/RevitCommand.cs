namespace RevitAppFramework.Mvvm;

public class RevitCommandFluent<TResult> : global::MVVMFluent.AsyncFluentCommand
{
    private readonly IExternalEventExecutor _externalEventExecutor;

    private global::System.Type? _inputType;
    private global::System.Func<object>? _getInput;
    private global::System.Action<TResult>? _onCompleted;

    public RevitCommandFluent(IExternalEventExecutor externalEventExecutor)
    {
        _externalEventExecutor = externalEventExecutor;
        SetExecute((_, ct) => ExecuteAsync(ct));
    }

    public void SetInput<TInput>(global::System.Func<TInput> getInput) where TInput : class
    {
        _inputType = typeof(TInput);
        _getInput = getInput;
    }

    public new global::RevitAppFramework.Mvvm.RevitCommandFluent<TResult> If(global::System.Func<bool> condition)
    {
        base.If(condition);
        return this;
    }

    public new global::RevitAppFramework.Mvvm.RevitCommandFluent<TResult> If(global::System.Func<object?, bool> canExecute)
    {
        base.If(canExecute);
        return this;
    }

    public global::RevitAppFramework.Mvvm.RevitCommandFluent<TResult> Then(global::System.Action<TResult> onCompleted)
    {
        _onCompleted = onCompleted;
        return this;
    }

    /// <summary>
    /// Specifies an action to handle exceptions during execution.
    /// </summary>
    /// <param name="handle">The action to handle exceptions.</param>
    /// <returns>The updated command instance.</returns>
    public new global::RevitAppFramework.Mvvm.RevitCommandFluent<TResult> Handle(global::System.Action<global::System.Exception> handle)
    {
        base.Handle(handle);
        return this;
    }

    /// <summary>
    /// Specifies an action to handle exceptions during execution.
    /// </summary>
    /// <param name="handle">The action to handle exceptions.</param>
    /// <returns>The updated command instance.</returns>
    public global::RevitAppFramework.Mvvm.RevitCommandFluent<TResult> Handle(global::System.Func<global::System.Exception, global::System.Threading.Tasks.Task> handle)
    {
        base.Handle(async e =>
        {
            try
            {
                await handle(e);
            }
            catch (System.Exception)
            {
                // Ignore
            }
        });
        return this;
    }

    /// <summary>
    /// Sets whether the execution should continue on the captured synchronization context.
    /// </summary>
    /// <param name="continueOnCapturedContext">Whether to continue on captured context.</param>
    /// <returns>The updated command instance.</returns>
    public new global::RevitAppFramework.Mvvm.RevitCommandFluent<TResult> ConfigureAwait(bool continueOnCapturedContext)
    {
        base.ConfigureAwait(continueOnCapturedContext);
        return this;
    }

    private async global::System.Threading.Tasks.Task<object?> ExecuteAsync(global::System.Threading.CancellationToken cancellationToken)
    {
        if (_getInput is null || _inputType is null)
        {
            throw new global::System.InvalidOperationException("Input is not set. Please ensure that the input is properly set before executing the command.");
        }

        if (_onCompleted is null)
        {
            await _externalEventExecutor.ExecuteCommandAsync(_getInput(), cancellationToken);
            return null;
        }

        var result = await _externalEventExecutor.ExecuteQueryAsync((global::RevitAppFramework.CQRS.IQuery<TResult>)_getInput(), cancellationToken) ?? throw new global::System.InvalidOperationException("Result is null");
        _onCompleted(result);
        return null;
    }
}
