namespace ThePalace.Common.Factories.Core;

public sealed class TCF : IDisposable
{
    private readonly bool BreakOnException;
    private Action<IReadOnlyList<Exception>> CatchBlock;
    private Action<TCFResults> FinallyBlock;

    private List<Action> TryBlocks;
    private TCFResults Results;

    private TCF()
    {
        TryBlocks = [];
        Results = new();
    }

    public TCF(bool breakOnException = true)
        : this()
    {
        BreakOnException = breakOnException;
    }

    public TCF(bool breakOnException = true, params Action[] tryBlocks)
        : this(breakOnException)
    {
        Try(tryBlocks);
    }

    public TCF(params Action[] tryBlocks)
        : this()
    {
        Try(tryBlocks);
    }

    public TCF(IEnumerable<Action> tryBlocks, bool breakOnException = true)
        : this(breakOnException)
    {
        Try(tryBlocks);
    }

    public TCF(params Func<object>[] tryBlocks)
        : this()
    {
        Try(tryBlocks);
    }

    public TCF(bool breakOnException = true, params Func<object>[] tryBlocks)
        : this(breakOnException)
    {
        Try(tryBlocks);
    }

    public TCF(IEnumerable<Func<object>> tryBlocks, bool breakOnException = true)
        : this(breakOnException)
    {
        Try(tryBlocks);
    }

    public TCF(bool breakOnException = true, params IDisposable[] tryObjects)
        : this(breakOnException)
    {
        TryDispose(tryObjects);
    }

    public TCF(IEnumerable<IDisposable> tryBlocks, bool breakOnException = true)
        : this(breakOnException)
    {
        TryDispose(tryBlocks);
    }

    public void Dispose()
    {
        TryBlocks?.Clear();
        TryBlocks = null;
        CatchBlock = null;
        FinallyBlock = null;
        Results = null;

        GC.SuppressFinalize(this);
    }

    ~TCF()
    {
        Dispose();
    }

    public static TCF Options(bool breakOnException = true)
    {
        return new TCF(breakOnException);
    }

    public static TCF _Try(params Action[] tryBlocks)
    {
        return new TCF(tryBlocks);
    }

    public static TCF _Try(IEnumerable<Action> tryBlocks, bool breakOnException = true)
    {
        return new TCF(tryBlocks, breakOnException);
    }

    public static TCF _Try(params Func<object>[] tryBlocks)
    {
        return new TCF(tryBlocks);
    }

    public static TCF _Try(IEnumerable<Func<object>> tryBlocks, bool breakOnException = true)
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

    public TCF Try(params Action[] tryBlocks)
    {
        if (tryBlocks != null)
            TryBlocks.AddRange(tryBlocks.OfType<Action>());

        return this;
    }

    public TCF Try(IEnumerable<Action> tryBlocks)
    {
        if (tryBlocks != null)
            TryBlocks.AddRange(tryBlocks.OfType<Action>());

        return this;
    }

    public TCF Try(params Func<object>[] tryBlocks)
    {
        if (tryBlocks != null)
            TryBlocks.AddRange(tryBlocks.OfType<Func<object>>()
                .Select(b => (Action)(() => Results.IResults.Add(b))));

        return this;
    }

    public TCF Try(IEnumerable<Func<object>> tryBlocks)
    {
        if (tryBlocks != null)
            TryBlocks.AddRange(tryBlocks.OfType<Func<object>>()
                .Select(b => (Action)(() => Results.IResults.Add(b))));

        return this;
    }

    public TCF TryDispose(params IDisposable[] tryObjects)
    {
        if (tryObjects != null)
            TryBlocks.AddRange(tryObjects.OfType<IDisposable>()
                .Select(o => (Action)(() => o?.Dispose())));

        return this;
    }

    public TCF TryDispose(IEnumerable<IDisposable> tryObjects)
    {
        if (tryObjects != null)
            TryBlocks.AddRange(tryObjects.OfType<IDisposable>()
                .Select(o => (Action)(() => o?.Dispose())));

        return this;
    }

    public TCF Catch(Action<IReadOnlyList<Exception>> catchBlock)
    {
        CatchBlock ??= catchBlock;

        return this;
    }

    public TCF Finally(Action finallyBlock)
    {
        FinallyBlock ??= results => finallyBlock();

        return this;
    }

    public TCF Finally(Action<TCFResults> finallyBlock)
    {
        FinallyBlock ??= finallyBlock;

        return this;
    }

    public TCFResults Execute()
    {
        try
        {
            if ((TryBlocks?.Count ?? 0) > 0)
                foreach (var tryBlock in TryBlocks.OfType<Action>())
                    try
                    {
                        tryBlock();
                    }
                    catch (Exception ex)
                    {
                        if (CatchBlock != null)
                            Results.IExceptions.Add(ex);

                        if (BreakOnException)
                            break;
                    }

            if (CatchBlock != null)
                try
                {
                    CatchBlock(Results.IExceptions.AsReadOnly());
                }
                catch (Exception ex)
                {
                    Results.IExceptions.Add(ex);
                }
        }
        catch (Exception ex)
        {
            if (CatchBlock != null)
                Results.IExceptions.Add(ex);
        }
        finally
        {
            if (FinallyBlock != null)
                try
                {
                    FinallyBlock(Results);
                }
                catch (Exception ex)
                {
                    if (CatchBlock != null)
                        Results.IExceptions.Add(ex);
                }
        }

        return Results;
    }
}

public sealed class TCFResults
{
    internal readonly List<Exception> IExceptions = [];
    internal readonly List<object> IResults = [];

    public IReadOnlyList<Exception> Exceptions => IExceptions.AsReadOnly();

    public IReadOnlyList<object> Results => IResults.AsReadOnly();
}