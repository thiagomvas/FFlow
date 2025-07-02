using FFlow.Core;

namespace FFlow;

public class StepTemplateRegistry : IStepTemplateRegistry
{
    private readonly Dictionary<string, object> _templates = new();
    private static readonly Lazy<StepTemplateRegistry> _instance = new(() => new StepTemplateRegistry());
    public static StepTemplateRegistry Instance => _instance.Value;
    
    public void RegisterTemplate<TStep>(string name, Action<TStep> configure) where TStep : IFlowStep
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Template name cannot be null or empty.", nameof(name));

        if (configure is null)
            throw new ArgumentNullException(nameof(configure));

        var key = $"{typeof(TStep).FullName}:{name}";
        _templates[key] = WrapForBaseInterface(configure);
    }


    public void RegisterTemplate(Type stepType, string name, Action<IFlowStep> configure)
    {
        if (stepType is null)
            throw new ArgumentNullException(nameof(stepType), "Step type cannot be null.");
        
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Template name cannot be null or empty.", nameof(name));
        
        if (configure is null)
            throw new ArgumentNullException(nameof(configure), "Configuration action cannot be null.");

        var key = $"{stepType.FullName}:{name}";
        _templates[key] = configure;
    }

    public Action<TStep> GetTemplate<TStep>(string name) where TStep : IFlowStep
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Template name cannot be null or empty.", nameof(name));

        var key = $"{typeof(TStep).FullName}:{name}";
        if (_templates.TryGetValue(key, out var template))
        {
            return (Action<TStep>)template;
        }

        throw new KeyNotFoundException($"Template '{name}' for step type '{typeof(TStep).FullName}' not found.");
    }

    public bool TryGetTemplate<TStep>(string name, out Action<TStep> configure) where TStep : IFlowStep
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            configure = null;
            return false;
        }

        var key = $"{typeof(TStep).FullName}:{name}";
        if (_templates.TryGetValue(key, out var template))
        {
            configure = (Action<TStep>)template;
            return true;
        }

        configure = null;
        return false;
    }

    public bool TryGetTemplate(Type stepType, string name, out Action<IFlowStep> configure)
    {
        if (stepType is null)
        {
            configure = null;
            return false;
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            configure = null;
            return false;
        }

        var key = $"{stepType.FullName}:{name}";
        if (_templates.TryGetValue(key, out var template))
        {
            configure = (Action<IFlowStep>)template;
            return true;
        }

        configure = null;
        return false;
    }

    public void OverrideDefaults<TStep>(Action<TStep> configure) where TStep : IFlowStep
    {
        RegisterTemplate("default", configure);
    }

    public bool TryGetOverridenDefaults<TStep>(out Action<IFlowStep> configure) where TStep : IFlowStep
    {
        return TryGetTemplate(typeof(TStep), "default", out configure);
    }

    public bool TryGetOverridenDefaults(Type type, out Action<IFlowStep> configure)
    {
        return TryGetTemplate(type, "default", out configure);
    }
    
    private static Action<IFlowStep> WrapForBaseInterface<TStep>(Action<TStep> configure) where TStep : IFlowStep
    {
        return step => configure((TStep)step); // safe cast, since you control TStep
    }
}