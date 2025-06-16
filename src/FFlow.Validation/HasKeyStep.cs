using FFlow.Core;

namespace FFlow.Validation;

public class HasKeyStep : IFlowStep
{
    private readonly string[] _key;

    public HasKeyStep(params string[] key)
    {
        _key = key ?? throw new ArgumentNullException(nameof(key));
    }

    public Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        cancellationToken.ThrowIfCancellationRequested();

        foreach (var key in _key)
        {
            if (!context.TryGet<object>(key, out _))
            {
                throw new KeyNotFoundException($"Key '{key}' not found in the context.");
            }
        }

        return Task.CompletedTask;
    }
}