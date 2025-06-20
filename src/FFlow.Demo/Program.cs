using System.Reflection;
using FFlow;
using FFlow.Core;
using FFlow.Extensions;

var workflow = new FFlowBuilder()
    .StartWith((ctx, ct) =>
    {
        Console.WriteLine("Starting workflow...");
        throw new AbandonedMutexException();
    })
    .WithRetryPolicy(RetryPolicies.FixedDelay(5, TimeSpan.FromSeconds(3)))
    .OnAnyError((_, _) =>
    {
        Console.WriteLine("An error occurred");
    })
    .Build();
    
await workflow.RunAsync("", CancellationToken.None);