using FFlow.Core;
using FFlow.Extensions;
using FFlow.Visualization;
using Workflow.Tests.Shared;

namespace FFlow.Tests;

public class VisualizationTests
{
    [Test]
    public void Describe_ShouldGenerateGraphInCorrectOrder()
    {
        var builder = new nFFlowBuilder()
            .StartWith<TestStep>()
            .Then<DelayedStep>()
            .Then<CompensableStep>();

        var graph = builder.Describe();
        
        Assert.Multiple(() =>
        {
            Assert.That(graph.Nodes, Has.Count.EqualTo(3), "Graph should contain 3 nodes.");
            Assert.That(graph.Edges, Has.Count.EqualTo(2), "Graph should contain 2 edges.");
            Assert.That(graph.Nodes[0].Id, Does.Contain("TestStep"), "First node should be TestStep.");
            Assert.That(graph.Nodes[1].Id, Does.Contain("DelayedStep"), "Second node should be DelayedStep.");
            Assert.That(graph.Nodes[2].Id, Does.Contain("CompensableStep"), "Third node should be CompensableStep.");
        });
    }

    [Test]
    public void Describe_ShouldUseMetadataAttributes()
    {
        var builder = new nFFlowBuilder()
            .StartWith<StepWithMetadata>();

        var graph = builder.Describe();
        
        Assert.Multiple(() =>
        {
            Assert.That(graph.Nodes, Has.Count.EqualTo(1), "Graph should contain 1 node.");
            Assert.That(graph.Nodes[0].Label, Does.Contain("Step With Metadata"), "Node should be StepWithMetadata.");
        });
    }

    [Test]
    public void Merge_ShouldMergeGraphsCorrectly()
    {
        var subGraph = new WorkflowGraph();
        subGraph.Nodes.Add(new WorkflowNode("SubStep1", "Sub Step 1"));
        subGraph.Nodes.Add(new WorkflowNode("SubStep2", "Sub Step 2"));
        subGraph.Edges.Add(new WorkflowEdge("SubStep1", "SubStep2"));

        var mainGraph = new WorkflowGraph();
        mainGraph.Nodes.Add(new WorkflowNode("MainStep", "Main Step"));

        var (entryId, exitIds) = mainGraph.Merge(subGraph, "");
        Assert.Multiple(() =>
        {
            Assert.That(entryId, Is.EqualTo("SubStep1"), "Entry ID should be SubStep1 (entry of merged graph).");
            Assert.That(exitIds, Has.Count.EqualTo(1), "There should be one exit ID.");
            Assert.That(exitIds[0], Is.EqualTo("SubStep2"), "Exit ID should be SubStep2.");
            Assert.That(mainGraph.Nodes, Has.Count.EqualTo(3), "Main graph should contain 3 nodes after merge.");
            Assert.That(mainGraph.Edges, Has.Count.EqualTo(1), "Main graph should contain 1 edge after merge.");
        });
    }

    [Test]
    public void Merge_WhenPassingMergeIntoId_ShouldMergeConnected()
    {
        var subGraph = new WorkflowGraph();
        subGraph.Nodes.Add(new WorkflowNode("SubStep1", "Sub Step 1"));
        subGraph.Nodes.Add(new WorkflowNode("SubStep2", "Sub Step 2"));
        subGraph.Edges.Add(new WorkflowEdge("SubStep1", "SubStep2"));

        var mainGraph = new WorkflowGraph();
        mainGraph.Nodes.Add(new WorkflowNode("MainStep", "Main Step"));

        var (entryId, exitIds) = mainGraph.Merge(subGraph, "", "MainStep");
        
        Assert.Multiple(() =>
        {
            Assert.That(entryId, Is.EqualTo("SubStep1"), "Entry ID should be SubStep1 (entry of merged graph).");
            Assert.That(exitIds, Has.Count.EqualTo(1), "There should be one exit ID.");
            Assert.That(mainGraph.Nodes, Has.Count.EqualTo(3), "Main graph should contain 3 nodes after merge.");
            Assert.That(mainGraph.Edges, Has.Count.EqualTo(2), "Main graph should contain 2 edges after merge.");
            Assert.That(mainGraph.Edges.Any(e => e.From == "MainStep" && e.To == "SubStep1"), 
                Is.True, "There should be an edge from MainStep to SubStep1.");
        });
        
    }

    [StepName("Step With Metadata")]
    private class StepWithMetadata : FlowStep
    {
        public string MetadataProperty { get; set; } = "TestValue";

        protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}