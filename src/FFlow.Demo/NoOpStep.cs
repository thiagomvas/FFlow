using FFlow.Core;
using FFlow.Visualization;

namespace FFlow.Demo;

public class NoOpStep : FlowStep, IDescribableStep
{
    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        context.SetOutputFor<NoOpStep, object>(context.GetLastOutput<object>());
        return Task.CompletedTask;
    }
    public WorkflowGraph Describe(string? rootId = null)
    {
        var graph = new WorkflowGraph();
        var metadata = StepMetadataRegistry.Instance.Value.GetMetadata(this.GetType());

        rootId ??= metadata.Id;
        var rootNode = new WorkflowNode(rootId, $"My Step Root Name");
        graph.Nodes.Add(rootNode);

        var someNode = new WorkflowNode($"{rootId}_someNode", "Some other node");
        graph.Nodes.Add(someNode);
        graph.Edges.Add(new WorkflowEdge(rootNode.Id, someNode.Id, "Some Label"));

        var someOtherNode = new WorkflowNode($"{rootId}_someOtherNode", "Some other node");
        graph.Nodes.Add(someOtherNode);
        graph.Edges.Add(new WorkflowEdge(rootNode.Id, someOtherNode.Id, "Some other label"));
        
        graph.ContinueFromId = rootId;
        graph.ExitEdgeLabel = "Continue from here!";
        return graph;

    }
}