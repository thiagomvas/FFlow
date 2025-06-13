using FFlow.Core;

namespace FFlow.Demo;

public class LoggerStepDecorator : BaseStepDecorator
{
    public LoggerStepDecorator(IFlowStep innerStep) : base(innerStep)
    {
    }
    
    public override async Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Starting step: {_innerStep.GetType().Name}");
        await _innerStep.RunAsync(context, cancellationToken);
        Console.WriteLine($"Finished step: {_innerStep.GetType().Name}");
    }
}