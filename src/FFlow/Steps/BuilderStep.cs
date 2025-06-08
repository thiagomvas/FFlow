using FFlow.Core;

namespace FFlow;

public class BuilderStep : ForwardingWorkflowBuilder, IFlowStep
{
    private readonly IWorkflowBuilder _workflowBuilder;

    public BuilderStep(IWorkflowBuilder workflowBuilder)
    {
        _workflowBuilder = workflowBuilder;
    }

    protected override IWorkflowBuilder Delegate => _workflowBuilder;

    public Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
        => _workflowBuilder.Build().SetContext(context).RunAsync(context.GetInput<object>(), cancellationToken);
}
