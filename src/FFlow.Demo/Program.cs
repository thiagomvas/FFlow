using System.Reflection;
using FFlow;
using FFlow.Core;
using FFlow.Demo;
using FFlow.Extensions;
using FFlow.Observability.Listeners;
using FFlow.Observability.Metrics;

var sink = new InMemoryMetricsSink();
var workflow = new FFlowBuilder()
    .WithOptions(options =>
    {
        options.WithEventListener(new MetricTrackingListener(sink));
    })
    .StartWith<HelloStep>().Input<HelloStep, string>(step => step.Name, "World")
    .Delay(1000)
    .Then<GoodByeStep>().Input<GoodByeStep, string>(step => step.Name, "World")
    .Build();
    
await workflow.RunAsync("", CancellationToken.None);

Console.WriteLine("Workflow completed. Counters collected:");
foreach (var metric in sink.GetCounters())
{
    Console.WriteLine($"{metric.Key}: {metric.Value}");
}
Console.WriteLine("==========================================\nTimings collected:");

foreach (var timing in sink.GetTimings())
{
    var values = timing.Value;
    var avg = values.Count > 0 ? TimeSpan.FromMilliseconds(values.Average(t => t.TotalMilliseconds)) : TimeSpan.Zero;
    Console.WriteLine($"{timing.Key}: Count={values.Count}, Avg={avg.TotalMilliseconds:F2}ms");
}

