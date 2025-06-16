using FFlow.Core;

namespace FFlow;

public class HasKeyStep : IFlowStep
{
    private readonly string _key;
    
public HasKeyStep(string key)
    {
        _key = key ?? throw new ArgumentNullException(nameof(key));
    }
    public Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        cancellationToken.ThrowIfCancellationRequested();

        // Check if the key exists in the context
        if (!context.TryGet<object>(_key, out _))
        {
            throw new KeyNotFoundException($"Key '{_key}' not found in the context.");
        }

        return Task.CompletedTask;
    }
}