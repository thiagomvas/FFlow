using FFlow.Core;

namespace FFlow;

[StepName("Throw Exception If")]
[StepTags("built-in")]
[SilentStep]
public class ThrowExceptionStep : IFlowStep
{
    private readonly Exception _exception;
    public ThrowExceptionStep(Exception exception)
    {
        _exception = exception ?? throw new ArgumentNullException(nameof(exception));
    }
    
    public ThrowExceptionStep(string message)
    {
        _exception = new Exception(message ?? throw new ArgumentNullException(nameof(message)));
    }
    
    
    public Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        if (context is null)
            throw new ArgumentNullException(nameof(context));
        
        context.SetSingleValue(_exception);
        return Task.FromException(_exception);
    }
}