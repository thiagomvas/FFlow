using FFlow.Core;

namespace FFlow.Demo;

public class HelloStep : IFlowStep
{
    private readonly string _name;
    public HelloStep(ServiceA service) 
    {
        _name = service.Name;
    }
    public Task RunAsync(IFlowContext context)
    {
        Console.WriteLine($"Hello, {_name}!");
        return Task.CompletedTask;
    }
}