namespace FFlow.Core;

public interface IRetryPolicy
{
    Task ExecuteAsync(Func<Task> action, CancellationToken cancellationToken = default);
    
}