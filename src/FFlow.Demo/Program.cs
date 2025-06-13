using FFlow;
using FFlow.Core;
using FFlow.Demo;
using FFlow.Steps.DotNet;

var workflow = new FFlowBuilder()
    .WithOptions(options =>
    {
        options.AddStepDecorator(step => new LoggerStepDecorator(step));
        options.GlobalTimeout = TimeSpan.FromMilliseconds(1750);
        options.StepTimeout = TimeSpan.FromMilliseconds(500);
    })
    .StartWith((_, _) => Task.Run(() => Console.WriteLine("Starting")))
    .Fork(ForkStrategy.FireAndForget, () => new FFlowBuilder()
        .Then((_, _) => Console.WriteLine("Task 1")),
        () => new FFlowBuilder()
            .Then((_, _) => throw new Exception("Task 2 threw an exception"))
            .Throw<AccessViolationException>("Simulated access violation exception"),
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