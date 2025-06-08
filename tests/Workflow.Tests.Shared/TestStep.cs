using FFlow.Core;

namespace Workflow.Tests.Shared;

public class TestStep : IFlowStep
{
    public Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        // Simulate some work
        var counter = context.Get<int>("counter");
        context.Set("counter", counter + 1);
        return Task.CompletedTask;
    }
}