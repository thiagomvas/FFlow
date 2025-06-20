using System.Reflection;
using FFlow;
using FFlow.Core;
using FFlow.Extensions;

var workflow = new FFlowBuilder()
    .StartWith((ctx, ct) => throw new AbandonedMutexException())
    .WithRetryPolicy(RetryPolicies.FixedDelay(5, TimeSpan.FromSeconds(3)))
    .Build();
    
await workflow.RunAsync("", CancellationToken.None);