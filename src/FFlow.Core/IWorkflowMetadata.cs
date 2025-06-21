namespace FFlow.Core;

public interface IWorkflowMetadata
{
    Guid Id { get; }
    string Name { get; set; }
    string Description { get; set; }
    Dictionary<string, string> Tags { get; }
}