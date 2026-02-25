namespace RevitAppFramework.CQRS;

public interface IQuery<out TResult> { };

public interface IQueryHandler<in TInput, out TResult> where TInput : global::RevitAppFramework.CQRS.IQuery<TResult>
{
    TResult Execute(TInput input, global::System.Threading.CancellationToken cancellationToken);
}
