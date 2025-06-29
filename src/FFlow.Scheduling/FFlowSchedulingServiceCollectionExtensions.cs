using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FFlow.Scheduling;

public static class FFlowSchedulingServiceCollectionExtensions
{
    public static IServiceCollection AddFflowScheduling(
        this IServiceCollection services,
        Action<IFflowSchedulingBuilder> configure)
    {
        services.AddSingleton<IFlowScheduleStore, InMemoryFlowScheduleStore>();

        var builder = new FflowSchedulingBuilder(services);
        configure(builder);

        services.AddSingleton<FFlowScheduleRunner>(provider =>
        {
            builder.ApplyRegistrations(provider);
            var store = provider.GetRequiredService<IFlowScheduleStore>();
            return new FFlowScheduleRunner(store);
        });

        services.AddSingleton<IHostedService>(provider =>
            provider.GetRequiredService<FFlowScheduleRunner>());



        return services;
    }
}