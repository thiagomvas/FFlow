using System.Text.Json;

namespace FFlow.Scheduling;

public class FileFlowScheduleStore : IFlowScheduleStore
{
    private readonly string _filePath;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private List<ScheduledWorkflow> _scheduledWorkflows = new();

    public FileFlowScheduleStore(string filePath)
    {
        _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        LoadFromFileAsync().RunSynchronously(); // sync load on construction
    }

    private async Task LoadFromFileAsync()
    {
        if (!File.Exists(_filePath))
        {
            _scheduledWorkflows = [];
            return;
        }

        await _semaphore.WaitAsync();
        try
        {
            var json = await File.ReadAllTextAsync(_filePath);
            _scheduledWorkflows = JsonSerializer.Deserialize<List<ScheduledWorkflow>>(json) ??
                                  [];
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task SaveToFileAsync()
    {
        await _semaphore.WaitAsync();
        try
        {
            var json = JsonSerializer.Serialize(_scheduledWorkflows,
                new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_filePath, json);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task AddAsync(ScheduledWorkflow workflow, CancellationToken cancellationToken = default)
    {
        if (workflow == null) throw new ArgumentNullException(nameof(workflow));

        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            _scheduledWorkflows.Add(workflow);
            await SaveToFileAsync();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<IEnumerable<ScheduledWorkflow>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            return _scheduledWorkflows.ToList();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<IEnumerable<ScheduledWorkflow>> GetDueAsync(DateTimeOffset now,
        CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            return _scheduledWorkflows.Where(w => w.ExecuteAt <= now).ToList();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task RemoveAsync(ScheduledWorkflow workflow, CancellationToken cancellationToken = default)
    {
        if (workflow == null) throw new ArgumentNullException(nameof(workflow));

        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            _scheduledWorkflows.Remove(workflow);
            await SaveToFileAsync();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<ScheduledWorkflow?> GetNextAsync(CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);
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