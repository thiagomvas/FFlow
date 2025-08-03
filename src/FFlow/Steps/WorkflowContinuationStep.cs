using FFlow.Core;
using FFlow.Extensions;
using FFlow.Visualization;

namespace FFlow;

[StepName("Workflow Continuation")]
[StepTags("built-in")]
public class WorkflowContinuationStep : FlowStep, IDescribableStep
{
    private readonly IWorkflowDefinition _workflowDefinition;
    private IWorkflow _execution;
    public WorkflowContinuationStep(IWorkflowDefinition workflowDefinition)
    {
        _workflowDefinition = workflowDefinition ?? throw new ArgumentNullException(nameof(workflowDefinition));
    }

    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);

        _execution = _workflowDefinition.Build().SetContext(context);
        return _execution.RunAsync(context.GetLastInput<object>(), cancellationToken);
    }

    public override Task CompensateAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        return _execution.CompensateAsync(cancellationToken);
    }

    public WorkflowGraph Describe(string? rootId = null)
    {
        var name = _workflowDefinition.MetadataStore.GetName() ?? _workflowDefinition.GetType().Name;
        var node = new WorkflowNode(rootId ?? $"workflow_continuation_{Guid.NewGuid()}", $"Workflow Continuation - {name}");
        
        var graph = new WorkflowGraph();
        graph.Nodes.Add(node);
        return graph;
    }
}