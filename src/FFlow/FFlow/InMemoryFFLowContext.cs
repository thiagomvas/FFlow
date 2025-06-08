using FFlow.Core;

namespace FFlow;

public class InMemoryFFLowContext : IFlowContext
{
    private readonly Dictionary<string, object> _storage = new();
    
    public T Get<T>(string key)
    {
        if (_storage.TryGetValue(key, out var value))
        {
            if (value is T typedValue)
            {
                return typedValue;
            }
            throw new InvalidCastException($"Stored value for key '{key}' is not of type {typeof(T).Name}.");
        }
        throw new KeyNotFoundException($"Key '{key}' not found in context.");
    }

    public void Set<T>(string key, T value)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        if (value == null) throw new ArgumentNullException(nameof(value));

        _storage[key] = value;
    }
}