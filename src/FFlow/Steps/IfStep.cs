using FFlow.Core;

namespace FFlow;

[StepName("If Condition")]
[StepTags("built-in")]
[SilentStep]
public class IfStep : FlowStep
{
    private readonly Func<IFlowContext, bool> _condition;
    private readonly IFlowStep _trueStep;
    private readonly IFlowStep? _falseStep;

    public IfStep(Func<IFlowContext, bool> condition, IFlowStep trueStep, IFlowStep? falseStep = null)
    {
        _condition = condition ?? throw new ArgumentNullException(nameof(condition));
        _trueStep = trueStep ?? throw new ArgumentNullException(nameof(trueStep));
        _falseStep = falseStep;
    }

    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (_condition == null) throw new InvalidOperationException("Condition must be set.");
        if (_trueStep == null) throw new InvalidOperationException("True step must be set.");

        cancellationToken.ThrowIfCancellationRequested();
        
        if (_condition(context))
        {
            return _trueStep.RunAsync(context, cancellationToken);
        }
        else
        {
            return _falseStep?.RunAsync(context, cancellationToken) ?? Task.CompletedTask;
        }
    }

    public override Task CompensateAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (_trueStep == null) throw new InvalidOperationException("True step must be set for compensation.");

        cancellationToken.ThrowIfCancellationRequested();
        bool res = _condition(context);
        if (res && _trueStep is ICompensableStep trueStep)
        {
            return trueStep.CompensateAsync(context, cancellationToken);
        }
        if (!res && _falseStep is ICompensableStep falseStep)
        {
            return falseStep.CompensateAsync(context, cancellationToken);
        }
        
        return Task.CompletedTask;
    }
}