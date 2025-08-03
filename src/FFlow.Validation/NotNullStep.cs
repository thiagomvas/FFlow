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
    {
        ArgumentNullException.ThrowIfNull(context);
        cancellationToken.ThrowIfCancellationRequested();

        foreach (var key in _keys)
        {
            if (context.GetValue<object>(key) is null)
            {
                throw new FlowValidationException($"Key '{key}' not found or is null in the context.");
            }
        }

        return Task.CompletedTask;
    }
}