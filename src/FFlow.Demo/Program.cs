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
    .AddFFlow()
    .AddSingleton<InMemoryMetricsSink>()
    .AddSingleton<MetricTrackingListener<InMemoryMetricsSink>>()
    .BuildServiceProvider();

var workflow = services.GetRequiredService<HelloWorkflow>().Build();
var ctx = await workflow.RunAsync("input");

// Get the InMemoryMetricsSink to access the metrics
var sink = services.GetRequiredService<InMemoryMetricsSink>();


