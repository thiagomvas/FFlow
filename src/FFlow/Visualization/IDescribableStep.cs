using FFlow.Core;

namespace FFlow.Visualization;

/// <summary>
/// Represents a step that can be described into a custom graph structure.
/// </summary>
public interface IDescribableStep
{
    /// <summary>
    /// Describes the step as a graph.
    /// </summary>
    /// <param name="rootId">The root node ID override.</param>
    /// <returns>A graph structure representing the logic or flow for this step and its behaviour.</returns>
    /// <remarks>
    /// Used mostly internally to generate graphs from an <see cref="IWorkflowBuilder"/>.
    /// When implemented, it'll replace a simple node with a graph in the generated graph by merging it
    /// into the resulting graph. The last exit node is the one used to continue the workflow
    /// after this step.
    /// </remarks>
    WorkflowGraph Describe(string? rootId = null);
}