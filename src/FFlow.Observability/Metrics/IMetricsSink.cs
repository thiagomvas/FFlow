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
    /// <param name="tags">Optional tags to associate with the metric for filtering or grouping.</param>
    void Increment(string name, IDictionary<string, string>? tags = null);

    /// <summary>
    /// Records a timing metric.
    /// </summary>
    /// <param name="name">The name of the metric.</param>
    /// <param name="duration">The duration to record.</param>
    /// <param name="tags">Optional tags to associate with the metric for filtering or grouping.</param>
    void RecordTiming(string name, TimeSpan duration, IDictionary<string, string>? tags = null);
    
    /// <summary>
    /// Sets a gauge metric to a specific value.
    /// A gauge represents a measurement that can arbitrarily go up or down,
    /// reflecting the current state or level of a resource or metric.
    /// </summary>
    /// <param name="name">The name of the gauge metric.</param>
    /// <param name="value">The value to set the gauge to.</param>
    /// <param name="tags">Optional tags to associate with the metric for filtering or grouping.</param>
    void SetGauge(string name, double value, IDictionary<string, string>? tags = null);

}