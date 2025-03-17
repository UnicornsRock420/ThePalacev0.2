namespace System.Collections;

public class UniqueList<T> : Disposable, IDisposable, IList<T>
{
    private const int CONST_INT_DefaultCapacity = 50;

    private UniqueList() : this(CONST_INT_DefaultCapacity)
    {
    }

    public UniqueList(int maxCapacity)
    {
        _maxCapacity = maxCapacity;
        _list = new(_maxCapacity);
    }

    public UniqueList(IEnumerable<T> items) : this(CONST_INT_DefaultCapacity, items?.ToArray())
    {
    }

    public UniqueList(params T[] items) : this(CONST_INT_DefaultCapacity, items)
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

    public int Count => _list?.Count ?? 0;
    public bool IsReadOnly => false;

    public void Add(T item)
    {
        if (IsDisposed) return;

        ArgumentNullException.ThrowIfNull(item, nameof(item));

        Insert(0, item);
    }

    public bool Remove(T item)
    {
        if (IsDisposed) return false;

        ArgumentNullException.ThrowIfNull(item, nameof(item));

        try
        {
            return _list?.Remove(item) ?? false;
        }
        catch
        {
        }

        return false;
    }

    public IEnumerator<T>? GetEnumerator()
    {
        return IsDisposed ? null : _list?.GetEnumerator();
    }

    IEnumerator? IEnumerable.GetEnumerator()
    {
        return IsDisposed ? null : _list?.GetEnumerator();
    }

    public void Clear()
    {
        if (IsDisposed) return;

        _list?.Clear();
    }

    public bool Contains(T? item)
    {
        if (IsDisposed) return false;

        ArgumentNullException.ThrowIfNull(item, nameof(item));

        return _list?.Contains(item) ?? false;
    }

    public void CopyTo(T[]? array, int arrayIndex)
    {
        if (IsDisposed) return;

        ArgumentNullException.ThrowIfNull(array, nameof(array));

        _list?.CopyTo(array, arrayIndex);
    }

    public int IndexOf(T? item)
    {
        if (IsDisposed) return 0;

        ArgumentNullException.ThrowIfNull(item, nameof(item));

        return _list?.IndexOf(item) ?? -1;
    }

    public void Insert(int index, T? item)
    {
        if (IsDisposed) return;

        ArgumentNullException.ThrowIfNull(item, nameof(item));

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
            _list?.Add(item);

            return;
        }

        index %= count;

        if (index < 0) index += count;

        try
        {
            _list?.Insert(index, item);
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

    public T? this[int index]
    {
        get
        {
            if (index < 0 ||
                index >= (_list?.Count ?? 0) - 1) throw new IndexOutOfRangeException(nameof(index));

            return _list[index];
        }
        set
        {
            if (index < 0 ||
                index >= (_list?.Count ?? 0) - 1) throw new IndexOutOfRangeException(nameof(index));

            _list[index] = value;
        }
    }
}