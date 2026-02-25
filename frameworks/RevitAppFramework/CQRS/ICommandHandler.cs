namespace RevitAppFramework.CQRS;

public interface ICommandHandler<TInput>
{
    void Execute(TInput input, global::System.Threading.CancellationToken cancellationToken);
}
