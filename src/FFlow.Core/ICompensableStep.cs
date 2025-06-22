namespace FFlow.Core;

/// <summary>
/// Represents a step in a workflow that supports compensation logic.
/// Compensation is used to undo the effects of a previously completed step,
/// typically in response to a failure in a later step of the workflow.
/// </summary>
public interface ICompensableStep
{
    /// <summary>
    /// Executes the compensation logic for the step, reversing any changes
    /// made during its execution.
    /// </summary>
    /// <param name="context">The workflow context providing shared data and services.</param>
    /// <param name="cancellationToken">A token to observe for cancellation.</param>
    /// <returns>A task representing the asynchronous compensation operation.</returns>
    Task CompensateAsync(IFlowContext context, CancellationToken cancellationToken = default);
}
