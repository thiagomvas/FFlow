using FFlow;
using FFlow.Core;
using FFlow.Demo;
using FFlow.Extensions.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

var workflow = new FFlowBuilder()
    .StartWith((ctx, ct) => Task.Run(() => ctx.Set("enumerable", Enumerable.Range(1, 10).Select(i => i * i).Cast<object>()), ct))
    .ForEach(ctx => ctx.Get<IEnumerable<object>>("enumerable"), 
        (ctx, ct) =>
    {
        var item = ctx.GetInput<int>();
        Console.WriteLine($"Processing item: {item}");
        return Task.CompletedTask;
    })
    .Build();

await workflow.RunAsync(null, new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);
