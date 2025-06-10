using FFlow.Core;

namespace FFlow;

public class ForkStep : IFlowStep
{
    private readonly Func<IWorkflowBuilder>[] _forks;
    private readonly ForkStrategy _forkStrategy;
    public ForkStep(ForkStrategy strategy, Func<IWorkflowBuilder>[] forks)
    {
        _forks = forks;
        _forkStrategy = strategy;
    }
    
    public async Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        var tasks = _forks.Select(f => f().Build()
            .SetContext(context.Fork())
            .RunAsync(context.GetInput<object>(), cancellationToken));

        switch (_forkStrategy)
        {
            case ForkStrategy.FireAndForget:
                _ = Task.WhenAll(tasks);
                break;
            case ForkStrategy.WaitForAll:
                await Task.WhenAll(tasks);
                break;
        }
    }
}