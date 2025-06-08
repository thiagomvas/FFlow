using FFlow;
using FFlow.Core;
using FFlow.Demo;
using FFlow.Extensions.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

var workflow = new FFlowBuilder()
    .If(ctx => false,
        then: ctx => Task.Run(() => Console.WriteLine("true")),
        otherwise: ctx => Task.Run(() => Console.WriteLine("false")))
    .Build();

await workflow.RunAsync(null);
