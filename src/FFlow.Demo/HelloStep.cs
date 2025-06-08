using FFlow.Core;

namespace FFlow.Demo;

public class HelloStep : IFlowStep
{
    public Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (context == null) throw new ArgumentNullException(nameof(context));

        var input = context.GetInput<object>();
        
        return Task.Run(() =>
        {
            Console.WriteLine($"Hello, {input}!");
        }, cancellationToken);
    }
}