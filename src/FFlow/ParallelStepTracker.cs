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

            if (cancellationToken.CanBeCanceled)
            {
                var cancellationTask = Task.Delay(Timeout.Infinite, cancellationToken);
                var completedTask = await Task.WhenAny(allTasks, cancellationTask).ConfigureAwait(false);
                if (completedTask == cancellationTask)
                {
                    throw new OperationCanceledException(cancellationToken);
                }

                await allTasks.ConfigureAwait(false);
            }
            else
            {
                await allTasks.ConfigureAwait(false);
            }
        }
    }


    
}