using FFlow.Core;

namespace FFlow;

public class ThrowExceptionIfStep : IFlowStep
{
    private readonly Func<IFlowContext, bool> _condition;
    private readonly Exception _exception;
    
    
    public ThrowExceptionIfStep(Func<IFlowContext, bool> condition, Exception exception)
    {
        _condition = condition ?? throw new ArgumentNullException(nameof(condition));
        _exception = exception ?? throw new ArgumentNullException(nameof(exception));
    }
    
    public ThrowExceptionIfStep(Func<IFlowContext, bool> condition, string message)
    {
        _condition = condition ?? throw new ArgumentNullException(nameof(condition));
        _exception = new Exception(message ?? throw new ArgumentNullException(nameof(message)));
    }
    public Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (!_condition(context)) return Task.CompletedTask;
        
        context.Set("Exception", _exception);
        return Task.FromException(_exception);

    }
}