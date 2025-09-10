namespace FFlow.Visualization;

/// <summary>
/// Represents an edge connecting two <see cref="WorkflowNode"/> inside a <see cref="WorkflowGraph"/>
/// </summary>
/// <param name="From">Where the connection starts from.</param>
/// <param name="To">Where the connection ends.</param>
/// <param name="Label">The display label for this edge.</param>
public record WorkflowEdge(string From, string To, string? Label = null);