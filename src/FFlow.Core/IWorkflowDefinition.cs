namespace FFlow.Core;

/// <summary>
/// Represents a definition for a workflow.
/// </summary>
/// <remarks>
/// Should be implemented following the factory pattern, where <see cref="IWorkflowDefinition.Build"/>
/// should contain the logic to construct the workflow instance based on the defined steps and conditions.
/// </remarks>
public interface IWorkflowDefinition
{
    /// <summary>
    /// Builds and returns an instance of a workflow based on the definition.
    /// </summary>
    /// <returns>An instance of <see cref="IWorkflow"/> representing the built workflow.</returns>
    IWorkflow Build();
    
    IWorkflowMetadataStore MetadataStore { get; }
}