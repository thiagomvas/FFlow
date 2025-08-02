using FFlow.Core;
using FFlow.Extensions;
using FFlow.Visualization;

namespace FFlow;

[StepName("Fork")]
[StepTags("built-in")]
[SilentStep]
public class ForkStep : IFlowStep, IDescribableStep
{
    private readonly Func<IWorkflow>[] _forks;
    private readonly ForkStrategy _forkStrategy;

    public ForkStep(ForkStrategy strategy, Func<IWorkflow>[] forks)
    {
        _forks = forks;
        _forkStrategy = strategy;
    }

    public async Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        var tasks = _forks.Select(f =>
        {
            var task = f()
                .SetContext(context.Fork())
                .RunAsync(context.GetLastInput<object>(), cancellationToken);

            if (_forkStrategy == ForkStrategy.FireAndForget)
                ParallelStepTracker.Instance.AddTask(context.GetId(), task);

            return task;
        });

        switch (_forkStrategy)
        {
            case ForkStrategy.FireAndForget:
                _ = Task.WhenAll(tasks);
                break;
            case ForkStrategy.WaitForAll:
                await Task.WhenAll(tasks);
                break;
        }
    }

    public WorkflowGraph Describe(string? rootId = null)
    {
        var graph = new WorkflowGraph();
        var metadata = StepMetadataRegistry.Instance.Value.GetMetadata(this.GetType());

        rootId ??= metadata.Id;
        var rootNode = new WorkflowNode(rootId, $"Fork - {_forkStrategy}");
        graph.Nodes.Add(rootNode);
        if (_forkStrategy == ForkStrategy.WaitForAll)
        {
            var mergeNode = new WorkflowNode($"{rootId}_merge", "Join");
            graph.Nodes.Add(mergeNode);

            for (int idx = 0; idx < _forks.Length; idx++)
            {
                var workflow = (Workflow) _forks[idx]();
                if (workflow is null) continue;
                var subgraph = workflow.Graph;
                var (entryId, exitIds) = graph.Merge(subgraph, $"{rootId}_branch{idx}");

                graph.Edges.Add(new WorkflowEdge(rootId, entryId, $"Branch {idx + 1}"));

                foreach (var exitId in exitIds)
                {
                    graph.Edges.Add(new WorkflowEdge(exitId, mergeNode.Id));
                }
            }

            return graph;
        }
        else 
        {
            for (int idx = 0; idx < _forks.Length; idx++)
            {
                var workflow = (Workflow) _forks[idx]();
                if (workflow is null) continue;
                var subgraph = workflow.Graph;
                var (entryId, exitIds) = graph.Merge(subgraph, $"{rootId}_branch{idx}");

                graph.Edges.Add(new WorkflowEdge(rootId, entryId, $"Branch {idx + 1}"));
            }
            graph.ContinueFromId = rootId;
            graph.ExitEdgeLabel = "Main Thread";

            return graph;
        }
    }


}
