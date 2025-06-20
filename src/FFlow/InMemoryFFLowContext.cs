using FFlow.Core;

namespace FFlow;

public class InMemoryFFLowContext : IFlowContext
{

    private readonly Dictionary<string, object> _storage = new();
    public Guid Id { get; private set; } = Guid.NewGuid();

    public InMemoryFFLowContext()
    {
        
    }
    public InMemoryFFLowContext(Dictionary<string, object> storage)
    {
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
    }

    public InMemoryFFLowContext(Dictionary<string, object> storage, Guid Id)
    {
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        this.Id = Id;
    }

    public TInput GetInput<TInput>()
    {
        if (_storage.TryGetValue(Internals.FFlowContextInputKey, out var value))
        {
            if (value is TInput typedValue)
            {
                return typedValue;
            }
            throw new InvalidCastException($"Stored input is not of type {typeof(TInput).Name}.");
        }
        return default; // Return default value if input is not set
    }

    public void SetInput<TInput>(TInput input)
    {
        _storage[Internals.FFlowContextInputKey] = input;
    }

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

        return default;
    }

    public void Set<T>(string key, T value)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        if (value == null) throw new ArgumentNullException(nameof(value));

        _storage[key] = value;
    }

    public bool TryGet<T>(string key, out T value)
    {
        if (_storage.TryGetValue(key, out var storedValue))
        {
            if (storedValue is T typedValue)
            {
                value = typedValue;
                return true;
            }
            value = default;
            return false; // Type mismatch
        }
        value = default;
        return false; // Key not found
    }

    public IFlowContext Fork()
    {
        return new InMemoryFFLowContext(_storage.ToDictionary(), Id);
    }

    public IFlowContext SetId(Guid id)
    {
        Id = id;
        return this;
    }
    
    public IEnumerable<KeyValuePair<string, object>> GetAll()
    {
        return _storage.AsEnumerable();
    }
}