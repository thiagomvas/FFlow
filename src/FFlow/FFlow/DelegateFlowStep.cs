using FFlow.Core;

namespace FFlow;

public class DelegateFlowStep : IFlowStep
{
    private readonly Func<IFlowContext, Task> _action;

    public DelegateFlowStep(Func<IFlowContext, Task> action)
    {
        _action = action ?? throw new ArgumentNullException(nameof(action));
    }

    public Task RunAsync(IFlowContext context)
    {
        return _action(context);
    }
}