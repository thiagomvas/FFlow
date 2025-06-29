using FFlow;
using FFlow.Demo;
using FFlow.Extensions;
using FFlow.Extensions.Microsoft.DependencyInjection;
using FFlow.Observability.Listeners;
using FFlow.Observability.Metrics;
using FFlow.Scheduling;
using FFlow.Steps.DotNet;
using FFlow.Steps.Shell;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var services = new ServiceCollection();
services.AddFFlow(typeof(HelloWorkflow).Assembly)
    .AddLogging(builder => builder.AddConsole())
    .AddSingleton<InMemoryMetricsSink>()
    .AddSingleton<MetricTrackingListener<InMemoryMetricsSink>>();
services.AddFflowScheduling(builder =>
{
    builder.AddWorkflow<HelloWorkflow>().RunEvery(TimeSpan.FromSeconds(15));
});

var serviceProvider = services.BuildServiceProvider();
var runner = serviceProvider.GetRequiredService<FFlowScheduleRunner>();
await runner.StartAsync(CancellationToken.None);
Console.WriteLine("Press any key to exit...");
Console.ReadKey();

await runner.StopAsync(CancellationToken.None);

Console.WriteLine("Scheduler stopped. Flushing metrics.");

var metrics = serviceProvider.GetService<InMemoryMetricsSink>();

if (metrics == null)
{
    Console.WriteLine("No metrics sink registered.");
    return;
}

var snapshot = metrics.Flush();

Console.WriteLine("Counters:");
foreach (var counter in snapshot.Counters)
{
    Console.WriteLine($"- {counter.Key}: {counter.Value}");
}

Console.WriteLine("Gauges:");
foreach (var gauge in snapshot.Gauges)
{
    Console.WriteLine($"- {gauge.Key}: {gauge.Value}");
}

Console.WriteLine("Timings:");
foreach (var timer in snapshot.Timings)
{
    Console.WriteLine($"- {timer.Key}: {timer.Value.Sum(t => t.TotalMilliseconds) / timer.Value.Count} ms (Count: {timer.Value.Count})");
}
