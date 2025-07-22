namespace FFlow.Visualization;

public class WorkflowGraph
{
    public List<WorkflowNode> Nodes { get; } = new();
    public List<WorkflowEdge> Edges { get; } = new();

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