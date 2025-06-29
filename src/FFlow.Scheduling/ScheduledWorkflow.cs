using FFlow.Core;

namespace FFlow.Scheduling;

public class ScheduledWorkflow
{
    public IWorkflowDefinition Workflow { get; set; }
    public DateTimeOffset ExecuteAt { get; set; }

    private ScheduledWorkflow()
    {
        
    }
    
    public static ScheduledWorkflow CreateSingleTime(
        IWorkflowDefinition workflow,
        DateTimeOffset executeAt)
    {
        return new ScheduledWorkflow
        {
            Workflow = workflow,
            ExecuteAt = executeAt
        };
    }
}