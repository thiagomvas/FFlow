using FFlow;
using FFlow.Demo;
using FFlow.Extensions;
using FFlow.Scheduling;
using FFlow.Steps.DotNet;
using FFlow.Steps.Shell;

var store = new InMemoryFlowScheduleStore();
var runner = new FFlowScheduleRunner(store);

await store.AddAsync(ScheduledWorkflow.CreateRecurring(new HelloWorkflow(),
    "* * * * *"));

Console.WriteLine("Scheduled workflow to run in 1 min...");
await runner.StartAsync(CancellationToken.None);

Console.WriteLine("Press any key to exit...");
Console.ReadKey();