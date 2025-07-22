using System.Collections;
using FFlow.Core;
using FFlow.Visualization;

namespace FFlow;

[StepName("For Each")]
[StepTags("built-in")]
[SilentStep]
public class ForEachStep : ForEachStep<object>
{
    public ForEachStep(Func<IFlowContext, IEnumerable> itemsSelector, IFlowStep itemAction)
        : base(context => itemsSelector(context).Cast<object>(), itemAction)
    {
    }
}

public class ForEachStep<T> : IFlowStep, IDescribableStep
{
    private readonly Func<IFlowContext, IEnumerable<T>> _itemsSelector;
    private readonly IFlowStep _itemAction;
    
    public ForEachStep(Func<IFlowContext, IEnumerable<T>> itemsSelector, IFlowStep itemAction)
    {
        _itemsSelector = itemsSelector ?? throw new ArgumentNullException(nameof(itemsSelector));
        _itemAction = itemAction ?? throw new ArgumentNullException(nameof(itemAction));
    }

    public async Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (context == null) throw new ArgumentNullException(nameof(context));
        if (_itemsSelector == null) throw new InvalidOperationException("Items selector must be set.");
        if (_itemAction == null) throw new InvalidOperationException("Item action must be set.");

        var items = _itemsSelector(context);
        if (items == null) return;

        foreach (var item in items)
        {
            context.SetInputFor(_itemAction, item);
            await _itemAction.RunAsync(context, cancellationToken);
        }
    }

    public WorkflowGraph Describe(string? rootId = null)
    {
        var graph = new WorkflowGraph();
        var metadata = StepMetadataRegistry.Instance.Value.GetMetadata(this.GetType());

        var genericType = typeof(T).Name;
        var displayLabel = $"ForEach<{genericType}>";

        rootId ??= metadata.Id;

        var rootNode = new WorkflowNode(rootId, displayLabel);
        graph.Nodes.Add(rootNode);

        var iteratorNode = rootNode;

        var doneNode = new WorkflowNode($"{rootId}_done", "Done");
        graph.Nodes.Add(doneNode);

        if (_itemAction is IDescribableStep describable)
        {
            var subgraph = describable.Describe();
            var (entryId, exitIds) = graph.Merge(subgraph, $"{rootId}_item");

            graph.Edges.Add(new WorkflowEdge(iteratorNode.Id, entryId, "Each item"));

            foreach (var exitId in exitIds)
            {
                // Loop back to iterator
                graph.Edges.Add(new WorkflowEdge(exitId, iteratorNode.Id, "Next"));
            }
        }
        else
        {
            var actionNodeId = $"{rootId}_item_action";
            graph.Nodes.Add(new WorkflowNode(actionNodeId, _itemAction.GetType().Name));
            graph.Edges.Add(new WorkflowEdge(iteratorNode.Id, actionNodeId, "Each item"));

            // Loop back
            graph.Edges.Add(new WorkflowEdge(actionNodeId, iteratorNode.Id, "Next"));
        }

        graph.ContinueFromId = rootId;
        graph.ExitEdgeLabel = "Done";

        return graph;
    }
}