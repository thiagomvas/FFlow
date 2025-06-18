using FFlow.Core;

namespace FFlow;

public class DelayStep : IFlowStep
{
    private readonly TimeSpan _delay;
    
    public DelayStep(int milliseconds)
    {
        _delay = TimeSpan.FromMilliseconds(milliseconds);
    }
    
    public DelayStep(TimeSpan delay)
    {
        _delay = delay;
    }


    public Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        return Task.Delay(_delay, cancellationToken);
    }
}