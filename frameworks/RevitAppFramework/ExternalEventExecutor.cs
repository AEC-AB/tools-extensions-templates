namespace RevitAppFramework;

public interface IExternalEventExecutor
{
    global::System.Threading.Tasks.Task<TResult?> ExecuteQueryAsync<TResult>(global::RevitAppFramework.CQRS.IQuery<TResult> query, global::System.Threading.CancellationToken cancellationToken);
    global::System.Threading.Tasks.Task ExecuteCommandAsync(object input, global::System.Threading.CancellationToken cancellationToken);
}

internal sealed class ExternalEventExecutor(AppExternalEventHandler handler, global::Autodesk.Revit.UI.ExternalEvent externalEvent) : IExternalEventExecutor
{
    private global::System.IServiceProvider? _serviceProvider;

    public void SetServiceProvider(global::System.IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public async global::System.Threading.Tasks.Task<TResult?> ExecuteQueryAsync<TResult>(global::RevitAppFramework.CQRS.IQuery<TResult> query, global::System.Threading.CancellationToken cancellationToken)
    {
        var handlerType = typeof(global::RevitAppFramework.CQRS.IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
        var executeMethod = handlerType.GetMethod("Execute");

        var result = await ExecuteAsync(handlerType, handler => executeMethod?.Invoke(handler, [query, cancellationToken]));
        return result is TResult res ? res : default;
    }

    public async global::System.Threading.Tasks.Task ExecuteCommandAsync(object input, global::System.Threading.CancellationToken cancellationToken)
    {
        var handlerType = typeof(global::RevitAppFramework.CQRS.ICommandHandler<>).MakeGenericType(input.GetType());
        var executeMethod = handlerType.GetMethod("Execute");
        if (executeMethod is null)
        {
            throw new global::System.InvalidOperationException($"Could not find Execute method in {handlerType.Name}");
        }
        await ExecuteAsync(handlerType, handler => executeMethod.Invoke(handler, [input, cancellationToken]));
    }

    public async global::System.Threading.Tasks.Task<T?> ExecuteAsync<T>(global::System.Type handlerType, global::System.Func<object, T> execute)
    {
        if (typeof(T).Namespace?.StartsWith("Autodesk") == true && global::System.Linq.Enumerable.Contains(typeof(T).GetInterfaces(), typeof(global::System.IDisposable)))
        {
            global::Autodesk.Revit.UI.TaskDialog.Show("Invalid return type", "You can not return Revit objects outside of the Revit context.");
            return default;
        }

        var cts = new global::System.Threading.Tasks.TaskCompletionSource<object?>();
        handler.Register(handlerType, context => execute(context), cts);
        externalEvent.Raise();
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
}
