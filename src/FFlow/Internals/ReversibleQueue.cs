namespace FFlow;

internal class ReversibleQueue<T>
{
    private readonly List<T> _items;
    private int _head = 0;

    public ReversibleQueue(IEnumerable<T> items)
    {
        _items = new List<T>(items);
    }

    public bool TryDequeue(out T item)
    {
        if (_head >= _items.Count)
        {
            item = default!;
            return false;
        }

        item = _items[_head++];
        return true;
    }

    public bool TryBacktrack(out T item)
    {
        if (_head <= 0)
        {
            item = default!;
            return false;
        }

        item = _items[--_head];
        return true;
    }

    public void Reset() => _head = 0;

    public void Add(T item) => _items.Add(item);

    public IReadOnlyList<T> Items => _items;
    public int Count => _items.Count;
    public int Head => _head;
}
