using FFlow.Core;

namespace FFlow.Demo;

public class NoOpStep : FlowStep
{
    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        context.SetOutputFor<NoOpStep, object>(context.GetLastOutput<object>());
        return Task.CompletedTask;
    }
}