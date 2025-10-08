using System.Reflection;
using FFlow.Core;

namespace FFlow.DSL;

public class StepContainer : IStepContainer
{
    private readonly Dictionary<string, Type> _steps = new();
    public StepContainer()
    {
    }

    public void LoadStepsFromRegistry(IStepRegistry registry)
    {
        registry.RegisterSteps(this);
    }
    
    public void LoadAllRegistries()
    {
        var registryTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(IStepRegistry).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

        foreach (var registryType in registryTypes)
        {
            if (Activator.CreateInstance(registryType) is IStepRegistry registry)
            {
                registry.RegisterSteps(this);
            }
        }
    }

    public FlowStep GetStep(string identifier)
    {
        if (_steps.TryGetValue(identifier, out var type))
        {
            return (FlowStep)Activator.CreateInstance(type)!;
        }
        throw new Exception($"Step with identifier '{identifier}' not found.");
    }

    public FlowStep GetStep(string identifier, Dictionary<string, object> parameters)
    {
        var step = GetStep(identifier);
        foreach (var param in parameters)
        {
            var property = step.GetType().GetProperty(param.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property != null && property.CanWrite)
            {
                property.SetValue(step, Convert.ChangeType(param.Value, property.PropertyType));
            }
        }
        return step;
        
    }

    public void AddStep<T>(string identifier) where T : FlowStep, new()
    {
        if (!_steps.ContainsKey(identifier))
        {
            _steps[identifier] = typeof(T);
        }
    }
}