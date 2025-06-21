using System.Reflection;
using FFlow;
using FFlow.Core;
using FFlow.Demo;
using FFlow.Extensions;
using FFlow.Extensions.Microsoft.DependencyInjection;
using FFlow.Observability.Extensions;
using FFlow.Observability.Listeners;
using FFlow.Observability.Metrics;
using Microsoft.Extensions.DependencyInjection;

var workflow = new FFlowBuilder()
    .Fork(ForkStrategy.FireAndForget, () => new FFlowBuilder()
        .Then((_, _) => Console.WriteLine("1"))
        .Then((_, _) => Console.WriteLine("2")),
    () => new FFlowBuilder()
        .Then((_, _) => Console.WriteLine("3")))
    .Throw<Exception>("Simulated")
    .Build();
    
    await workflow.RunAsync("");