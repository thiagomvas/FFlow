using System.Collections.Concurrent;
using FFlow.Core;

namespace Workflow.Tests.Shared;

public class TestFlowContext : IFlowContext
{
    private readonly ConcurrentDictionary<string, object?> _data = new();
    private object? _input;
    public Guid Id { get; private set; } = Guid.NewGuid();

    public TInput GetInput<TInput>()
    {
        if (_input is TInput typedInput)
            return typedInput;

        throw new InvalidOperationException($"Input of type {typeof(TInput).Name} was not set.");
    }

    public void SetInput<TInput>(TInput input)
    {
        _input = input;
    }

    public T Get<T>(string key)
    {
        if (!_data.TryGetValue(key, out var value))
            throw new KeyNotFoundException($"Key '{key}' was not found in context.");

        return value is T typedValue
            ? typedValue
            : throw new InvalidCastException($"Stored value for key '{key}' cannot be cast to {typeof(T).Name}.");
    }

    public void Set<T>(string key, T value)
    {
        _data[key] = value;
    }

    public bool TryGet<T>(string key, out T value)
    {
        if (_data.TryGetValue(key, out var raw) && raw is T typed)
        {
            value = typed;
            return true;
        }

        value = default!;
        return false;
    }

    public IFlowContext Fork()
    {
        var clone = new TestFlowContext
        {
            _input = _input,
            Id = Guid.NewGuid()
        };

        foreach (var kvp in _data)
            clone._data[kvp.Key] = kvp.Value;

        return clone;
    }

    public IFlowContext SetId(Guid id)
    {
        Id = id;
        return this;
    }
    
    public IEnumerable<KeyValuePair<string, object>> GetAll()
    {
        foreach (var kvp in _data)
        {
            yield return new KeyValuePair<string, object>(kvp.Key, kvp.Value!);
        }
    }
}