using FFlow.Core;

namespace FFlow;

public class WorkflowContinuationStep : IFlowStep
{
    private readonly IWorkflowDefinition _workflowDefinition;
    
public WorkflowContinuationStep(IWorkflowDefinition workflowDefinition)
    {
        _workflowDefinition = workflowDefinition ?? throw new ArgumentNullException(nameof(workflowDefinition));
    }
    public Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        
        var workflow = _workflowDefinition.Build().SetContext(context);
        return workflow.RunAsync(context.GetLastInput<object>(), cancellationToken);
    }
}