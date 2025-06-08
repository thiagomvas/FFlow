using FFlow.Core;

namespace Workflow.Tests.Shared;

public class DelayedStep : IFlowStep
{
    public Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        return Task.Delay(1000, cancellationToken);
    }
}