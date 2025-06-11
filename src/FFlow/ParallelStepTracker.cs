using System.Collections.Concurrent;

namespace FFlow;

internal class ParallelStepTracker
{
    private static readonly Lazy<ParallelStepTracker> _instance = 
        new(() => new ParallelStepTracker());

    public static ParallelStepTracker Instance => _instance.Value;

    private readonly ConcurrentDictionary<Guid, ConcurrentBag<Task>> _parallelTasks = new();

    public void Initialize(Guid parentId)
    {
        if (parentId == Guid.Empty)
            throw new ArgumentException("Parent ID cannot be empty.", nameof(parentId));

        _parallelTasks.TryAdd(parentId, new ConcurrentBag<Task>());
    }

    public void AddTask(Guid parentId, Task task)
    {
        ArgumentNullException.ThrowIfNull(task);

        if (!_parallelTasks.TryGetValue(parentId, out var bag))
        {
            bag = new ConcurrentBag<Task>();
            _parallelTasks.TryAdd(parentId, bag);
        }

        bag.Add(task);
    }
    
    public async Task WaitForAllTasksAsync(Guid parentId, CancellationToken cancellationToken = default)
    {
        if (_parallelTasks.TryRemove(parentId, out var tasks))
        {
            var allTasks = Task.WhenAll(tasks);
            try
            {
                using (cancellationToken.Register(() => throw new OperationCanceledException(cancellationToken)))
                {
                    await allTasks;
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                if (allTasks.Exception != null)
                    throw new AggregateException($"Error while awaiting tasks for {parentId}", allTasks.Exception.InnerExceptions);
                throw;
            }

        }
    }

    
}