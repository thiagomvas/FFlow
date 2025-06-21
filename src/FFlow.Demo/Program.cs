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

var services = new ServiceCollection()
    .AddFFlow(typeof(HelloWorkflow).Assembly)
    .AddSingleton<InMemoryMetricsSink>()
    .AddSingleton<MetricTrackingListener<InMemoryMetricsSink>>()
    .BuildServiceProvider();

var workflow = services.GetRequiredService<HelloWorkflow>().Build();
var ctx = await workflow.RunAsync("input");

// Get the InMemoryMetricsSink to access the metrics
var sink = services.GetRequiredService<InMemoryMetricsSink>();
var snapshot = sink.Flush();

Console.WriteLine("Metrics Snapshot:");
foreach (var counter in snapshot.Counters)
{
    Console.WriteLine($"Counter: {counter.Key}, Value: {counter.Value}");
}
foreach (var timing in snapshot.Timings)
{
    Console.WriteLine($"Timing: {timing.Key}, Values: {string.Join(", ", timing.Value)}");
}
foreach (var gauge in snapshot.Gauges)
{
    Console.WriteLine($"Gauge: {gauge.Key}, Value: {gauge.Value}");
}


