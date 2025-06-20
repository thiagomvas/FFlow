using FFlow.Core;

namespace FFlow;

public class DelegateFlowStep : FlowStep
{
    private readonly AsyncFlowAction _action;

    public DelegateFlowStep(AsyncFlowAction action)
    {
        _action = action ?? throw new ArgumentNullException(nameof(action));
    }

    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (context == null) throw new ArgumentNullException(nameof(context));
        if (_action == null) throw new InvalidOperationException("Action must be set.");
        
        return _action(context, cancellationToken);
    }
}