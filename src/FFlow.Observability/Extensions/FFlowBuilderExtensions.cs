using FFlow.Observability.Listeners;
using FFlow.Observability.Metrics;

namespace FFlow.Observability.Extensions;

public static class FFlowBuilderExtensions
{
    /// <summary>
    /// Adds a metrics sink to the FFlow builder.
    /// </summary>
    /// <param name="builder">The <see cref="FFlowBuilder"/> to attach the <see cref="IMetricsSink"/> into</param>
    /// <param name="metricsSink">The <see cref="IMetricsSink"/> to be attached.</param>
    /// <returns>The same <see cref="FFlowBuilder"/> instance.</returns>
    public static FFlowBuilder UseMetrics<TSink>(this FFlowBuilder builder, TSink metricsSink) where TSink : class, IMetricsSink
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(metricsSink);

        builder.WithOptions(options =>
        {
            options.WithEventListener(new MetricTrackingListener<TSink>(metricsSink));
        });
        return builder;
        
    }
    
}