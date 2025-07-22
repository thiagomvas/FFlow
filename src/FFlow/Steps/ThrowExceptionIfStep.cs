using FFlow.Core;
using FFlow.Visualization;

namespace FFlow;

[StepName("Throw If")]
[StepTags("built-in")]
[SilentStep]
public class ThrowExceptionIfStep : IFlowStep, IDescribableStep
{
    private readonly Func<IFlowContext, bool> _condition;
    private readonly Exception _exception;
    
    
    public ThrowExceptionIfStep(Func<IFlowContext, bool> condition, Exception exception)
    {
        _condition = condition ?? throw new ArgumentNullException(nameof(condition));
        _exception = exception ?? throw new ArgumentNullException(nameof(exception));
    }
    
    public ThrowExceptionIfStep(Func<IFlowContext, bool> condition, string message)
    {
        _condition = condition ?? throw new ArgumentNullException(nameof(condition));
        _exception = new Exception(message ?? throw new ArgumentNullException(nameof(message)));
    }
    public Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (!_condition(context)) return Task.CompletedTask;
        
        context.SetSingleValue(_exception);
        return Task.FromException(_exception);

    }

    public WorkflowGraph Describe(string? rootId = null)
    {
        var graph = new WorkflowGraph();
        var metadata = StepMetadataRegistry.Instance.Value.GetMetadata(this.GetType());
        
        rootId ??= metadata.Id;
        var rootNode = new WorkflowNode(rootId, metadata.Name);
        graph.Nodes.Add(rootNode);
        
        var exceptionNode = new WorkflowNode($"{rootId}_exception", _exception.GetType().Name);
        graph.Nodes.Add(exceptionNode);

        var edge = new WorkflowEdge(rootId, exceptionNode.Id, "Condition is true");
        graph.Edges.Add(edge);

        graph.ContinueFromId = rootNode.Id;
        graph.ContinueFromLabel = "Condition is false";
        
        return graph;
    }
}