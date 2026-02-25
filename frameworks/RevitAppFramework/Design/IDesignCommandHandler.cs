namespace RevitAppFramework.Design;

public interface IDesignCommandHandler<TInput>
{
    void Execute(TInput input, global::System.Threading.CancellationToken cancellationToken);
}
