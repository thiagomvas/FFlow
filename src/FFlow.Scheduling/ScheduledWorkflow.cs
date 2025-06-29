using FFlow.Core;

namespace FFlow.Scheduling;

public class ScheduledWorkflow
{
    public IWorkflowDefinition Workflow { get; set; }
    public DateTimeOffset ExecuteAt { get; set; }
    
    public bool Recurring { get; private set; }
    public TimeSpan? Interval { get; private set; }
    public string? CronExpression { get; private set; }

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
    
    public static ScheduledWorkflow CreateRecurring(
        IWorkflowDefinition workflow,
        TimeSpan interval,
        DateTimeOffset? nextExecutionAt = null)
    {
        if (nextExecutionAt is null)
        {
            nextExecutionAt = DateTimeOffset.UtcNow.Add(interval);
        }
        return new ScheduledWorkflow
        {
            Workflow = workflow,
            Recurring = true,
            Interval = interval,
            ExecuteAt = nextExecutionAt.Value
        };
    }
    
    public static ScheduledWorkflow CreateRecurring(
        IWorkflowDefinition workflow,
        string cronExpression)
    {
        return new ScheduledWorkflow
        {
            Workflow = workflow,
            Recurring = true,
            CronExpression = cronExpression
        };
    }
    
    internal void UpdateNextExecution(DateTimeOffset now)
    {
        if (!Recurring) return;
        if (Interval.HasValue)
        {
            ExecuteAt = now.Add(Interval.Value);
        }
    }
}