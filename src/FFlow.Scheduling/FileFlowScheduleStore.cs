using System.Text.Json;

namespace FFlow.Scheduling;

/// <summary>
/// A file-based implementation of the <see cref="IFlowScheduleStore"/> interface.
/// Provides methods to manage scheduled workflows, storing them in a JSON file.
/// </summary>
public class FileFlowScheduleStore : IFlowScheduleStore
{
    private readonly string _filePath;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private List<ScheduledWorkflow> _scheduledWorkflows = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="FileFlowScheduleStore"/> class.
    /// </summary>
    /// <param name="filePath">The path to the file where workflows will be stored.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="filePath"/> is null.</exception>
    public FileFlowScheduleStore(string filePath)
    {
        _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        LoadFromFileAsync().RunSynchronously(); // sync load on construction
    }

    /// <summary>
    /// Loads scheduled workflows from the file asynchronously.
    /// </summary>
    private async Task LoadFromFileAsync()
    {
        if (!File.Exists(_filePath))
        {
            _scheduledWorkflows = [];
            return;
        }

        await _semaphore.WaitAsync().ConfigureAwait(false);
        try
        {
            var json = await File.ReadAllTextAsync(_filePath).ConfigureAwait(false);
            _scheduledWorkflows = JsonSerializer.Deserialize<List<ScheduledWorkflow>>(json) ??
                                  [];
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Saves the current scheduled workflows to the file asynchronously.
    /// </summary>
    private async Task SaveToFileAsync()
    {
        await _semaphore.WaitAsync().ConfigureAwait(false);
        try
        {
            var json = JsonSerializer.Serialize(_scheduledWorkflows,
                new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_filePath, json).ConfigureAwait(false);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Adds a new scheduled workflow to the store.
    /// </summary>
    /// <param name="workflow">The workflow to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="workflow"/> is null.</exception>
    public async Task AddAsync(ScheduledWorkflow workflow, CancellationToken cancellationToken = default)
    {
        if (workflow == null) throw new ArgumentNullException(nameof(workflow));

        await _semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            _scheduledWorkflows.Add(workflow);
            await SaveToFileAsync().ConfigureAwait(false);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Retrieves all scheduled workflows from the store.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A collection of all scheduled workflows.</returns>
    public async Task<IEnumerable<ScheduledWorkflow>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            return _scheduledWorkflows.ToList();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Retrieves workflows that are due for execution.
    /// </summary>
    /// <param name="now">The current time to check against.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A collection of workflows that are due for execution.</returns>
    public async Task<IEnumerable<ScheduledWorkflow>> GetDueAsync(DateTimeOffset now,
        CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            return _scheduledWorkflows.Where(w => w.ExecuteAt <= now).ToList();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Removes a scheduled workflow from the store.
    /// </summary>
    /// <param name="workflow">The workflow to remove.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="workflow"/> is null.</exception>
    public async Task RemoveAsync(ScheduledWorkflow workflow, CancellationToken cancellationToken = default)
    {
        if (workflow == null) throw new ArgumentNullException(nameof(workflow));

        await _semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            _scheduledWorkflows.Remove(workflow);
            await SaveToFileAsync().ConfigureAwait(false);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Retrieves the next scheduled workflow for execution.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The next scheduled workflow, or <c>null</c> if no workflows are scheduled.</returns>
    public async Task<ScheduledWorkflow?> GetNextAsync(CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            return _scheduledWorkflows.OrderBy(w => w.ExecuteAt).FirstOrDefault();
        }
        finally
        {
            _semaphore.Release();
        }
    }
}