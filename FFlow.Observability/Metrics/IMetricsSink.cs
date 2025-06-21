namespace FFlow.Observability.Metrics;

public interface IMetricsSink
{
    void Increment(string name, IDictionary<string, string>? tags = null);
    void RecordTiming(string name, TimeSpan duration, IDictionary<string, string>? tags = null);
}
