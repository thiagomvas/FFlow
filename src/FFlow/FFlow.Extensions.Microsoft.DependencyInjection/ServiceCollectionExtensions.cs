using System.Reflection;
using FFlow.Core;
using Microsoft.Extensions.DependencyInjection;

namespace FFlow.Extensions.Microsoft.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFFlow(this IServiceCollection services, params Assembly[] assemblies)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        foreach (var assembly in assemblies)
        {
            var stepTypes = assembly.GetTypes()
                .Where(t => typeof(IFlowStep).IsAssignableFrom(t) && !t.IsAbstract && t.IsClass);

            foreach (var stepType in stepTypes)
            {
                services.AddTransient(stepType);
            }

            var workflowTypes = assembly.GetTypes()
                .Where(t => typeof(IWorkflowDefinition).IsAssignableFrom(t) && !t.IsAbstract && t.IsClass);
            foreach (var workflowType in workflowTypes)
            {
                services.AddTransient(workflowType);
                services.AddTransient(provider =>
                {
                    var workflowDefinition = (IWorkflowDefinition)provider.GetRequiredService(workflowType);
                    return workflowDefinition.Build();
                });
            }
        }

        return services;
    }

    
}