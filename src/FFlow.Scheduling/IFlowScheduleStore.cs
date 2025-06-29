namespace FFlow.Scheduling;

/// <summary>
/// Represents a store for managing scheduled workflows.
/// </summary>
public interface IFlowScheduleStore
{
    /// <summary>
    /// Adds a new scheduled workflow to the store.
    /// </summary>
    /// <param name="workflow">The workflow to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddAsync(ScheduledWorkflow workflow, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all scheduled workflows from the store.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that returns a collection of all scheduled workflows.</returns>
    Task<IEnumerable<ScheduledWorkflow>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves workflows that are due for execution.
    /// </summary>
    /// <param name="now">The current time to check against.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that returns a collection of workflows that are due for execution.</returns>
    Task<IEnumerable<ScheduledWorkflow>> GetDueAsync(DateTimeOffset now, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a scheduled workflow from the store.
    /// </summary>
    /// <param name="workflow">The workflow to remove.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RemoveAsync(ScheduledWorkflow workflow, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the next scheduled workflow for execution.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that returns the next scheduled workflow, or <c>null</c> if no workflows are scheduled.</returns>
    Task<ScheduledWorkflow?> GetNextAsync(CancellationToken cancellationToken = default);
}