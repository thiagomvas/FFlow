namespace FFlow.Core;


/// <summary>
/// Represents the base class for all workflow steps in the FFlow library.
/// Provides a simplified mechanism for implementing <see cref="IFlowStep"/> by
/// delegating the step logic to the <see cref="ExecuteAsync"/> method.
/// </summary>
public abstract class FlowStep : IFlowStep, IRetryableFlowStep, ICompensableStep
{
    private IRetryPolicy? _retryPolicy;
    
    /// <inheritdoc />
    public Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        context.SetInputFor(this, context.GetLastOutput<object>());
        if (_retryPolicy != null)
        {
            return _retryPolicy.ExecuteAsync(() => ExecuteAsync(context, cancellationToken), cancellationToken);
        }
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

    public IRetryPolicy SetRetryPolicy(IRetryPolicy retryPolicy)
    {
        _retryPolicy = retryPolicy ?? throw new ArgumentNullException(nameof(retryPolicy));
        return _retryPolicy;
    }

    public virtual Task CompensateAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}