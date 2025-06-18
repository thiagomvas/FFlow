using FFlow.Core;
using FFlow.Validation.Annotations;

namespace FFlow.Demo;

[RequireKey("name")]
[RequireNotNull("name")]
[RequireRegex("name", @"^[A-Za-z\s]+$")]
public class HelloStep : IFlowStep
{
    public Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (context == null) throw new ArgumentNullException(nameof(context));

        var input = context.Get<string>("name") ?? context.GetInput<string>();
        
        return Task.Run(() =>
        {
            Console.WriteLine($"Hello, {input}!");
        }, cancellationToken);
    }
}