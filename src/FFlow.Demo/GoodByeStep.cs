using FFlow.Core;

namespace FFlow.Demo;

public class GoodByeStep : IFlowStep
{
    public Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Task.Run(() =>
        {
            Console.WriteLine($"Goodbye!");
        }, cancellationToken);
    }
}