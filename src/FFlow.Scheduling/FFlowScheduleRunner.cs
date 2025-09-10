using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FFlow.Scheduling;

/// <summary>
/// A background service that manages the execution of scheduled workflows.
/// </summary>
public class FFlowScheduleRunner : BackgroundService
{
    private readonly IFlowScheduleStore _flowScheduleStore;
    private ILogger<FFlowScheduleRunner>? _logger;
    private readonly FFlowScheduleRunnerOptions _options;

    public FFlowScheduleRunner(IFlowScheduleStore flowScheduleStore, FFlowScheduleRunnerOptions options,
        ILogger<FFlowScheduleRunner>? logger)
    {
        _logger = logger;
        _flowScheduleStore = flowScheduleStore ?? throw new ArgumentNullException(nameof(flowScheduleStore));
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public FFlowScheduleRunner(IFlowScheduleStore flowScheduleStore, FFlowScheduleRunnerOptions options)
        : this(flowScheduleStore, options, null)
    {
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTimeOffset.UtcNow;
            
            try
            {
                var dueWorkflows = await _flowScheduleStore.GetDueAsync(now, stoppingToken).ConfigureAwait(false);

                foreach (var workflow in dueWorkflows)
                {
                    await workflow.Workflow.Build().RunAsync("", stoppingToken).ConfigureAwait(false);
                    if (_options.EnableLogging)
                        _logger?.LogInformation("Executed scheduled workflow: {WorkflowName} at {ExecutionTime}",
                            workflow.Workflow.GetType().Name, now);

                    if (workflow.Recurring)
                        workflow.UpdateNextExecution(now);
                    else
                        await _flowScheduleStore.RemoveAsync(workflow, stoppingToken).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                if (_options.EnableLogging)
                    _logger?.LogError(ex, "An error occurred while executing scheduled workflows.");
            }

            var timeUntilNext = await _flowScheduleStore.GetNextAsync(stoppingToken).ConfigureAwait(false);
            var delay = timeUntilNext?.ExecuteAt - now;
            
            TimeSpan delayToWait;
            if (delay.HasValue && delay.Value < _options.PollingInterval)
            {
                delayToWait = delay.Value;
                if (_options.EnableLogging)
                    _logger?.LogInformation("Next workflow execution in {Delay}", delayToWait);
            }
            else
            {
                delayToWait = _options.PollingInterval;
                if (_options.EnableLogging)
                    _logger?.LogInformation("Checking for due workflows in {Delay}", delayToWait);
            }
            
            
            await Task.Delay(delayToWait, stoppingToken).ConfigureAwait(false);
        }
    }
}