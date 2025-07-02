namespace FFlow.Core;

/// <summary>
/// A base class for implementing decorators for flow steps.
/// </summary>
public abstract class BaseStepDecorator : IFlowStep
{
    protected readonly IFlowStep _innerStep;

    protected BaseStepDecorator(IFlowStep innerStep)
    {
        _innerStep = innerStep ?? throw new ArgumentNullException(nameof(innerStep));
    }

    public virtual async Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        await _innerStep.RunAsync(context, cancellationToken);
    }
}