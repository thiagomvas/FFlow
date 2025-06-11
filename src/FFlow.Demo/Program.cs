using FFlow;
using FFlow.Core;
using FFlow.Demo;
using FFlow.Steps.DotNet;

var workflow = new FFlowBuilder()
    .StartWith((_, _) => Task.Run(() => Console.WriteLine("Starting")))
    .Fork(ForkStrategy.FireAndForget, () => new FFlowBuilder()
        .Then((_, _) => Console.WriteLine("Task 1")),
        () => new FFlowBuilder()
            .Then((_, _) => throw new Exception("Task 2 threw an exception"))
            .Then((_, _) => Console.WriteLine("Task 2")),
        () => new FFlowBuilder()
            .Then((_, _) => throw new Exception("Task 3 threw an exception"))
            .Then((_, _) => Console.WriteLine("Task 3")))
    .Then<HelloStep>()
    .Build();

await workflow.RunAsync(null, new CancellationTokenSource(2150).Token);