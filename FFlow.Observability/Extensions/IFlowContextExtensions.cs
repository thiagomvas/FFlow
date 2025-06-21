using FFlow.Core;
using FFlow.Observability.Metrics;

namespace FFlow.Observability.Extensions;

public static class IFlowContextExtensions
{
    public static TMetricSink GetMetricsSink<TMetricSink>(this IFlowContext context)
        where TMetricSink : class, IMetricsSink
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        
        var key = Internals.BuildMetricsSinkKey<TMetricSink>();
        if (context.TryGet(key, out TMetricSink? sink))
        {
            return sink;
        }
        
        throw new InvalidOperationException($"Metrics sink of type {typeof(TMetricSink).FullName} is not registered in the context.");
    }
    
}