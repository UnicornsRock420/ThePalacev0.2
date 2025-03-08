namespace ThePalace.Common.Factories;

public sealed class TCF : IDisposable
{
    private readonly bool BreakOnException;
    private Action<IReadOnlyList<Exception>> CatchBlock;
    private Action<TCFResults> FinallyBlock;
    private TCFResults Results;

    private List<Action> TryBlocks;

    //public static class Types
    //{
    //    public const string ClassName = nameof(TCF);
    //    public static readonly Type TCF = typeof(TCF);
    //}

    private TCF()
    {
        TryBlocks = [];
        Results = new TCFResults();
    }

    public TCF(bool breakOnException = true)
        : this()
    {
        BreakOnException = breakOnException;
    }

    public TCF(params Action[] tryBlocks)
        : this()
    {
        Try(tryBlocks);
    }

    public TCF(IEnumerable<Action> tryBlocks)
        : this()
    {
        Try(tryBlocks);
    }

    public TCF(params Func<object>[] tryBlocks)
        : this()
    {
        Try(tryBlocks);
    }

    public TCF(IEnumerable<Func<object>> tryBlocks)
        : this()
    {
        Try(tryBlocks);
    }

    public TCF(params IDisposable[] tryObjects)
        : this()
    {
        TryDispose(tryObjects);
    }

    public void Dispose()
    {
        TryBlocks?.Clear();
        TryBlocks = null;
        CatchBlock = null;
        FinallyBlock = null;
        Results = null;
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

    public static TCF _Try(IEnumerable<Action> tryBlocks)
    {
        return new TCF(tryBlocks);
    }

    public static TCF _Try(params Func<object>[] tryBlocks)
    {
        return new TCF(tryBlocks);
    }

    public static TCF _Try(IEnumerable<Func<object>> tryBlocks)
    {
        return new TCF(tryBlocks);
    }

    public static TCF _TryDispose(params IDisposable[] tryObjects)
    {
        return new TCF(tryObjects);
    }

    public TCF Try(params Action[] tryBlocks)
    {
        if (tryBlocks != null)
            TryBlocks.AddRange(tryBlocks
                .Where(b => b != null));

        return this;
    }

    public TCF Try(IEnumerable<Action> tryBlocks)
    {
        if (tryBlocks != null)
            TryBlocks.AddRange(tryBlocks
                .Where(b => b != null));

        return this;
    }

    public TCF Try(params Func<object>[] tryBlocks)
    {
        if (tryBlocks != null)
            TryBlocks.AddRange(tryBlocks
                .Where(b => b != null)
                .Select(b => (Action)(() => Results.IResults.Add(b))));

        return this;
    }

    public TCF Try(IEnumerable<Func<object>> tryBlocks)
    {
        if (tryBlocks != null)
            TryBlocks.AddRange(tryBlocks
                .Where(b => b != null)
                .Select(b => (Action)(() => Results.IResults.Add(b))));

        return this;
    }

    public TCF TryDispose(params IDisposable[] tryObjects)
    {
        if (tryObjects != null)
            TryBlocks.AddRange(tryObjects
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
            if (TryBlocks != null && TryBlocks.Count > 0)
                foreach (var tryBlock in TryBlocks)
                    if (tryBlock != null)
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
    internal List<Exception> IExceptions = [];
    internal List<object> IResults = [];

    public IReadOnlyList<Exception> Exceptions =>
        IExceptions.AsReadOnly();

    public IReadOnlyList<object> Results =>
        IResults.AsReadOnly();
}