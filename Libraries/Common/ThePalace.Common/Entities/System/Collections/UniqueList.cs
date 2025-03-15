namespace System.Collections;

public class UniqueList<T> : Disposable, IDisposable, IList<T>
{
    private UniqueList()
    {
        _list = new();
    }

    public UniqueList(int maxCapacity)
    {
        _maxCapacity = maxCapacity;
        _list = new(maxCapacity);
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
        try
        {
            _list.Remove(item);
        }
        catch
        {
        }

        if (_list.Count >= _maxCapacity)
            try
            {
                _list.RemoveAt(_list.Count - 1);
            }
            catch
            {
            }

        try
        {
            _list.Insert(0, item);
        }
        catch
        {
        }
    }

    public bool TryRemove(T item)
    {
        try
        {
            _list.Remove(item);

            return true;
        }
        catch
        {
        }

        return false;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    public void Clear()
    {
        _list.Clear();
    }

    public bool Contains(T item)
    {
        return _list.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        _list.CopyTo(array, arrayIndex);
    }

    public bool Remove(T item)
    {
        return _list.Remove(item);
    }

    public int IndexOf(T item)
    {
        return _list.IndexOf(item);
    }

    public void Insert(int index, T item)
    {
        try
        {
            _list.Remove(item);
        }
        catch
        {
        }

        if (_list.Count >= _maxCapacity)
            try
            {
                _list.RemoveAt(_list.Count - 1);
            }
            catch
            {
            }

        try
        {
            _list.Insert(index, item);
        }
        catch
        {
        }
    }

    public void RemoveAt(int index)
    {
        _list.RemoveAt(index);
    }

    public T this[int index]
    {
        get => _list[index];
        set => _list[index] = value;
    }
}