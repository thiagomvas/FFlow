namespace FFlow.Observability.Metrics;

/// <summary>
/// A no-op implementation of <see cref="IMetricsSink"/> that ignores all metric operations.
/// Useful as a default or when metrics collection is disabled.
/// </summary>
public class NoOpMemorySink : IMetricsSink
{
    public void Increment(string name, IDictionary<string, string>? tags = null)
    {
        
    }

    public void RecordTiming(string name, TimeSpan duration, IDictionary<string, string>? tags = null)
    {
    }

    public void SetGauge(string name, double value, IDictionary<string, string>? tags = null)
    {
    }

    /// <summary>
    /// Returns an empty <see cref="MetricsSnapshot"/> without recording any metrics.
    /// </summary>
    /// <returns>An empty <see cref="MetricsSnapshot"/>.</returns>
    public MetricsSnapshot Flush(bool reset = false)
    {
        return new MetricsSnapshot(
            new Dictionary<string, int>(),
            new Dictionary<string, List<TimeSpan>>(),
            new Dictionary<string, double>());
    }
}