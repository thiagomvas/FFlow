using FFlow;
using FFlow.Core;
using FFlow.Demo;
using FFlow.Steps.DotNet;
using FFlow.Validation;

var workflow = new FFlowBuilder()
    .UseValidators()
    .StartWith((ctx, _) => ctx.Set("name", "David"))
    .Then<HelloStep>()
        .Input<HelloStep, string>(step => step.Name, ctx => ctx.Get<string>("name"))
        .Output<HelloStep, string>(step => step.Name, (ctx, name) => ctx.Set("name2", name))
    .Then<GoodByeStep>()
        .Input<GoodByeStep, string>(step => step.Name, ctx => ctx.Get<string>("name2"))
    .Build();

await workflow.RunAsync("input", new CancellationTokenSource(2000).Token);