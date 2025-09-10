using FFlow.Core;
using FFlow.Observability.Metrics;

namespace FFlow.Observability.Extensions;

public static class IFlowContextExtensions
{
    /// <summary>
    /// Retrieves a metrics sink of the specified type from the <see cref="IFlowContext"/>.
    /// </summary>
    /// <typeparam name="TMetricSink">
    /// The type of the metrics sink to retrieve. Must implement <see cref="IMetricsSink"/>.
    /// </typeparam>
    /// <param name="context">The flow context to retrieve the metrics sink from.</param>
    /// <returns>The registered metrics sink of type <typeparamref name="TMetricSink"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if a metrics sink of type <typeparamref name="TMetricSink"/> is not registered in the context.
    /// </exception>

    public static TMetricSink GetMetricsSink<TMetricSink>(this IFlowContext context)
        where TMetricSink : class, IMetricsSink
    {
        ArgumentNullException.ThrowIfNull(context);

        var key = Internals.BuildMetricsSinkKey<TMetricSink>();
        if (context.GetValue<TMetricSink>(key) is TMetricSink sink)
        {
            return sink;
        }
        
        throw new InvalidOperationException($"Metrics sink of type {typeof(TMetricSink).FullName} is not registered in the context.");
    }
    
}