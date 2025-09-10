using FFlow.Core;

namespace FFlow;

/// <summary>
/// Represents a workflow builder that allows configuration of workflow options.
/// </summary>
public interface IConfigurableWorkflowBuilder
{
    /// <summary>
    /// Configures the workflow with the specified options.
    /// </summary>
    /// <param name="configure">An action to configure the <see cref="WorkflowOptions"/>.</param>
    /// <returns>An instance of <see cref="IWorkflowBuilder"/> with the applied options.</returns>
    WorkflowBuilderBase WithOptions(Action<WorkflowOptions> configure);
}