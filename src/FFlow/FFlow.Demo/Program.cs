using FFlow;
using FFlow.Core;
using FFlow.Demo;
using FFlow.Extensions.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

var workflow = new FFlowBuilder()
    .StartWith((ctx, ct) => Task.Delay(1000, ct))
    .OnAnyError((ctx, ct) => 
    {
        Console.WriteLine("An error occurred in the workflow.");
        return Task.CompletedTask;
    })
    .Build();

await workflow.RunAsync(null, new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);
