using FFlow.Core;
using FFlow.Visualization;

namespace FFlow.Extensions;

public static class IWorkflowBuilderVisualizationExtensions
{
    /// <summary>
    /// Describes the current state of the workflow builder as a graph.
    /// </summary>
    /// <param name="builder">The builder to describe.</param>
    /// <param name="flags">
    /// The flags that control which steps to include in the graph.
    /// By default, it ignores input and output setters.
    /// </param>
    /// <returns>A graph that represents the current state of the builder.</returns>
    /// <exception cref="InvalidOperationException">Thrown if there are no steps in the builder.</exception>
    /// <remarks>
    /// If more steps are added after this method is called, they will not be included in the graph.
    /// </remarks>
    public static WorkflowGraph Describe(this FFlowBuilder builder, VisualizationFlags flags = VisualizationFlags.IgnoreInputOutputSetters)
    {
        var graph = new WorkflowGraph();
        var visited = new HashSet<IFlowStep>();
        var queue = new Queue<IFlowStep>();
        
        var steps = builder.Steps.ToList();
        int i = 0;
        
        queue.Enqueue(steps.FirstOrDefault() ?? throw new InvalidOperationException("No steps found in the workflow."));
        WorkflowNode? previousNode = null;
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
            
            var node = new WorkflowNode($"{metadata.Id}{i}", metadata.Name);
            graph.Nodes.Add(node);
            if (previousNode != null)
            {
                graph.Edges.Add(new WorkflowEdge(previousNode.Id, node.Id));
            }
            previousNode = node;

            i++;
            if(i < steps.Count)
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