namespace FFlow.Scheduling;

/// <summary>
/// Defines methods for configuring the execution schedule of workflows.
/// </summary>
public interface IFflowScheduledWorkflowBuilder
{
    /// <summary>
    /// Configures the workflow to run periodically based on a cron expression.
    /// </summary>
    /// <param name="cron">The cron expression specifying the schedule.</param>
    void RunEvery(string cron);

    /// <summary>
    /// Configures the workflow to run periodically at a fixed interval.
    /// </summary>
    /// <param name="interval">The time interval between executions.</param>
    void RunEvery(TimeSpan interval);

    /// <summary>
    /// Configures the workflow to run once at a specific UTC time.
    /// </summary>
    /// <param name="timeUtc">The UTC time at which the workflow should execute.</param>
    void RunOnceAt(DateTime timeUtc);
}