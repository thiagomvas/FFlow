namespace FFlow.Core;

/// <summary>
/// Represents a step in a workflow that can be executed asynchronously.
/// </summary>
public interface IFlowStep
{
    /// <summary>
    /// Executes the step asynchronously within the given workflow context.
    /// </summary>
    /// <param name="context">The context in which the step is executed.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default);
}