using System.Collections;
using FFlow.Core;

namespace FFlow.Validation;


internal class NotEmptyStep : IFlowStep
{
    private readonly string[] _keys;
    
    public NotEmptyStep(params string[] keys)
    {
        if (keys == null || keys.Length == 0) throw new ArgumentException("Keys cannot be null or empty.", nameof(keys));

        foreach (var key in keys)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Key cannot be null or empty.", nameof(keys));
        }

        _keys = keys;
    }
    public Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        cancellationToken.ThrowIfCancellationRequested();

        foreach (var key in _keys)
        {
            if (!context.TryGet<object>(key, out var obj))
            {
                throw new FlowValidationException($"Key '{key}' not found in the context.");
            }

            if (obj is string str && string.IsNullOrWhiteSpace(str))
            {
                throw new FlowValidationException($"Value for key '{key}' cannot be empty.");
            }
            
            if (obj is ICollection collection && collection.Count == 0)
            {
                throw new FlowValidationException($"Collection for key '{key}' cannot be empty.");
            }
        }

        return Task.CompletedTask;
    }
}