namespace FFlow.Observability.Metrics;

/// <summary>
/// An in-memory implementation of <see cref="IMetricsSink"/> that stores metrics in local dictionaries.
/// Useful for testing and debugging.
/// </summary>
public class InMemoryMetricsSink : IMetricsSink
{
    private readonly Dictionary<string, int> _counters = new();
    private readonly Dictionary<string, List<TimeSpan>> _timings = new();
    private readonly Dictionary<string, double> _gauges = new();

    public void Increment(string name, IDictionary<string, string>? tags = null)
    {
        var key = BuildKey(name, tags);
        _counters.TryGetValue(key, out var current);
        _counters[key] = current + 1;
    }

    public void RecordTiming(string name, TimeSpan duration, IDictionary<string, string>? tags = null)
    {
        var key = BuildKey(name, tags);
        if (!_timings.TryGetValue(key, out var timings))
        {
            timings = new List<TimeSpan>();
            _timings[key] = timings;
        }

        timings.Add(duration);
    }

    public void SetGauge(string name, double value, IDictionary<string, string>? tags = null)
    {
        var key = BuildKey(name, tags);
        _gauges[key] = value;
    }

    private static string BuildKey(string name, IDictionary<string, string>? tags)
    {
        if (tags is null || tags.Count == 0) return name;
        var tagString = string.Join(",", tags.OrderBy(kv => kv.Key).Select(kv => $"{kv.Key}={kv.Value}"));
        return $"{name}[{tagString}]";
    }
    
    public MetricsSnapshot Flush(bool reset = false)
    {
        var snapshot = new MetricsSnapshot(
            new Dictionary<string, int>(_counters),
            _timings.ToDictionary(kv => kv.Key, kv => new List<TimeSpan>(kv.Value)),
            new Dictionary<string, double>(_gauges));

        if (reset)
        {
            _counters.Clear();
            _timings.Clear();
            _gauges.Clear();
        }

        return snapshot;
    }

    /// <summary>
    /// Gets the recorded counter metrics.
    /// </summary>
    /// <returns>A read-only dictionary of counters keyed by metric name and tags.</returns>
    public IReadOnlyDictionary<string, int> GetCounters() => _counters;

    /// <summary>
    /// Gets the recorded timing metrics.
    /// </summary>
    /// <returns>A read-only dictionary of timing lists keyed by metric name and tags.</returns>
    public IReadOnlyDictionary<string, List<TimeSpan>> GetTimings() => _timings;

    /// <summary>
    /// Gets the recorded gauge metrics.
    /// </summary>
    /// <returns>A read-only dictionary of gauge values keyed by metric name and tags.</returns>
    public IReadOnlyDictionary<string, double> GetGauges() => _gauges;
}