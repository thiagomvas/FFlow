using FFlow.Core;

namespace FFlow;

[StepName("Stop Execution If")]
[StepTags("built-in")]
internal class StopExecutionIfStep : FlowStep
{
    private readonly Func<IFlowContext, bool> _condition;
    
    public StopExecutionIfStep(Func<IFlowContext, bool> condition)
    {
        _condition = condition ?? throw new ArgumentNullException(nameof(condition));
    }
    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        cancellationToken.ThrowIfCancellationRequested();

        if (_condition(context))
        {
            context.GetSingleValue<IWorkflow>()?.StopGracefully();
        }

        return Task.CompletedTask;
    }
}