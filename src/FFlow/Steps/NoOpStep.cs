using FFlow.Core;

namespace FFlow;

[StepName("No Operation")]
[StepTags("built-in")]
[SilentStep]
internal class NoOpStep : FlowStep
{
    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}