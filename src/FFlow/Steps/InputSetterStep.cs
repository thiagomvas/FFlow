using FFlow.Core;

namespace FFlow;

[StepName("Input Setter")]
[StepTags("built-in")]
[SilentStep]
internal class InputSetterStep : IFlowStep
{
    private readonly IEnumerable<Action<IFlowContext>> _inputSetters;
    
    public InputSetterStep(IEnumerable<Action<IFlowContext>> inputSetters)
    {
        _inputSetters = inputSetters ?? throw new ArgumentNullException(nameof(inputSetters), "Input setters cannot be null.");
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