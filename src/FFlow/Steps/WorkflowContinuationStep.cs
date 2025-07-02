using FFlow.Core;

namespace FFlow;

[StepName("Workflow Continuation")]
[StepTags("built-in")]
public class WorkflowContinuationStep : FlowStep
{
    private readonly IWorkflowDefinition _workflowDefinition;
    private IWorkflow _execution;
    public WorkflowContinuationStep(IWorkflowDefinition workflowDefinition)
    {
        _workflowDefinition = workflowDefinition ?? throw new ArgumentNullException(nameof(workflowDefinition));
    }

    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        _execution = _workflowDefinition.Build().SetContext(context);
        return _execution.RunAsync(context.GetLastInput<object>(), cancellationToken);
    }

    public override Task CompensateAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        return _execution.CompensateAsync(cancellationToken);
    }
}