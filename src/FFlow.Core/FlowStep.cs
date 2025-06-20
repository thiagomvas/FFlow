namespace FFlow.Core;


/// <summary>
/// Represents the base class for all workflow steps in the FFlow library.
/// Provides a simplified mechanism for implementing <see cref="IFlowStep"/> by
/// delegating the step logic to the <see cref="ExecuteAsync"/> method.
/// </summary>
public abstract class FlowStep : IFlowStep
{
    /// <inheritdoc />
    public Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        return ExecuteAsync(context, cancellationToken);
    }
        
    /// <summary>
    /// When implemented in a derived class, contains the asynchronous logic
    /// for the workflow step.
    /// </summary>
    /// <param name="context">The current workflow context.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected abstract Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken);
}