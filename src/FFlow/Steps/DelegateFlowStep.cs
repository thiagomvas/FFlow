using FFlow.Core;

namespace FFlow;

public class DelegateFlowStep : IFlowStep
{
    private readonly AsyncFlowAction _action;

    public DelegateFlowStep(AsyncFlowAction action)
    {
        _action = action ?? throw new ArgumentNullException(nameof(action));
    }

    public Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (context == null) throw new ArgumentNullException(nameof(context));
        if (_action == null) throw new InvalidOperationException("Action must be set.");
        
        return _action(context, cancellationToken);
    }
}