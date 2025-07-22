namespace FFlow.Visualization;

/// <summary>
/// Represents a node inside a <see cref="WorkflowGraph"/>.
/// </summary>
/// <param name="Id">The id for the node.</param>
/// <param name="Label">The display label for the node.</param>
public record WorkflowNode(string Id, string Label);