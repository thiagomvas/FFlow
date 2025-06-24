using FFlow.Core;

namespace Workflow.Tests.Shared;

public class TestStep : IFlowStep
{
    public int Increment { get; set; } = 1;
    public Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        // Simulate some work
        var counter = context.GetValue<int>("counter");
        context.SetValue("counter", counter + Increment);
        return Task.CompletedTask;
    }
}