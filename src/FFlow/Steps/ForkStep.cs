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
        var parentId = context.Id;

        var tasks = _forks.Select(f =>
        {
            var task = f().Build()
                .SetContext(context.Fork())
                .RunAsync(context.GetInput<object>(), cancellationToken);

            if (_forkStrategy == ForkStrategy.FireAndForget)
                ParallelStepTracker.Instance.AddTask(parentId, task);

            return task;
        });

        switch (_forkStrategy)
        {
            case ForkStrategy.FireAndForget:
                // fire-and-forget but still tracked
                _ = Task.WhenAll(tasks);
                break;
            case ForkStrategy.WaitForAll:
                await Task.WhenAll(tasks);
                break;
        }
    }
}