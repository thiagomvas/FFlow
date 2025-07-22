namespace FFlow.Visualization;

public interface IDescribableStep
{
    WorkflowGraph Describe(string? rootId = null);
}