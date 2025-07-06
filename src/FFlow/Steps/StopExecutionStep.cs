using FFlow.Core;

namespace FFlow;

[StepName("Stop Execution")]
[StepTags("built-in")]
internal class StopExecutionStep : FlowStep
{
    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        context.GetSingleValue<IWorkflow>()?.StopGracefully();
        return Task.CompletedTask;
    }
}