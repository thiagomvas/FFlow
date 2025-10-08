using FFlow.Core;
using FFlow.Validation.Annotations;

namespace FFlow.Demo;

[RequireKey("name")]
[RequireNotNull("name")]
[RequireRegex("name", @"^[A-Za-z\s]+$")]
public class HelloStep : FlowStep
{
    public string Name { get; set; }
    protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentNullException.ThrowIfNull(context);
        
        return Task.Run(() =>
        {
            Console.WriteLine($"Hello, {Name}!");
        }, cancellationToken);
    }
}