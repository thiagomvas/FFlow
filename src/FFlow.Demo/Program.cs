using FFlow;
using FFlow.Core;
using FFlow.Demo;
using FFlow.Steps.DotNet;

var workflow = new FFlowBuilder()
    .StartWith((_, _) => Task.Run(() => Console.WriteLine("Starting")))
    .Fork(ForkStrategy.FireAndForget, () => new FFlowBuilder()
        .Then((_, _) => Console.WriteLine("Task 1")),
        () => new FFlowBuilder()
            .Then((_, _) => Task.Delay(1000))
            .Then((_, _) => Console.WriteLine("Task 2")),
        () => new FFlowBuilder()
            .Then((_, _) => Console.WriteLine("Task 3")))
    .Then<HelloStep>()
    .OnAnyError((context, token) =>
    {
        Console.WriteLine("An error occurred:");
        var ex = context.Get<Exception>("Exception");
        if (ex is not null)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        else
        {
            Console.WriteLine("No error information available.");
        }
        
        return Task.CompletedTask;
        
    })
    .Build();

await workflow.RunAsync(null, new CancellationTokenSource(2000).Token);