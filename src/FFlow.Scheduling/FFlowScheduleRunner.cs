using Microsoft.Extensions.Hosting;

namespace FFlow.Scheduling;

public class FFlowScheduleRunner : BackgroundService
{
    private readonly IFlowScheduleStore _flowScheduleStore;

    public FFlowScheduleRunner(IFlowScheduleStore flowScheduleStore)
    {
        _flowScheduleStore = flowScheduleStore ?? throw new ArgumentNullException(nameof(flowScheduleStore));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("FFlow Schedule Runner started.");

        var allWorkflows = await _flowScheduleStore.GetAllAsync(stoppingToken);
        foreach (var workflow in allWorkflows)
        {
            Console.WriteLine($"Loaded scheduled workflow: {workflow.Workflow.GetType().Name} " +
                              $"(Next execution: {workflow.ExecuteAt}, Recurring: {workflow.Recurring})");
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var now = DateTimeOffset.UtcNow;
                var dueWorkflows = await _flowScheduleStore.GetDueAsync(now, stoppingToken);

                foreach (var workflow in dueWorkflows)
                {
                    Console.WriteLine($"Executing workflow: {workflow.Workflow.GetType().Name} at {now}");
                    await workflow.Workflow.Build().RunAsync("", stoppingToken);

                    if (workflow.Recurring)
                        workflow.UpdateNextExecution(now);
                    else
                        await _flowScheduleStore.RemoveAsync(workflow, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing scheduled workflows: {ex.Message}");
            }

            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }
    }
}