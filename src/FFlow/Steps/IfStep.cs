using FFlow.Core;
using FFlow.Visualization;

namespace FFlow;

[StepName("If Condition")]
[StepTags("built-in")]
[SilentStep]
public class IfStep : FlowStep, IDescribableStep
{
    private readonly Func<IFlowContext, bool> _condition;
    private readonly IFlowStep _trueStep;
    private readonly IFlowStep? _falseStep;

    public IfStep(Func<IFlowContext, bool> condition, IFlowStep trueStep, IFlowStep? falseStep = null)
    {
        _condition = condition ?? throw new ArgumentNullException(nameof(condition));
        _trueStep = trueStep ?? throw new ArgumentNullException(nameof(trueStep));
        _falseStep = falseStep;
    }

    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (_condition == null) throw new InvalidOperationException("Condition must be set.");
        if (_trueStep == null) throw new InvalidOperationException("True step must be set.");

        cancellationToken.ThrowIfCancellationRequested();

        if (_condition(context))
        {
            return _trueStep.RunAsync(context, cancellationToken);
        }
        else
        {
            return _falseStep?.RunAsync(context, cancellationToken) ?? Task.CompletedTask;
        }
    }

    public override Task CompensateAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (_trueStep == null) throw new InvalidOperationException("True step must be set for compensation.");

        cancellationToken.ThrowIfCancellationRequested();
        bool res = _condition(context);
        if (res && _trueStep is ICompensableStep trueStep)
        {
            return trueStep.CompensateAsync(context, cancellationToken);
        }

        if (!res && _falseStep is ICompensableStep falseStep)
        {
            return falseStep.CompensateAsync(context, cancellationToken);
        }

        return Task.CompletedTask;
    }

    public WorkflowGraph Describe(string? rootId = null)
    {
        var graph = new WorkflowGraph();
        var metadata = StepMetadataRegistry.Instance.Value.GetMetadata(this.GetType());

        rootId ??= metadata.Id;
        var rootNode = new WorkflowNode(rootId, metadata.Name);
        graph.Nodes.Add(rootNode);

        var conditionNode = rootNode;

        var mergeNode = new WorkflowNode($"{rootId}_exit", "End If");
        graph.Nodes.Add(mergeNode);

        // TRUE BRANCH
        if (_trueStep is IDescribableStep trueDescribable)
        {
            var trueSubgraph = trueDescribable.Describe($"{rootId}_true");
            var (entryId, exitIds) = graph.Merge(trueSubgraph, $"{rootId}_true");

            graph.Edges.Add(new WorkflowEdge(conditionNode.Id, entryId, "True"));
            foreach (var exitId in exitIds)
            {
                graph.Edges.Add(new WorkflowEdge(exitId, mergeNode.Id));
            }
        }
        else
        {
            var inlineId = $"{rootId}_true_inline";
            graph.Nodes.Add(new WorkflowNode(inlineId, _trueStep.GetType().Name));
            graph.Edges.Add(new WorkflowEdge(conditionNode.Id, inlineId, "True"));
            graph.Edges.Add(new WorkflowEdge(inlineId, mergeNode.Id));
        }

        // FALSE BRANCH
        if (_falseStep is IDescribableStep falseDescribable)
        {
            var falseSubgraph = falseDescribable.Describe($"{rootId}_false");
            var (entryId, exitIds) = graph.Merge(falseSubgraph, $"{rootId}_false");

            graph.Edges.Add(new WorkflowEdge(conditionNode.Id, entryId, "False"));
            foreach (var exitId in exitIds)
            {
                graph.Edges.Add(new WorkflowEdge(exitId, mergeNode.Id));
            }
        }
        else if (_falseStep != null)
        {
            var inlineId = $"{rootId}_false_inline";
            graph.Nodes.Add(new WorkflowNode(inlineId, _falseStep.GetType().Name));
            graph.Edges.Add(new WorkflowEdge(conditionNode.Id, inlineId, "False"));
            graph.Edges.Add(new WorkflowEdge(inlineId, mergeNode.Id));
        }

        return graph;
    }
}