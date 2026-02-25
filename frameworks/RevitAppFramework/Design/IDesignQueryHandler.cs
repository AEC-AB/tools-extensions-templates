namespace RevitAppFramework.Design;

public interface IDesignQueryHandler<in TInput, out TResult> where TInput : global::RevitAppFramework.CQRS.IQuery<TResult>
{
    TResult Execute(TInput input, global::System.Threading.CancellationToken cancellationToken);
}
