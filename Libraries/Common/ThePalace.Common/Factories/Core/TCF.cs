namespace ThePalace.Common.Factories.Core;

public sealed class TCF : IDisposable
{
    #region cStr

    private TCF()
    {
        _tryBlocks = [];
    }

    public TCF(bool breakOnException = false) : this()
    {
        _breakOnException = breakOnException;
    }

    public TCF(bool breakOnException = false, params Action[] tryBlocks) : this(breakOnException)
    {
        Try(tryBlocks);
    }

    public TCF(params Action[] tryBlocks) : this()
    {
        Try(tryBlocks);
    }

    public TCF(IEnumerable<Action> tryBlocks, bool breakOnException = false) : this(breakOnException)
    {
        Try(tryBlocks);
    }

    public TCF(params Func<object>[] tryBlocks) : this()
    {
        Try(tryBlocks);
    }

    public TCF(bool breakOnException = false, params Func<object>[] tryBlocks) : this(breakOnException)
    {
        Try(tryBlocks);
    }

    public TCF(IEnumerable<Func<object>> tryBlocks, bool breakOnException = false) : this(breakOnException)
    {
        Try(tryBlocks);
    }

    public TCF(bool breakOnException = false, params IDisposable[] tryObjects) : this(breakOnException)
    {
        TryDispose(tryObjects);
    }

    public TCF(IEnumerable<IDisposable> tryBlocks, bool breakOnException = false) : this(breakOnException)
    {
        TryDispose(tryBlocks);
    }

    ~TCF()
    {
        Dispose();
    }

    public void Dispose()
    {
        _tryBlocks?.Clear();
        _tryBlocks = null;
        _catchBlock = null;
        _finallyBlock = null;

        GC.SuppressFinalize(this);
    }

    #endregion

    #region Properties

    private readonly bool _breakOnException;
    private List<object> _tryBlocks;
    private Action<IReadOnlyList<Exception>> _catchBlock;
    private Action<TCFResults> _finallyBlock;

    #endregion

    #region Static Methods

    public static TCF Options(bool breakOnException = false)
    {
        return new TCF(breakOnException);
    }

    public static TCF _Try(params Action[] tryBlocks)
    {
        return new TCF(tryBlocks);
    }

    public static TCF _Try(IEnumerable<Action> tryBlocks, bool breakOnException = false)
    {
        return new TCF(tryBlocks, breakOnException);
    }

    public static TCF _Try(params Func<object>[] tryBlocks)
    {
        return new TCF(tryBlocks);
    }

    public static TCF _Try(IEnumerable<Func<object>> tryBlocks, bool breakOnException = false)
    {
        return new TCF(tryBlocks, breakOnException);
    }

    public static TCF _TryDispose(params IDisposable[] tryObjects)
    {
        return new TCF(tryObjects);
    }

    public static TCF _TryDispose(params IEnumerable<IDisposable> tryObjects)
    {
        return new TCF(tryObjects);
    }

    #endregion

    #region Methods

    public TCF Try(params Action[] tryBlocks)
    {
        if (tryBlocks != null)
            _tryBlocks.AddRange(tryBlocks.OfType<Action>());

        return this;
    }

    public TCF Try(IEnumerable<Action> tryBlocks)
    {
        if (tryBlocks != null)
            _tryBlocks.AddRange(tryBlocks.OfType<Action>());

        return this;
    }

    public TCF Try(params Func<object>[] tryBlocks)
    {
        if (tryBlocks != null)
            _tryBlocks.AddRange(tryBlocks.OfType<Func<object>>());

        return this;
    }

    public TCF Try(IEnumerable<Func<object>> tryBlocks)
    {
        if (tryBlocks != null)
            _tryBlocks.AddRange(tryBlocks.OfType<Func<object>>());

        return this;
    }

    public TCF TryDispose(params IDisposable[] tryObjects)
    {
        if (tryObjects != null)
            _tryBlocks.AddRange(tryObjects.OfType<IDisposable>());

        return this;
    }

    public TCF TryDispose(IEnumerable<IDisposable> tryObjects)
    {
        if (tryObjects != null)
            _tryBlocks.AddRange(tryObjects.OfType<IDisposable>());

        return this;
    }

    public TCF Catch(Action<IReadOnlyList<Exception>> catchBlock)
    {
        _catchBlock ??= catchBlock;

        return this;
    }

    public TCF Finally(Action finallyBlock)
    {
        _finallyBlock ??= results => finallyBlock();

        return this;
    }

    public TCF Finally(Action<TCFResults> finallyBlock)
    {
        _finallyBlock ??= finallyBlock;

        return this;
    }

    public TCFResults Execute()
    {
        if ((_tryBlocks?.Count ?? 0) < 1) return null;

        var results = new TCFResults();

        try
        {
            foreach (var tryBlock in _tryBlocks)
                try
                {
                    switch (tryBlock)
                    {
                        case Action action:
                            action();

                            break;
                        case Func<object> @func:
                            results._Results.Add(@func());

                            break;
                    }
                }
                catch (Exception ex)
                {
                    if (_catchBlock != null)
                        results._Exceptions.Add(ex);

                    if (_breakOnException)
                        break;
                }

            if (_catchBlock != null)
                try
                {
                    _catchBlock(results.Exceptions);
                }
                catch (Exception ex)
                {
                    results._Exceptions.Add(ex);
                }
        }
        catch (Exception ex)
        {
            if (_catchBlock != null)
                results._Exceptions.Add(ex);
        }
        finally
        {
            if (_finallyBlock != null)
                try
                {
                    _finallyBlock(results);
                }
                catch (Exception ex)
                {
                    if (_catchBlock != null)
                        results._Exceptions.Add(ex);
                }
        }

        return results;
    }

    #endregion
}

public sealed class TCFResults
{
    ~TCFResults()
    {
        _Exceptions?.Clear();
        _Exceptions = null;
        _Results?.Clear();
        _Results = null;
    }

    internal List<Exception> _Exceptions = [];
    internal List<object> _Results = [];

    public IReadOnlyList<Exception> Exceptions => _Exceptions.AsReadOnly();

    public IReadOnlyList<object> Results => _Results.AsReadOnly();
}