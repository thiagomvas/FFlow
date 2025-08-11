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
    /// Gets or sets the ID of the <see cref="WorkflowNode"/> that subsequent nodes should continue from when merging.
    /// </summary>
    public string? ContinueFromId { get; set; }
    
    /// <summary>
    /// Gets or sets the <see cref="WorkflowNode"/> that subsequent nodes should continue from when merging.
    /// </summary>
    public WorkflowNode? ContinueFrom
    {
        get => Nodes.FirstOrDefault(n => n.Id == ContinueFromId);
        set => ContinueFromId = value?.Id;
    }

    /// <summary>
    /// Gets or sets the label for the <see cref="WorkflowEdge"/> connecting the subsequent nodes to this graph.
    /// </summary>
    public string? ExitEdgeLabel { get; set; }

    /// <summary>
    /// Merges a subgraph at the end of the graph.
    /// </summary>
    /// <param name="subGraph">The subgraph to merge.</param>
    /// <param name="idPrefix">The prefix to add to any nodes merged.</param>
    /// <returns>A tuple containing the entry node ID and a list of exit node IDs.</returns>
    /// <remarks>
    /// This <b>does not</b> connect the merged graph into the existing graph.
    /// All it does is create copies of the nodes and edges into the current graph. If you wish
    /// to connect them, add a <see cref="WorkflowEdge"/> manually.
    /// </remarks>
    public (string EntryNodeId, List<string> ExitNodeIds) Merge(WorkflowGraph subGraph, string idPrefix, string? mergeIntoId = null)
    {
        var idMap = new Dictionary<string, string>();

        foreach (var node in subGraph.Nodes)
        {
            var newId = string.IsNullOrWhiteSpace(idPrefix) ? node.Id : $"{idPrefix}_{node.Id}";
            idMap[node.Id] = newId;
            Nodes.Add(node with { Id = newId });
        }

        foreach (var edge in subGraph.Edges)
        {
            Edges.Add(new WorkflowEdge(
                idMap[edge.From],
                idMap[edge.To],
                edge.Label));
        }
        
        if (!string.IsNullOrWhiteSpace(mergeIntoId))
        {
            var firstNodeId = idMap[subGraph.Nodes.First().Id];
            Edges.Add(new WorkflowEdge(mergeIntoId, firstNodeId, ExitEdgeLabel));
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
    
    /// <summary>
    /// Converts the graph into a DOT graph format string.
    /// </summary>
    /// <returns>A string representing the graph in the DOT language format.</returns>
    public string ToDot()
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("digraph WorkflowGraph {");
        sb.AppendLine("  rankdir=TD;"); 

        foreach (var node in Nodes)
        {
            var label = node.Label.Replace("\"", "\\\""); // Escape quotes
            sb.AppendLine($"  \"{node.Id}\" [label=\"{label}\"];");
        }

        foreach (var edge in Edges)
        {
            if (string.IsNullOrEmpty(edge.Label))
                sb.AppendLine($"  \"{edge.From}\" -> \"{edge.To}\";");
            else
            {
                var label = edge.Label.Replace("\"", "\\\"");
                sb.AppendLine($"  \"{edge.From}\" -> \"{edge.To}\" [label=\"{label}\"];");
            }
        }

        sb.AppendLine("}");
        return sb.ToString();
    }

}