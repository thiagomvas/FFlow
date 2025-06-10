using FFlow;
using FFlow.Core;
using FFlow.Demo;
using FFlow.Steps.DotNet;

var workflow = new FFlowBuilder()
    .StartWith((_, _) => Task.Run(() => Console.WriteLine("Starting")))
    .Fork(ForkStrategy.FireAndForget, () => new FFlowBuilder()
            .Then((_, _) => Task.Run(() => Console.WriteLine("Task 1"))),
        () => new FFlowBuilder()
            .Then((_, _) => Task.Run(() => Console.WriteLine("Task 2"))),
        () => new FFlowBuilder()
            .Then((_, _) => Task.Run(() => throw new Exception("Will")))
            .Then((_, _) => Task.Run(() => Console.WriteLine("Task 3"))))
    .Then<HelloStep>()
    .Then((_, _) => Task.Delay(500))
    .Then<HelloStep>()
    .Build();

await workflow.RunAsync(null, CancellationToken.None);