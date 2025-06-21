using FFlow.Core;

namespace FFlow;

public class InMemoryFFLowContext : IFlowContext
{
    private readonly Dictionary<string, object> _values = new();
    private readonly Dictionary<Type, object> _inputs = new();
    private readonly Dictionary<Type, object> _outputs = new();


    public void SetInput<TStep, TInput>(TInput input) where TStep : class, IFlowStep where TInput : class
    {
        if (input == null)
            throw new ArgumentNullException(nameof(input), "Input cannot be null.");

        var key = typeof(TStep);
        _inputs[key] = input;
    }

    public void SetOutput<TStep, TOutput>(TOutput output) where TStep : class, IFlowStep where TOutput : class
    {
        if (output == null)
            throw new ArgumentNullException(nameof(output), "Output cannot be null.");

        var key = typeof(TStep);
        _outputs[key] = output;
    }

    public TInput? GetInputFor<TStep, TInput>() where TStep : class, IFlowStep where TInput : class
    {
        var key = typeof(TStep);
        if (_inputs.TryGetValue(key, out var input))
        {
            return input as TInput;
        }
        return null;
    }

    public TOutput? GetOutputFor<TStep, TOutput>() where TStep : class, IFlowStep where TOutput : class
    {
        var key = typeof(TStep);
        if (_outputs.TryGetValue(key, out var output))
        {
            return output as TOutput;
        }
        return null;
    }

    public T? GetValue<T>(string key, T defaultValue = null) where T : class
    {
        if (_values.TryGetValue(key, out var value))
        {
            return value as T;
        }
        return defaultValue;
    }

    public void SetValue<T>(string key, T value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value), "Value cannot be null.");

        _values[key] = value;
    }

    public T? GetSingleValue<T>() where T : class
    {
        if (_values.TryGetValue(typeof(T).FullName ?? string.Empty, out var value))
        {
            return value as T;
        }
        return null;
    }

    public void SetSingleValue<T>(T value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value), "Value cannot be null.");

        _values[typeof(T).FullName ?? string.Empty] = value;
    }
}
