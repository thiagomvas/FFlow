# Observability, Metrics and Logging
Observability in FFlow is done through the use of the `IFlowEventListener` interface, which allows you to listen to various events in the workflow execution process. This can be used to implement logging, metrics collection, or any other form of observability.

The package `FFlow.Observability` provides a set of utilities like **Metrics Sinks**, **Loggers**, **Timeline Recorders** and others. 

```bash
dotnet add package FFlow.Observability
```

## Metrics Sinks
Metrics Sinks are used to collect and report metrics during the workflow execution. You can implement your own metrics sink by implementing the `IMetricsSink` interface.

For an example implementation, see InMemoryMetricsSink.

```csharp
var sink = new InMemoryMetricsSink();
var workflow = new FFlowBuilder()
    .UseMetrics(sink)
    .StartWith<HelloStep>().Input<HelloStep, string>(step => step.Name, "World")
    .Delay(1000)
    .Then<GoodByeStep>().Input<GoodByeStep, string>(step => step.Name, "World")
    .Build();

var ctx = await workflow.RunAsync("", CancellationToken.None);

// If you don't have the reference to the sink, you can get it from the context
Console.WriteLine("Workflow completed. Counters collected:");
foreach (var metric in ctx.GetMetricsSink<InMemoryMetricsSink>().GetCounters())
{
    Console.WriteLine($"{metric.Key}: {metric.Value}");
}
Console.WriteLine("==========================================\nTimings collected:");

foreach (var timing in ctx.GetMetricsSink<InMemoryMetricsSink>().GetTimings())
{
    var values = timing.Value;
    var avg = values.Count > 0 ? TimeSpan.FromMilliseconds(values.Average(t => t.TotalMilliseconds)) : TimeSpan.Zero;
    Console.WriteLine($"{timing.Key}: Count={values.Count}, Avg={avg.TotalMilliseconds:F2}ms");
}
```


## Loggers
Loggers are used to log events during the workflow execution. You can implement your own logger by implementing the `IFlowEventListener` interface like normal.

## Timeline Recorder
The Timeline Recorder is used to record the execution time of each step in the workflow. It can be useful for performance monitoring and optimization. It is just an `IFlowEventListener` that records the start and end times of each step and calculates the duration.

```csharp
var timelineRecorder = new TimelineRecorder();
var workflow = new FFlowBuilder()
    .WithOptions(o => o.WithEventListener(timelineRecorder))
    .StartWith<HelloStep>().Input<HelloStep, string>(step => step.Name, "World")
    .Delay(1000)
    .Then<GoodByeStep>().Input<GoodByeStep, string>(step => step.Name, "World")
    .Build();

var ctx = await workflow.RunAsync("", CancellationToken.None);

// If you don't have the reference to any event listener, you can get it from the context
foreach (var entry in ctx.GetEventListener<TimelineRecorder>()?.GetTimeline() ?? [])
{
    Console.WriteLine(entry);
}
```

## Dependency Injection
All of the observability components can be registered in the Dependency Injection container. You can register your custom metrics sinks, loggers, and timeline recorders using the `IServiceCollection` interface.

Each component also supports dependency injection, allowing you to inject services and other dependencies into your observability components.

```csharp
// All the observability components can be registered in the DI container
var services = new ServiceCollection()
    .AddFFlow()
    .AddSingleton<InMemoryMetricsSink>()
    .AddSingleton<MetricTrackingListener<InMemoryMetricsSink>>()
    .BuildServiceProvider();

var workflow = services.GetRequiredService<HelloWorkflow>().Build();
var ctx = await workflow.RunAsync("input");

// Get the InMemoryMetricsSink to access the metrics
var sink = services.GetRequiredService<InMemoryMetricsSink>();
```