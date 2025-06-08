using FFlow.Core;
using Microsoft.Extensions.DependencyInjection;

namespace FFlow.Extensions.Microsoft.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFFlow(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        // Register all steps
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            var stepTypes = assembly.GetTypes()
                .Where(t => typeof(IFlowStep).IsAssignableFrom(t) && !t.IsAbstract && t.IsClass);
            
            foreach (var stepType in stepTypes)
            {
                services.AddTransient(stepType);
            }
        }

        return services;

    }
    
}