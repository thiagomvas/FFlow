using FFlow.Core;

namespace FFlow.Validation;

/// <summary>
/// Checks if the specified keys exist in the flow context.
/// </summary>
internal class NotNullStep : IFlowStep
{
    private readonly string[] _keys;
    
    public NotNullStep(params string[] keys)
    {
        if (keys == null || keys.Length == 0) throw new ArgumentException("Keys cannot be null or empty.", nameof(keys));
        
        foreach (var key in keys)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Key cannot be null or empty.", nameof(keys));
        }
        
        _keys = keys;
    }
    public Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {        if (context == null) throw new ArgumentNullException(nameof(context));
        cancellationToken.ThrowIfCancellationRequested();

        foreach (var key in _keys)
        {
            if (!context.TryGet<object>(key, out var obj))
            {
                throw new FlowValidationException($"Key '{key}' not found in the context.");
            }
            
            if (obj == null)
            {
                throw new FlowValidationException($"Value for key '{key}' cannot be null.");
            }
        }

        return Task.CompletedTask;
    }
}