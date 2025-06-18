using FFlow;
using FFlow.Core;
using FFlow.Demo;
using FFlow.Steps.DotNet;
using FFlow.Validation;

var workflow = new FFlowBuilder()
    .UseValidators()
    .StartWith((ctx, _) => ctx.Set("name", "John Doe"))
    .Then<HelloStep>()
    .Then<GoodByeStep>()
    .Build();

await workflow.RunAsync("input", new CancellationTokenSource(2000).Token);