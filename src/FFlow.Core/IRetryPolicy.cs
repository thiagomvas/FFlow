namespace FFlow.Core;

/// <summary>
/// Defines a retry policy for executing actions with retry logic.
/// </summary>
public interface IRetryPolicy
{
    /// <summary>
    /// Executes the specified action asynchronously, applying the retry policy.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ExecuteAsync(Func<Task> action, CancellationToken cancellationToken = default);
}