using System;
using FFlow.Core;
using NCrontab;

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
        var schedule = CrontabSchedule.Parse(cronExpression);
        var next = schedule.GetNextOccurrence(DateTime.UtcNow);
        return new ScheduledWorkflow
        {
            Workflow = workflow,
            Recurring = true,
            CronExpression = cronExpression,
            ExecuteAt = new DateTimeOffset(next, TimeSpan.Zero)
        };
    }
    
    internal void UpdateNextExecution(DateTimeOffset now)
    {
        if (!Recurring) return;
        
        if (Interval.HasValue)
        {
            ExecuteAt = now.Add(Interval.Value);
        }
        else if (!string.IsNullOrEmpty(CronExpression))
        {
            var schedule = CrontabSchedule.Parse(CronExpression);
            var next = schedule.GetNextOccurrence(now.UtcDateTime);
            ExecuteAt = new DateTimeOffset(next, TimeSpan.Zero);
        }
    }
}
