using FFlow.Core;

namespace FFlow;

[StepName("Builder")]
[StepTags("built-in")]
[SilentStep]
public class BuilderStep : ForwardingWorkflowBuilder, IFlowStep
{
    private readonly IWorkflowBuilder _workflowBuilder;
    private readonly IServiceProvider? _serviceProvider;

    public BuilderStep(IWorkflowBuilder workflowBuilder, IServiceProvider? provider = null)
    {
        _workflowBuilder = workflowBuilder;
        _serviceProvider = provider;
    }

    protected override IWorkflowBuilder Delegate => _workflowBuilder;
    public Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
        => _workflowBuilder.Build().SetContext(context).RunAsync(context.GetLastOutput<object>(), cancellationToken);
}
