# Workflow Scheduling
The package `FFlow.Scheduling` provides a way to schedule workflows to run at specific times or intervals. This is useful for scenarios where you need to execute workflows periodically or at a specific time.

```bash
dotnet add package FFlow.Scheduling
```

To schedule, initialize a `IFlowScheduleStore` to store the scheduled workflows. You can use the `InMemoryFlowScheduleStore` for testing or development purposes, or implement your own store for production use. There is a `FileFlowScheduleStore` that can be used to store schedules in a file.

After that, you can create instances of `ScheduledWorkflow` by passing a `IWorkflowDefinition` to the factory methods.

## Usage
```csharp
using FFlow.Scheduling;

var store = new InMemoryFlowScheduleStore();
var scheduled = ScheduledWorkflow.CreateRecurring(
    new HelloWorkflow(), // Your workflow definition
    TimeSpan.FromMinutes(5), // Interval
);

var oneTime = ScheduledWorkflow.CreateOneTime(
    new HelloWorkflow(), // Your workflow definition
    DateTime.UtcNow.AddMinutes(10) // Scheduled time
);

var withCron = ScheduledWorkflow.CreateCron(
    new HelloWorkflow(), // Your workflow definition
    "0 0 * * *" // Cron expression for every hour
);

await store.AddAsync(scheduled);
await store.AddAsync(oneTime);
await store.AddAsync(withCron);

// To run the scheduled workflows, you can use the background service provided by the package.
var service = new FFlowScheduleRunner(store);
await service.RunAsync(cancellationToken);
```

## Dependency Injection and Service Collection
The package provides extensions and utilities for scheduling workflows using dependency injection. Scheduling becomes simpler when using the `AddFFlowScheduling` extension methods for `IServiceCollection`
```csharp
using FFlow.Scheduling;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddFlow(); // You have to register the FFlow core services first (definitions and steps)
services.AddFflowScheduling(builder =>
{
    builder.AddWorkflow<HelloWorkflow>().RunEvery(TimeSpan.FromSeconds(1));
});
```

It will automatically register a `FFlowScheduleRunner` as a hosted service that will run the scheduled workflows in the background. You may optionally pass a type parameter to set which `IFlowScheduleStore` implementation to use, defaulting to `InMemoryFlowScheduleStore`.

```csharp
services.AddFflowScheduling<FileFlowScheduleStore>(builder =>
{
    builder.AddWorkflow<HelloWorkflow>().RunEvery(TimeSpan.FromSeconds(1));
});
```

> [!WARNING]
> Make sure to register and configure the `IFlowScheduleStore` before adding the scheduling services. The `FFlowScheduleRunner` will use the store to retrieve and execute scheduled workflows.

### Configuring the Runner
You can configure the `FFlowScheduleRunner` by calling the `Configure<FFlowScheduleRunnerOptions>` method on the `IServiceCollection`. 
```csharp
services.Configure<FFlowScheduleRunnerOptions>(options =>
{
    options.PollingInterval = TimeSpan.FromSeconds(10);
});
```