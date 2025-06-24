namespace FFlow.Observability.Metrics;

/// <summary>
/// Represents a snapshot of recorded metrics at a specific point in time.
/// </summary>
public class MetricsSnapshot
{
    /// <summary>
    /// Gets the recorded counter metrics.
    /// </summary>
    public IReadOnlyDictionary<string, int> Counters { get; }

    /// <summary>
    /// Gets the recorded timing metrics.
    /// </summary>
    public IReadOnlyDictionary<string, List<TimeSpan>> Timings { get; }

    /// <summary>
    /// Gets the recorded gauge metrics.
    /// </summary>
    public IReadOnlyDictionary<string, double> Gauges { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MetricsSnapshot"/> class.
    /// </summary>
    /// <param name="counters">The recorded counters.</param>
    /// <param name="timings">The recorded timings.</param>
    /// <param name="gauges">The recorded gauges.</param>
    public MetricsSnapshot(
        IReadOnlyDictionary<string, int> counters,
        IReadOnlyDictionary<string, List<TimeSpan>> timings,
        IReadOnlyDictionary<string, double> gauges)
    {
        Counters = counters;
        Timings = timings;
        Gauges = gauges;
    }
}
