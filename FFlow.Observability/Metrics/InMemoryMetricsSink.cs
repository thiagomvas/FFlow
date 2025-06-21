namespace FFlow.Observability.Metrics;

public class InMemoryMetricsSink : IMetricsSink
{
    private readonly Dictionary<string, int> _counters = new();
    private readonly Dictionary<string, List<TimeSpan>> _timings = new();
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
    
    private static string BuildKey(string name, IDictionary<string, string>? tags)
    {
        if (tags is null || tags.Count == 0) return name;
        var tagString = string.Join(",", tags.Select(kv => $"{kv.Key}={kv.Value}"));
        return $"{name}[{tagString}]";
    }
    
    public IReadOnlyDictionary<string, int> GetCounters() => _counters;
    public IReadOnlyDictionary<string, List<TimeSpan>> GetTimings() => _timings;
}