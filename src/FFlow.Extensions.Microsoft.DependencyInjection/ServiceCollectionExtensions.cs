using System.Reflection;
    using FFlow.Core;
    using Microsoft.Extensions.DependencyInjection;
    
    namespace FFlow.Extensions.Microsoft.DependencyInjection;
    
    /// <summary>
    /// Provides extension methods for registering FFlow components in the dependency injection container.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers FFlow steps and workflow definitions from the specified assemblies into the service collection.
        /// </summary>
        /// <param name="services">The service collection to add the registrations to.</param>
        /// <param name="assemblies">The assemblies to scan for FFlow steps and workflow definitions.</param>
        /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="services"/> parameter is null.</exception>
        public static IServiceCollection AddFFlow(this IServiceCollection services, params Assembly[] assemblies)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            
            if (assemblies == null || assemblies.Length == 0)
            {
                throw new ArgumentException("At least one assembly must be provided.", nameof(assemblies));
            }
    
            foreach (var assembly in assemblies)
            {
                // Register all types implementing IFlowStep
                var stepTypes = assembly.GetTypes()
                    .Where(t => typeof(IFlowStep).IsAssignableFrom(t) && !t.IsAbstract && t.IsClass && !typeof(BaseStepDecorator).IsAssignableFrom(t));
    
                foreach (var stepType in stepTypes)
                {
                    services.AddTransient(stepType);
                    services.AddTransient(typeof(IFlowStep), stepType);
                }
    
                // Register all types implementing IWorkflowDefinition
                var workflowTypes = assembly.GetTypes()
                    .Where(t => typeof(IWorkflowDefinition).IsAssignableFrom(t) && !t.IsAbstract && t.IsClass);
                foreach (var workflowType in workflowTypes)
                {
                    services.AddTransient(typeof(IWorkflowDefinition), workflowType);
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