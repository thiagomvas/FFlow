using System.Collections.Concurrent;
using FFlow.Core;

namespace FFlow;

public class InMemoryMetadataStore : IWorkflowMetadataStore
{
    private readonly ConcurrentDictionary<string, object?> _data = new();

    public void Set<T>(string key, T value)
    {
        _data[key] = value;
    }

    public T? Get<T>(string key)
    {
        if (_data.TryGetValue(key, out var obj) && obj is T t)
            return t;
        return default;
    }

    public bool TryGet<T>(string key, out T? value)
    {
        if (_data.TryGetValue(key, out var obj) && obj is T t)
        {
            value = t;
            return true;
        }
        value = default;
        return false;
    }

    public bool HasKey(string key)
    {
        return _data.ContainsKey(key);
    }
}