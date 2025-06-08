using FFlow.Core;

namespace FFlow;

public class IfStep : IFlowStep
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

    public Task RunAsync(IFlowContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (_condition == null) throw new InvalidOperationException("Condition must be set.");
        if (_trueStep == null) throw new InvalidOperationException("True step must be set.");

        if (_condition(context))
        {
            return _trueStep.RunAsync(context);
        }
        else
        {
            return _falseStep?.RunAsync(context) ?? Task.CompletedTask;
        }
    }
}