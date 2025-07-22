using FFlow.Core;
using FFlow.Extensions;
using FFlow.Visualization;

namespace FFlow;

[StepName("Switch Case")]
[StepTags("built-in")]
[SilentStep]
public class SwitchStep : FlowStep, IDescribableStep
{
    private readonly List<SwitchCase> _switches = new List<SwitchCase>();
    private IWorkflow _execution;
    internal SwitchStep(List<SwitchCase> cases)
    {
        _switches = cases;
    }
    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if(_switches == null) throw new ArgumentNullException(nameof(_switches));
        cancellationToken.ThrowIfCancellationRequested();
        
        foreach (var switchCase in _switches)
        {
            if (switchCase.Condition == null)
                throw new InvalidOperationException("Condition must be set for each switch case.");
            if (switchCase.Builder == null)
                throw new InvalidOperationException("Builder must be set for each switch case.");
            if (switchCase.Condition(context))
            {
                _execution = switchCase.Builder!.Build();
                return _execution.SetContext(context).RunAsync(context, cancellationToken);
            }
        }
        
        return Task.CompletedTask;
    }

    public override Task CompensateAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (_switches == null) throw new ArgumentNullException(nameof(_switches));
        cancellationToken.ThrowIfCancellationRequested();
        
        foreach (var switchCase in _switches)
        {
            if (switchCase.Builder == null)
                throw new InvalidOperationException("Builder must be set for each switch case.");
            if (switchCase.Condition(context))
            {
                return _execution.CompensateAsync(cancellationToken);
            }
        }
        
        return Task.CompletedTask;
    }

    public WorkflowGraph Describe(string? rootId = null)
    {
        var graph = new WorkflowGraph();
        var metadata = StepMetadataRegistry.Instance.Value.GetMetadata<SwitchStep>();
        rootId ??= metadata.Id;

        var root = new WorkflowNode(rootId, "Switch");
        graph.Nodes.Add(root);

        for (int i = 0; i < _switches.Count; i++)
        {
            var switchCase = _switches[i];
            var caseSubgraph = switchCase.Builder?.Describe();

            var (entryNodeId, _) = graph.Merge(caseSubgraph, $"{rootId}_case_{i}");
            graph.Edges.Add(new WorkflowEdge(rootId, entryNodeId, switchCase.Name));
        }

        graph.ContinueFromId = rootId;
        graph.ContinueFromLabel = "Continue with";
        
        return graph;
    }
}