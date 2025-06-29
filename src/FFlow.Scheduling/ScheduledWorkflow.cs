using System;
using FFlow.Core;
using NCrontab;

namespace FFlow.Scheduling;

/// <summary>
/// Represents a workflow that is scheduled for execution.
/// </summary>
public class ScheduledWorkflow
{
    /// <summary>
    /// Gets or sets the workflow definition associated with this scheduled workflow.
    /// </summary>
    public IWorkflowDefinition Workflow { get; set; }

    /// <summary>
    /// Gets or sets the date and time at which the workflow is scheduled to execute.
    /// </summary>
    public DateTimeOffset ExecuteAt { get; set; }

    /// <summary>
    /// Gets a value indicating whether the workflow is recurring.
    /// </summary>
    public bool Recurring { get; private set; }

    /// <summary>
    /// Gets the interval between executions for recurring workflows.
    /// </summary>
    public TimeSpan? Interval { get; private set; }

    /// <summary>
    /// Gets the cron expression used to schedule recurring workflows.
    /// </summary>
    public string? CronExpression { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ScheduledWorkflow"/> class.
    /// Private constructor to enforce the use of factory methods.
    /// </summary>
    private ScheduledWorkflow()
    {
    }

    /// <summary>
    /// Creates a one-time scheduled workflow.
    /// </summary>
    /// <param name="workflow">The workflow definition to schedule.</param>
    /// <param name="executeAt">The date and time at which the workflow should execute.</param>
    /// <returns>A new instance of <see cref="ScheduledWorkflow"/> configured for one-time execution.</returns>
    public static ScheduledWorkflow CreateOneTime(
        IWorkflowDefinition workflow,
        DateTimeOffset executeAt)
    {
        return new ScheduledWorkflow
        {
            Workflow = workflow,
            ExecuteAt = executeAt
        };
    }

    /// <summary>
    /// Creates a recurring scheduled workflow with a fixed interval.
    /// </summary>
    /// <param name="workflow">The workflow definition to schedule.</param>
    /// <param name="interval">The interval between executions.</param>
    /// <param name="nextExecutionAt">Optional. The date and time of the next execution. Defaults to the current time plus the interval.</param>
    /// <returns>A new instance of <see cref="ScheduledWorkflow"/> configured for recurring execution.</returns>
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

    /// <summary>
    /// Creates a recurring scheduled workflow using a cron expression.
    /// </summary>
    /// <param name="workflow">The workflow definition to schedule.</param>
    /// <param name="cronExpression">The cron expression defining the schedule.</param>
    /// <returns>A new instance of <see cref="ScheduledWorkflow"/> configured for recurring execution.</returns>
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

    /// <summary>
    /// Updates the next execution time for a recurring workflow based on the current time.
    /// </summary>
    /// <param name="now">The current date and time.</param>
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