using FFlow.Core;

namespace FFlow.Scheduling;

/// <summary>
/// Provides methods for building and scheduling workflows.
/// </summary>
public interface IFflowSchedulingBuilder
{
    /// <summary>
    /// Adds a workflow to the scheduling builder.
    /// </summary>
    /// <typeparam name="TWorkflow">The type of the workflow definition to add.</typeparam>
    /// <returns>An instance of <see cref="IFflowScheduledWorkflowBuilder"/> for further configuration.</returns>
    IFflowScheduledWorkflowBuilder AddWorkflow<TWorkflow>() where TWorkflow : IWorkflowDefinition;
}