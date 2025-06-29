using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FFlow.Scheduling;

/// <summary>
/// Provides extension methods for configuring FFlow scheduling services.
/// </summary>
public static class FFlowSchedulingServiceCollectionExtensions
{
    /// <summary>
    /// Adds FFlow scheduling services to the specified service collection using the default in-memory schedule store.
    /// </summary>
    /// <param name="services">The service collection to add the scheduling services to.</param>
    /// <param name="configure">An optional delegate to configure the scheduling builder.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddFflowScheduling(
        this IServiceCollection services,
        Action<IFflowSchedulingBuilder>? configure = null)
    {
        services.AddSingleton<IFlowScheduleStore, InMemoryFlowScheduleStore>();
        return AddFlowScheduling<InMemoryFlowScheduleStore>(services, configure);
    }

    /// <summary>
    /// Adds FFlow scheduling services to the specified service collection using a custom schedule store.
    /// </summary>
    /// <typeparam name="TScheduleStore">The type of the schedule store to use.</typeparam>
    /// <param name="services">The service collection to add the scheduling services to.</param>
    /// <param name="configure">An optional delegate to configure the scheduling builder.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddFlowScheduling<TScheduleStore>(
        this IServiceCollection services,
        Action<IFflowSchedulingBuilder>? configure = null)
        where TScheduleStore : class, IFlowScheduleStore
    {

        var builder = new FflowSchedulingBuilder(services);
        configure?.Invoke(builder);

        services.AddSingleton<FFlowScheduleRunner>(provider =>
        {
            builder.ApplyRegistrations(provider);
            var store = provider.GetRequiredService<IFlowScheduleStore>();
            var options = provider.GetRequiredService<IOptions<FFlowScheduleRunnerOptions>>().Value;
            var logger = provider.GetService<ILogger<FFlowScheduleRunner>>();
            return new FFlowScheduleRunner(store, options, logger);
        });

        services.AddSingleton<IHostedService>(provider =>
            provider.GetRequiredService<FFlowScheduleRunner>());

        return services;
    }
}