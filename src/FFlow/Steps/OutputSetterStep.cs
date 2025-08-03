using FFlow.Core;

namespace FFlow;

[StepName("Output Setter")]
[StepTags("built-in")]
[SilentStep]
internal class OutputSetterStep : IFlowStep
{
    internal readonly List<Action<IFlowContext>> _outputWriters = new();

    public OutputSetterStep(IEnumerable<Action<IFlowContext>> outputWriters)
    {
        _outputWriters.AddRange(outputWriters ?? throw new ArgumentNullException(nameof(outputWriters)));
    }

    public Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        cancellationToken.ThrowIfCancellationRequested();

        foreach (var writer in _outputWriters)
        {
            writer(context);
        }

        return Task.CompletedTask;
    }
}