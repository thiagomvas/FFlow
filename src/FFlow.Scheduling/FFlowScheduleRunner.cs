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
       while (!stoppingToken.IsCancellationRequested) 
        {
            try
            {
                var now = DateTimeOffset.UtcNow;
                var dueWorkflows = await _flowScheduleStore.GetDueAsync(now, stoppingToken);
                
                foreach (var workflow in dueWorkflows)
                {
                    // Execute the workflow here
                    // This is a placeholder for actual execution logic
                    Console.WriteLine($"Executing workflow: {workflow.Workflow.GetType().Name} at {now}");
                    await workflow.Workflow.Build().RunAsync("", stoppingToken);
                    
                    // After execution, remove the workflow from the store
                    await _flowScheduleStore.RemoveAsync(workflow, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (logging, etc.)
                Console.WriteLine($"Error executing scheduled workflows: {ex.Message}");
            }

            // Wait for a while before checking again
            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }
    }
}