using FFlow.Core;
using FFlow.Visualization;

namespace FFlow.Extensions;

/// <summary>
/// Contains extension methods for <see cref="IWorkflowBuilder"/> related to visualization.
/// </summary>
public static class IWorkflowBuilderVisualizationExtensions
{
    /// <summary>
    /// Describes the current state of the builder as a graph structure.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="flags"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static WorkflowGraph Describe(this IWorkflowBuilder builder, VisualizationFlags flags = VisualizationFlags.IgnoreInputOutputSetters)
    {
        var graph = new WorkflowGraph();
        var visited = new HashSet<IFlowStep>();
        var queue = new Queue<IFlowStep>();

        var steps = builder.Steps.ToList();
        int i = 0;

        queue.Enqueue(steps.FirstOrDefault() ?? throw new InvalidOperationException("No steps found in the workflow."));
        WorkflowNode? previousNode = null;
        string? nextLabel = null;

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (visited.Contains(current))
                continue;

            if (ShouldSkipBasedOnFlag(current.GetType(), flags) && i + 1 < steps.Count)
            {
                i++;
                queue.Enqueue(steps[i]);
                continue;
            }

            visited.Add(current);
            var metadata = StepMetadataRegistry.Instance.Value.GetMetadata(current.GetType());

            if (current is IDescribableStep describable)
            {
                var subgraph = describable.Describe($"{metadata.Id}{i}");
                var (entryId, exitIds) = graph.Merge(subgraph, $"");

                if (previousNode != null)
                    graph.Edges.Add(new WorkflowEdge(previousNode.Id, entryId, nextLabel));

                var lastExit = subgraph.ContinueFrom ?? graph.Nodes.FirstOrDefault(n => n.Id == exitIds.Last());
                nextLabel = subgraph.ExitEdgeLabel;
                previousNode = lastExit ?? graph.Nodes.First(n => n.Id == entryId);
            }

            else
            {
                var node = new WorkflowNode($"{metadata.Id}{i}", metadata.Name);
                graph.Nodes.Add(node);

                if (previousNode != null)
                {
                    graph.Edges.Add(new WorkflowEdge(previousNode.Id, node.Id, nextLabel));
                }

                nextLabel = null;
                previousNode = node;
            }

            i++;
            if (i < steps.Count)
                queue.Enqueue(steps[i]);
        }

        return graph;
    }

    private static bool ShouldSkipBasedOnFlag(Type stepType, VisualizationFlags flags)
    {
        if (flags.HasFlag(VisualizationFlags.IgnoreInputSetters) && stepType == typeof(InputSetterStep))
            return true;

        if (flags.HasFlag(VisualizationFlags.IgnoreOutputSetters) && stepType == typeof(OutputSetterStep))
            return true;

        if (flags.HasFlag(VisualizationFlags.IgnoreSilentSteps)
            && stepType.GetCustomAttributes(typeof(SilentStepAttribute), true).Length > 0)
            return true;

        return false;
    }
}
