namespace FFlow.Visualization;

/// <summary>
/// Represents a workflow using a graph structure.
/// </summary>
public class WorkflowGraph
{
    /// <summary>
    /// Gets the node list for the graph.
    /// </summary>
    public List<WorkflowNode> Nodes { get; } = new();
    
    /// <summary>
    /// Gets the edge list for the graph.
    /// </summary>
    public List<WorkflowEdge> Edges { get; } = new();

    /// <summary>
    /// Merges a subgraph at the end of the graph.
    /// </summary>
    /// <param name="subGraph">The subgraph to merge.</param>
    /// <param name="idPrefix">The prefix to add to any nodes merged.</param>
    /// <returns>A tuple containing the entry node ID and a list of exit node IDs.</returns>
    public (string EntryNodeId, List<string> ExitNodeIds) Merge(WorkflowGraph subGraph, string idPrefix)
    {
        var idMap = new Dictionary<string, string>();

        foreach (var node in subGraph.Nodes)
        {
            var newId = $"{idPrefix}_{node.Id}";
            idMap[node.Id] = newId;
            Nodes.Add(new WorkflowNode(newId, node.Label));
        }

        foreach (var edge in subGraph.Edges)
        {
            Edges.Add(new WorkflowEdge(
                idMap[edge.From],
                idMap[edge.To],
                edge.Label));
        }

        string entryId = idMap[subGraph.Nodes.First().Id];
        var exitIds = subGraph.Nodes
            .Where(n => subGraph.Edges.All(e => e.From != n.Id)) // Nodes with no outgoing edges
            .Select(n => idMap[n.Id])
            .ToList();

        return (entryId, exitIds);
    }

    /// <summary>
    /// Converts the graph into a markdown mermaid graph.
    /// </summary>
    /// <returns>The mermaid graph representation of this graph.</returns>
    public string ToMermaid()
    {
        var mermaid = new System.Text.StringBuilder("graph TD\n");

        foreach (var node in Nodes)
        {
            mermaid.AppendLine($"    {node.Id}[{node.Label}]");
        }

        foreach (var edge in Edges)
        {
            if (string.IsNullOrEmpty(edge.Label))
            {
                mermaid.AppendLine($"    {edge.From} --> {edge.To}");
            }
            else
            {
                mermaid.AppendLine($"    {edge.From} -->|{edge.Label}| {edge.To}");
            }
        }

        return mermaid.ToString();
    }

}