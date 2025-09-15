using System.Collections;
using FFlow.Core;
using FFlow.Visualization;

namespace FFlow;

internal class ForEachStep<TItem, TStepIterator> : FlowStep, IDescribableStep
    where TStepIterator : IFlowStep
{
    private readonly Func<IFlowContext, IEnumerable<TItem>> _itemsSelector;
    private readonly Action<TItem>? _executor;
    private readonly Action<TItem, TStepIterator>? _configurator;
    private readonly Func<TStepIterator> _stepFactory;

    public ForEachStep(Func<IFlowContext, IEnumerable<TItem>> itemsSelector, Action<TItem> executor)
    {
        _itemsSelector = itemsSelector ?? throw new ArgumentNullException(nameof(itemsSelector));
        _executor = executor ?? throw new ArgumentNullException(nameof(executor));
    }

    public ForEachStep(Func<IFlowContext, IEnumerable<TItem>> itemsSelector, Func<TStepIterator> factory,
        Action<TItem, TStepIterator> configurator)
    {
        _itemsSelector = itemsSelector ?? throw new ArgumentNullException(nameof(itemsSelector));
        _configurator = configurator ?? throw new ArgumentNullException(nameof(configurator));
        _stepFactory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    protected override async Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        var items = _itemsSelector(context);
        if (items is null || !items.Any()) return;

        foreach (var item in items)
        {
            if (_executor != null)
            {
                _executor(item);
            }
            else if (_configurator != null)
            {
                var step = _stepFactory();
                _configurator(item, step);
                await step.RunAsync(context);
            }
            else
            {
                throw new InvalidOperationException("Either executor or configurator must be provided.");
            }
        }
    }


    public WorkflowGraph Describe(string? rootId = null)
    {
        var graph = new WorkflowGraph();
        var metadata = StepMetadataRegistry.Instance.Value.GetMetadata(this.GetType());

        var genericType = typeof(TItem).Name;
        var displayLabel = $"ForEach<{genericType}>";

        rootId ??= metadata.Id;

        var rootNode = new WorkflowNode(rootId, displayLabel);
        graph.Nodes.Add(rootNode);

        var iteratorNode = rootNode;

        if (_configurator != null && _stepFactory != null)
        {
            // Create a prototype step from the factory to describe it
            var prototypeStep = _stepFactory();
            if (prototypeStep is IDescribableStep describable)
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
                graph.Nodes.Add(new WorkflowNode(actionNodeId, prototypeStep.GetType().Name));
                graph.Edges.Add(new WorkflowEdge(iteratorNode.Id, actionNodeId, "Each item"));
                graph.Edges.Add(new WorkflowEdge(actionNodeId, iteratorNode.Id, "Next"));
            }
        }
        else if (_executor != null)
        {
            // No describable step; represent the executor as a single node
            var actionNodeId = $"{rootId}_item_action";
            graph.Nodes.Add(new WorkflowNode(actionNodeId, _executor.GetType().Name));
            graph.Edges.Add(new WorkflowEdge(iteratorNode.Id, actionNodeId, "Each item"));
            graph.Edges.Add(new WorkflowEdge(actionNodeId, iteratorNode.Id, "Next"));
        }

        graph.ContinueFromId = rootId;
        graph.ExitEdgeLabel = "Done";

        return graph;
    }
}

internal class ForEachStep<TItem> : ForEachStep<TItem, FlowStep>
{
    public ForEachStep(Func<IFlowContext, IEnumerable<TItem>> itemsSelector, Action<TItem> executor)
        : base(itemsSelector, executor)
    {
    }

    public ForEachStep(Func<IFlowContext, IEnumerable<TItem>> itemsSelector, Action<TItem, FlowStep> configurator)
        : base(itemsSelector, null, configurator)
    {
    }
}

internal class ForEachStep : ForEachStep<object, FlowStep>
{
    public ForEachStep(Func<IFlowContext, IEnumerable<object>> itemsSelector, Action<object> executor)
        : base(context => itemsSelector(context), executor)
    {
    }

    public ForEachStep(Func<IFlowContext, IEnumerable<object>> itemsSelector, Action<object, FlowStep> configurator)
        : base(context => itemsSelector(context), null, configurator)
    {
    }
}