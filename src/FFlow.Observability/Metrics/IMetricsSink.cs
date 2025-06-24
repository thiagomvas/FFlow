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

    /// <summary>
    /// Captures a snapshot of the current metrics, optionally resetting the internal state.
    /// </summary>
    /// <param name="reset">
    /// If <c>true</c>, clears the internal counters, timings, and gauges after capturing the snapshot. 
    /// If <c>false</c>, leaves the internal state unchanged.
    /// </param>
    /// <returns>
    /// A <see cref="MetricsSnapshot"/> containing the current metrics data at the time of the call.
    /// </returns>
    MetricsSnapshot Flush(bool reset = false);

}