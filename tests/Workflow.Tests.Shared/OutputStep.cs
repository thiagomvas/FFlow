using FFlow.Core;

namespace Workflow.Tests.Shared;

public class OutputStep : FlowStep
{
    public object Output { get; set; }

    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        context.SetOutputFor(this, Output);
        return Task.CompletedTask;
    }
}