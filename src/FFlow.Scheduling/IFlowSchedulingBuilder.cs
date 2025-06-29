using FFlow.Core;

namespace FFlow.Scheduling;

public interface IFflowSchedulingBuilder
{
    IFflowScheduledWorkflowBuilder AddWorkflow<TWorkflow>() where TWorkflow : IWorkflowDefinition;
}