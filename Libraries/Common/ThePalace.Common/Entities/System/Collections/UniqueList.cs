namespace System.Collections;

public class UniqueList<T> : Disposable, IDisposable, IList<T>
{
    private UniqueList() : this(50)
    {
    }

    public UniqueList(int maxCapacity)
    {
        _maxCapacity = maxCapacity;
        _list = new(_maxCapacity);
    }

    public UniqueList(IEnumerable<T> items) : this(50, items?.ToArray())
    {
    }

    public UniqueList(params T[] items) : this(50, items)
    {
    }

    public UniqueList(int maxCapacity, IEnumerable<T> items) : this(maxCapacity, items?.ToArray())
    {
    }

    public UniqueList(int maxCapacity, params T[] items) : this(maxCapacity)
    {
        if ((items?.Length ?? 0) > 0)
            items?.ToList()?.ForEach(i => Add(i));
    }

    ~UniqueList()
    {
        Dispose();
    }

    public void Dispose()
    {
        if (IsDisposed) return;

        _list?.Clear();
        _list = null;

        base.Dispose();

        GC.SuppressFinalize(this);
    }

    private List<T> _list;
    private int _maxCapacity;

    public int Count => _list.Count;
    public bool IsReadOnly => false;

    public void Add(T item)
    {
        Insert(0, item);
    }

    public bool Remove(T item)
    {
        if (item == null ||
            IsDisposed) return false;

        try
        {
            return _list?.Remove(item) ?? false;
        }
        catch
        {
        }

        return false;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return IsDisposed ? null : _list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return IsDisposed ? null : _list.GetEnumerator();
    }

    public void Clear()
    {
        if (IsDisposed) return;

        _list?.Clear();
    }

    public bool Contains(T item)
    {
        return item == null ||
               IsDisposed
            ? false
            : _list.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        if (array == null ||
            IsDisposed) return;

        _list?.CopyTo(array, arrayIndex);
    }

    public int IndexOf(T item)
    {
        if (item == null ||
            IsDisposed) return 0;

        return _list.IndexOf(item);
    }

    public void Insert(int index, T item)
    {
        if (item == null ||
            IsDisposed) return;

        try
        {
            _list?.Remove(item);
        }
        catch
        {
        }

        var count = _list.Count;

        while (count >= _maxCapacity)
            try
            {
                _list?.RemoveAt(count - 1);

                count = _list.Count;
            }
            catch
            {
            }

        if (index >= count)
        {
            _list.Add(item);

            return;
        }

        if (index < 0) index = count + index;

        try
        {
            _list?.Insert(index % count, item);
        }
        catch
        {
        }
    }

    public void RemoveAt(int index)
    {
        if (IsDisposed) return;

        _list?.RemoveAt(index);
    }

    public T[] ToArray() => (IsDisposed ? null : _list)?.ToArray() ?? [];

    public T this[int index]
    {
        get => _list[index];
        set => _list[index] = value;
    }
}