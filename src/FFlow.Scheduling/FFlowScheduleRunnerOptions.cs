namespace FFlow.Scheduling;

/// <summary>
/// Represents configuration options for the FFlow schedule runner.
/// </summary>
public class FFlowScheduleRunnerOptions
{
    /// <summary>
    /// Gets or sets the interval at which the schedule runner checks for the next due workflow.
    /// If no workflow is due sooner, this interval is used as the delay between polling cycles.
    /// Default value is 30 seconds.
    /// </summary>
    public TimeSpan PollingInterval { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Gets or sets a value indicating whether logging is enabled for the schedule runner.
    /// Default value is <c>true</c>.
    /// </summary>
    public bool EnableLogging { get; set; } = true;
}