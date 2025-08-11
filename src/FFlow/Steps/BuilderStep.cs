using FFlow.Core;

namespace FFlow;

[StepName("Builder")]
[StepTags("built-in")]
[SilentStep]
public class BuilderStep : FlowStep
{
    private readonly WorkflowBuilderBase _workflowBuilder;
    private readonly IServiceProvider? _serviceProvider;

    public BuilderStep(WorkflowBuilderBase workflowBuilder, IServiceProvider? provider = null)
    {
        _workflowBuilder = workflowBuilder;
        _serviceProvider = provider;
    }

    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        return _workflowBuilder.Build().SetContext(context).RunAsync(context.GetLastOutput<object>(), cancellationToken);
    }
}
