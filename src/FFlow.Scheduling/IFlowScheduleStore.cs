namespace FFlow.Scheduling;

public interface IFlowScheduleStore
{
    Task AddAsync(ScheduledWorkflow workflow, CancellationToken cancellationToken = default);
    Task<IEnumerable<ScheduledWorkflow>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ScheduledWorkflow>> GetDueAsync(DateTimeOffset now, CancellationToken cancellationToken = default);
    Task RemoveAsync(ScheduledWorkflow workflow, CancellationToken cancellationToken = default);
    Task<ScheduledWorkflow?> GetNextAsync(CancellationToken cancellationToken = default);
}