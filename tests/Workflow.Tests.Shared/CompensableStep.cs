using FFlow.Core;

namespace Workflow.Tests.Shared;

public class CompensableStep : FlowStep
{
    public bool Executed { get; set; }
    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        Executed = true;
        context.SetOutputFor<CompensableStep, object>("executed");
        return Task.CompletedTask;
    }
    
    public override Task CompensateAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        Executed = false;
        context.SetOutputFor<CompensableStep, object>("compensated");
        return Task.CompletedTask;
    }
}