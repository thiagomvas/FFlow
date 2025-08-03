namespace FFlow.Scheduling;

/// <summary>
/// Represents an in-memory implementation of the <see cref="IFlowScheduleStore"/> interface.
/// Provides methods to manage scheduled workflows stored in memory.
/// </summary>
public class InMemoryFlowScheduleStore : IFlowScheduleStore
{
    private readonly List<ScheduledWorkflow> _scheduledWorkflows = new();
    
    public Task AddAsync(ScheduledWorkflow workflow, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(workflow);

        _scheduledWorkflows.Add(workflow);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<ScheduledWorkflow>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IEnumerable<ScheduledWorkflow>>(_scheduledWorkflows);
    }

    public Task<IEnumerable<ScheduledWorkflow>> GetDueAsync(DateTimeOffset now, CancellationToken cancellationToken = default)
    {
        var dueWorkflows = _scheduledWorkflows
            .Where(w => w.ExecuteAt <= now)
            .ToList();
        
        return Task.FromResult<IEnumerable<ScheduledWorkflow>>(dueWorkflows);
    }

    public Task RemoveAsync(ScheduledWorkflow workflow, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(workflow);

        _scheduledWorkflows.Remove(workflow);
        return Task.CompletedTask;
    }

    public Task<ScheduledWorkflow?> GetNextAsync(CancellationToken cancellationToken = default)
    {
        var nextWorkflow = _scheduledWorkflows
            .OrderBy(w => w.ExecuteAt)
            .FirstOrDefault();

        return Task.FromResult(nextWorkflow);
    }
}