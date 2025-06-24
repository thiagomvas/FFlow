using FFlow.Core;
using FFlow.Observability.Metrics;

namespace FFlow.Observability;

internal static class Internals
{
    internal static string BuildMetricsSinkKey<TMetricSink>()
        where TMetricSink : class, IMetricsSink
    {
        return $"__metrics_sink:{typeof(TMetricSink).FullName}";
    }

    internal static string BuildEventListenerKey<TEventListener>()
        where TEventListener : class, IFlowEventListener
    {
        return $"__event_listener:{typeof(TEventListener).FullName}";
    }
    
    
}