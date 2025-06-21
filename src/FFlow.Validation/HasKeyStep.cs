using FFlow.Core;

namespace FFlow.Validation;

/// <summary>
/// Checks if the specified keys exist in the flow context.
/// </summary>
internal class HasKeyStep : IFlowStep
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
            if (context.GetValue<object>(key) is null)
            {
                throw new FlowValidationException($"Key '{key}' not found in the context.");
            }
        }

        return Task.CompletedTask;
    }
}