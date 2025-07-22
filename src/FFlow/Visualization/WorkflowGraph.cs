namespace FFlow.Visualization;

public class WorkflowGraph
{
    public List<WorkflowNode> Nodes { get; } = new();
    public List<WorkflowEdge> Edges { get; } = new();

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