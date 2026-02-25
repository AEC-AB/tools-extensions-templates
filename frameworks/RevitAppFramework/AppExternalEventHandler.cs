namespace RevitAppFramework;

public class AppExternalEventHandler : global::Autodesk.Revit.UI.IExternalEventHandler
{
    private class QueueItem(global::System.Type commandType, global::System.Func<object, object?> action, global::System.Threading.Tasks.TaskCompletionSource<object?> tcs)
    {
        public global::System.Type CommandType { get; } = commandType;
        public global::System.Func<object, object?> Action { get; } = action;
        public global::System.Threading.Tasks.TaskCompletionSource<object?> TaskCompletionSource { get; } = tcs;
    }

    private string? _identifier;
    private global::System.IServiceProvider? _serviceProvider;
    private readonly global::System.Collections.Generic.Queue<global::RevitAppFramework.AppExternalEventHandler.QueueItem> _queue = new();

    public void Execute(global::Autodesk.Revit.UI.UIApplication app)
    {
        if (_serviceProvider == null)
        {
            global::Autodesk.Revit.UI.TaskDialog.Show("Error", "Service provider is not set.");
            return;
        }
        using var scope = global::Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.CreateScope(_serviceProvider);
        while (_queue.Count > 0)
        {
            var item = _queue.Dequeue();
            try
            {
                var command = global::Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService(_serviceProvider, item.CommandType);
                var result = item.Action(command);
                item.TaskCompletionSource.SetResult(result);
            }
            catch (global::System.Exception ex)
            {
                item.TaskCompletionSource.SetException(ex);
            }
        }
    }

    public string GetName() => _identifier ??= GetType().Name;

    internal void Register(global::System.Type commandType, global::System.Func<object, object?> execute, global::System.Threading.Tasks.TaskCompletionSource<object?> tcs)
    {
        _queue.Enqueue(new global::RevitAppFramework.AppExternalEventHandler.QueueItem(commandType, execute, tcs));
    }

    internal void SetServiceProvider(global::System.IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
}