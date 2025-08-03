using FFlow.Core;
using FFlow.Validation.Annotations;

namespace FFlow.Demo;

[RequireKey("name")]
[RequireNotNull("name")]
[RequireRegex("name", @"^[A-Za-z\s]+$")]
public class HelloStep : IFlowStep
{
    public string Name { get; set; }
    public Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentNullException.ThrowIfNull(context);

        var input = context.GetValue<string>("name") ?? context.GetLastOutput<string>();
        
        return Task.Run(() =>
        {
            Console.WriteLine($"Hello, {Name}!");
        }, cancellationToken);
    }
}