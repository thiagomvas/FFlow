using FFlow.Core;

namespace FFlow;

[StepName("Input Setter")]
[StepTags("built-in")]
[SilentStep]
internal class InputSetterStep : IFlowStep
{
    private readonly List<Action<IFlowContext>> _inputSetters = new();
    
    public InputSetterStep(IEnumerable<Action<IFlowContext>> inputSetters)
    {
        _inputSetters.AddRange(inputSetters ?? throw new ArgumentNullException(nameof(inputSetters)));
    }
    public Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        cancellationToken.ThrowIfCancellationRequested();

        foreach (var setter in _inputSetters)
        {
            setter(context);
        }

        return Task.CompletedTask;
    }
}