namespace FFlow.Observability.Metrics;

/// <summary>
/// Represents a sink for emitting metrics such as counters and timings.
/// </summary>
public interface IMetricsSink
{
    /// <summary>
    /// Increments a counter metric by one.
    /// </summary>
    /// <param name="name">The name of the metric.</param>
    /// <param name="tags">Optional tags to associate with the metric.</param>
    void Increment(string name, IDictionary<string, string>? tags = null);

    /// <summary>
    /// Records a timing metric.
    /// </summary>
    /// <param name="name">The name of the metric.</param>
    /// <param name="duration">The duration to record.</param>
    /// <param name="tags">Optional tags to associate with the metric.</param>
    void RecordTiming(string name, TimeSpan duration, IDictionary<string, string>? tags = null);
}