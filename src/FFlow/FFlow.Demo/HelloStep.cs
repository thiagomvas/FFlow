using FFlow.Core;

namespace FFlow.Demo;

public class HelloStep : IFlowStep
{
    public Task RunAsync(IFlowContext context)
    {
        Console.WriteLine("Hello, World!");
        return Task.CompletedTask;
    }
}