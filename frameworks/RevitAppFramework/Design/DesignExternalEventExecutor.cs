namespace RevitAppFramework.Design;

internal sealed class DesignExternalEventExecutor : IExternalEventExecutor
{
    private global::System.IServiceProvider? _serviceProvider;

    public void SetServiceProvider(global::System.IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public async global::System.Threading.Tasks.Task<TResult?> ExecuteQueryAsync<TResult>(global::RevitAppFramework.CQRS.IQuery<TResult> query, global::System.Threading.CancellationToken cancellationToken)
    {
        var handlerType = typeof(global::RevitAppFramework.Design.IDesignQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
        var executeMethod = handlerType.GetMethod("Execute");

        var result = await ExecuteAsync(handlerType, handler => executeMethod?.Invoke(handler, [query, cancellationToken]));
        return result is TResult res ? res : default;
    }

    public async global::System.Threading.Tasks.Task ExecuteCommandAsync(object input, global::System.Threading.CancellationToken cancellationToken)
    {
        var handlerType = typeof(global::RevitAppFramework.Design.IDesignCommandHandler<>).MakeGenericType(input.GetType());
        var executeMethod = handlerType.GetMethod("Execute");
        if (executeMethod is null)
        {
            throw new global::System.InvalidOperationException($"Could not find Execute method in {handlerType.Name}");
        }
        await ExecuteAsync(handlerType, handler => executeMethod.Invoke(handler, [input, cancellationToken]));
    }

    public async global::System.Threading.Tasks.Task<T?> ExecuteAsync<T>(global::System.Type handlerType, global::System.Func<object, T> execute)
    {
        var cts = new global::System.Threading.Tasks.TaskCompletionSource<object?>();

        var handler = CreateHandler(handlerType);

        await global::System.Threading.Tasks.Task.Run(async () =>
        {
            try
            {
                await global::System.Threading.Tasks.Task.Delay(10); // Simulate some delay for async execution
                var result = execute(handler);
                cts.SetResult(result);
            }
            catch (global::System.Exception ex)
            {
                cts.SetException(ex);
            }
        }).ConfigureAwait(false);

        try
        {
            var result = await cts.Task;
            return (T?)result;
        }
        catch (global::System.Exception e)
        {
            if (e is global::System.Reflection.TargetInvocationException && e.InnerException is not null)
                e = e.InnerException;

            throw e;
        }
    }

    internal object CreateHandler(global::System.Type type)
    {
        object? handler = null;
        try
        {
            handler = _serviceProvider?.GetService(type);
        }
        catch (global::System.Exception)
        {
            // Ignore for now
        }

        return handler ?? throw new global::System.InvalidOperationException($"{type.Name} could not be created. Make sure it is registered in the service provider.");
    }
}
