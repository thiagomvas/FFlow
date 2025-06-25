using FFlow.Core;

namespace FFlow;

/// <summary>
/// In memory implementation of <see cref="IFlowContext"/>.
/// </summary>
public class InMemoryFFLowContext : IFlowContext
{
    private readonly Dictionary<string, object> _values = new();
    private readonly OrderedDictionary<Type, object> _inputs = new();
    private readonly OrderedDictionary<Type, object> _outputs = new();


    public void SetInputFor<TStep, TInput>(TInput input) where TStep : class, IFlowStep
    {

        var key = typeof(TStep);
        _inputs[key] = input;
    }

    public void SetInputFor<TStep, TInput>(TStep step, TInput input) where TStep : class, IFlowStep
    {

        if (step == null)
            throw new ArgumentNullException(nameof(step), "Step cannot be null.");

        var key = step.GetType();
        _inputs[key] = input;
    }

    public void SetOutputFor<TStep, TOutput>(TOutput output) where TStep : class, IFlowStep
    {

        var key = typeof(TStep);
        _outputs[key] = output;
    }

    public void SetOutputFor<TStep, TOutput>(TStep step, TOutput output) where TStep : class, IFlowStep
    {

        if (step == null)
            throw new ArgumentNullException(nameof(step), "Step cannot be null.");

        var key = step.GetType();
        _outputs[key] = output;
    }

    public TInput? GetInputFor<TStep, TInput>() where TStep : class, IFlowStep
    {
        var key = typeof(TStep);
        if (_inputs.TryGetValue(key, out var input) && input is TInput result)
        {
            return result;
        }
        return default;
    }

    public TOutput? GetOutputFor<TStep, TOutput>() where TStep : class, IFlowStep
    {
        var key = typeof(TStep);
        if (_outputs.TryGetValue(key, out var output) && output is TOutput result)
        {
            return result;
        }
        return default;
    }

    public T? GetValue<T>(string key, T defaultValue = default) 
    {
        if (_values.TryGetValue(key, out var value))
        {
            return (T)value;
        }
        return defaultValue;
    }

    public void SetValue<T>(string key, T value)
    {

        _values[key] = value;
    }

    public T? GetSingleValue<T>()
    {
        if (_values.TryGetValue(typeof(T).FullName ?? string.Empty, out var value))
        {
            return (T)value;
        }
        return default;
    }

    public void SetSingleValue<T>(T value)
    {
        _values[typeof(T).FullName ?? string.Empty] = value;
    }

    public T? GetLastInput<T>()
    {
        if (_inputs.Count == 0)
            return default;

        var lastInput = _inputs.Values.LastOrDefault();
        if (lastInput is T result)
        {
            return result;
        }
        return default;
    }

    public T? GetLastOutput<T>()
    {
        if (_outputs.Count == 0)
            return default;

        var lastOutput = _outputs.Values.LastOrDefault();
        if (lastOutput is T result)
        {
            return result;
        }
        return default;
    }

    public IFlowContext Fork()
    {
        var forkedContext = new InMemoryFFLowContext();
        foreach (var kvp in _values)
        {
            forkedContext.SetValue(kvp.Key, kvp.Value);
        }
        foreach (var kvp in _inputs)
        {
            forkedContext._inputs[kvp.Key] = kvp.Value;
        }
        foreach (var kvp in _outputs)
        {
            forkedContext._outputs[kvp.Key] = kvp.Value;
        }
        return forkedContext;
        
    }
}
