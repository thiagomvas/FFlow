using FFlow.Core;

namespace FFlow;

internal class OutputSetterStep : IFlowStep
{
    private readonly List<Action<IFlowContext>> _outputWriters = new();

    public OutputSetterStep(IEnumerable<Action<IFlowContext>> outputWriters)
    {
        _outputWriters.AddRange(outputWriters ?? throw new ArgumentNullException(nameof(outputWriters)));
    }

    public Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        cancellationToken.ThrowIfCancellationRequested();

        foreach (var writer in _outputWriters)
        {
            writer(context);
        }

        return Task.CompletedTask;
    }
}