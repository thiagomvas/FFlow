using FFlow.Core;

namespace FFlow;

[StepName("Delegate")]
[StepTags("built-in")]
[SilentStep]
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

        ArgumentNullException.ThrowIfNull(context);
        if (_action == null) throw new InvalidOperationException("Action must be set.");
        
        return Task.Run(() => _action.Invoke(context, cancellationToken));
    }
}