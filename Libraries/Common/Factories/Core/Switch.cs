using System.Collections.Concurrent;

namespace Lib.Common.Factories.Core;

public sealed class Switch : IDisposable
{
    public delegate bool ScCondition(object? value = default);

    public delegate object? ScBlock(object? value = default);

    #region ctors

    private Switch()
    {
        _caseBlocks = new();
    }

    private Switch(object value) : this()
    {
        _value = value;
    }

    private Switch(object value, object? defaultValue = default) : this()
    {
        _value = value;
        _defaultValue = defaultValue;
    }

    public Switch(object value, bool breakOnFirstTrueCondition = true) : this(value)
    {
        _breakOnFirstTrueCondition = breakOnFirstTrueCondition;
    }

    public Switch(object value, object? defaultValue = default, bool breakOnFirstTrueCondition = true) : this(value, defaultValue)
    {
        _breakOnFirstTrueCondition = breakOnFirstTrueCondition;
    }

    public Switch(object value, IEnumerable<KeyValuePair<ScCondition?, ScBlock>> caseBlocks, bool breakOnFirstTrueCondition = true) : this(value, breakOnFirstTrueCondition)
    {
        foreach (var caseBlock in caseBlocks)
            _caseBlocks.TryAdd(caseBlock.Key, caseBlock.Value);
    }

    public Switch(object value, IEnumerable<KeyValuePair<ScCondition?, ScBlock>> caseBlocks, object? defaultValue = default, bool breakOnFirstTrueCondition = true) : this(value,
        defaultValue, breakOnFirstTrueCondition)
    {
        foreach (var caseBlock in caseBlocks)
            _caseBlocks.TryAdd(caseBlock.Key, caseBlock.Value);
    }

    public Switch(object value, params KeyValuePair<ScCondition?, ScBlock>[] caseBlocks) : this(value)
    {
        foreach (var caseBlock in caseBlocks)
            _caseBlocks.TryAdd(caseBlock.Key, caseBlock.Value);
    }

    public Switch(object value, object? defaultValue = default, params KeyValuePair<ScCondition?, ScBlock>[] caseBlocks) : this(value, defaultValue)
    {
        foreach (var caseBlock in caseBlocks)
            _caseBlocks.TryAdd(caseBlock.Key, caseBlock.Value);
    }

    public Switch(object value, bool breakOnFirstTrueCondition = true, params KeyValuePair<ScCondition?, ScBlock>[] caseBlocks) : this(value, breakOnFirstTrueCondition)
    {
        foreach (var caseBlock in caseBlocks)
            _caseBlocks.TryAdd(caseBlock.Key, caseBlock.Value);
    }

    public Switch(object value, object? defaultValue = default, bool breakOnFirstTrueCondition = true, params KeyValuePair<ScCondition?, ScBlock>[] caseBlocks) : this(value,
        defaultValue, breakOnFirstTrueCondition)
    {
        foreach (var caseBlock in caseBlocks)
            _caseBlocks.TryAdd(caseBlock.Key, caseBlock.Value);
    }

    ~Switch()
    {
        Dispose();
    }

    public void Dispose()
    {
        _caseBlocks?.Clear();
        _caseBlocks = null;

        GC.SuppressFinalize(this);
    }

    #endregion

    #region Properties

    private readonly bool _breakOnFirstTrueCondition;
    private ConcurrentDictionary<ScCondition?, object> _caseBlocks;
    private readonly object? _defaultValue;
    private readonly object? _value;

    #endregion

    #region Static Methods

    public static Switch Options(object value, bool breakOnFirstTrueCondition = true)
    {
        return new Switch(value, breakOnFirstTrueCondition);
    }

    public static Switch Options(object value, object? defaultValue = default, bool breakOnFirstTrueCondition = true)
    {
        return new Switch(value, defaultValue, breakOnFirstTrueCondition);
    }

    public static Switch _Case(object value, ScCondition? condition, ScBlock block, bool breakOnFirstTrueCondition = true)
    {
        return new Switch(value, [new KeyValuePair<ScCondition?, ScBlock>(condition, block)], breakOnFirstTrueCondition);
    }

    public static Switch _Case(object value, ScCondition? condition, ScBlock block, object? defaultValue = default, bool breakOnFirstTrueCondition = true)
    {
        return new Switch(value, [new KeyValuePair<ScCondition?, ScBlock>(condition, block)], defaultValue, breakOnFirstTrueCondition);
    }

    public static Switch _Case(object value, IEnumerable<KeyValuePair<ScCondition?, ScBlock>> caseBlocks, bool breakOnFirstTrueCondition = true)
    {
        return new Switch(value, caseBlocks, breakOnFirstTrueCondition);
    }

    public static Switch _Case(object value, IEnumerable<KeyValuePair<ScCondition?, ScBlock>> caseBlocks, object? defaultValue = default, bool breakOnFirstTrueCondition = true)
    {
        return new Switch(value, caseBlocks, defaultValue, breakOnFirstTrueCondition);
    }

    public static Switch _Case(object value, bool breakOnFirstTrueCondition = true, params KeyValuePair<ScCondition?, ScBlock>[] caseBlocks)
    {
        return new Switch(value, breakOnFirstTrueCondition, caseBlocks);
    }

    public static Switch _Case(object value, object? defaultValue = default, bool breakOnFirstTrueCondition = true, params KeyValuePair<ScCondition?, ScBlock>[] caseBlocks)
    {
        return new Switch(value, defaultValue, breakOnFirstTrueCondition, caseBlocks);
    }

    #endregion

    #region Methods

    public Switch Case(ScCondition? condition, ScBlock block)
    {
        _caseBlocks.TryAdd(condition, block);

        return this;
    }

    public Switch Case(IEnumerable<KeyValuePair<ScCondition?, ScBlock>> caseBlocks)
    {
        foreach (var caseBlock in caseBlocks)
            _caseBlocks.TryAdd(caseBlock.Key, caseBlock.Value);

        return this;
    }

    public Switch Case(params KeyValuePair<ScCondition?, ScBlock>[] caseBlocks)
    {
        foreach (var caseBlock in caseBlocks)
            _caseBlocks.TryAdd(caseBlock.Key, caseBlock.Value);

        return this;
    }

    public SwitchResults Execute()
    {
        if ((_caseBlocks?.Count ?? 0) < 1) return null;


        var results = new SwitchResults();
        var result = (object?)null;
        var match = false;

        foreach (var @case in _caseBlocks)
        {
            try
            {
                if (!(@case.Key switch
                    {
                        ScCondition func => func(_value),
                        _ => false,
                    })) continue;

                match = true;

                result = @case.Value switch
                {
                    ScBlock func => func(_value),
                    _ => @case.Value,
                };
                if (result != null)
                    results._Results.Add(result);
            }
            catch (Exception ex)
            {
                results._Exceptions.Add(ex);

                continue;
            }

            if (match &&
                _breakOnFirstTrueCondition)
                break;
        }

        if (match ||
            !_caseBlocks.TryGetValue(null, out var block)) return results;

        result = block switch
        {
            ScBlock func => func(_value),
            _ => block,
        } ?? _defaultValue;
        if (result != null)
            results._Results.Add(result);


        return results;
    }

    #endregion
}

public sealed class Switch<T> : IDisposable
{
    public delegate bool ScCondition<T>(T? value = default(T));

    public delegate T ScBlock<T>(T? value = default(T));

    #region ctors

    private Switch()
    {
        _caseBlocks = new();
    }

    public Switch(T? value) : this()
    {
        _value = value;
        _defaultValue = default(T);
    }

    public Switch(T? value, T? defaultValue = default(T)) : this()
    {
        _value = value;
        _defaultValue = defaultValue;
    }

    public Switch(T? value, bool breakOnFirstTrueCondition = true) : this(value)
    {
        _breakOnFirstTrueCondition = breakOnFirstTrueCondition;
    }

    public Switch(T? value, T? defaultValue = default(T), bool breakOnFirstTrueCondition = true) : this(value, defaultValue)
    {
        _breakOnFirstTrueCondition = breakOnFirstTrueCondition;
    }

    public Switch(T? value, IEnumerable<KeyValuePair<ScCondition<T>?, ScBlock<T>>> caseBlocks, bool breakOnFirstTrueCondition = true) : this(value, breakOnFirstTrueCondition)
    {
        foreach (var caseBlock in caseBlocks)
            _caseBlocks.TryAdd(caseBlock.Key, caseBlock.Value);
    }

    public Switch(T? value, IEnumerable<KeyValuePair<ScCondition<T>?, ScBlock<T>>> caseBlocks, T? defaultValue = default(T), bool breakOnFirstTrueCondition = true) : this(value,
        defaultValue, breakOnFirstTrueCondition)
    {
        foreach (var caseBlock in caseBlocks)
            _caseBlocks.TryAdd(caseBlock.Key, caseBlock.Value);
    }

    public Switch(T? value, params KeyValuePair<ScCondition<T>?, ScBlock<T>>[] caseBlocks) : this(value)
    {
        foreach (var caseBlock in caseBlocks)
            _caseBlocks.TryAdd(caseBlock.Key, caseBlock.Value);
    }

    public Switch(T? value, T? defaultValue = default(T), params KeyValuePair<ScCondition<T>?, ScBlock<T>>[] caseBlocks) : this(value, defaultValue)
    {
        foreach (var caseBlock in caseBlocks)
            _caseBlocks.TryAdd(caseBlock.Key, caseBlock.Value);
    }

    public Switch(T? value, bool breakOnFirstTrueCondition = true, params KeyValuePair<ScCondition<T>?, ScBlock<T>>[] caseBlocks) : this(value, breakOnFirstTrueCondition)
    {
        foreach (var caseBlock in caseBlocks)
            _caseBlocks.TryAdd(caseBlock.Key, caseBlock.Value);
    }

    public Switch(T? value, T? defaultValue = default(T), bool breakOnFirstTrueCondition = true, params KeyValuePair<ScCondition<T>?, ScBlock<T>>[] caseBlocks) : this(value,
        defaultValue, breakOnFirstTrueCondition)
    {
        foreach (var caseBlock in caseBlocks)
            _caseBlocks.TryAdd(caseBlock.Key, caseBlock.Value);
    }

    ~Switch()
    {
        Dispose();
    }

    public void Dispose()
    {
        _caseBlocks?.Clear();
        _caseBlocks = null;

        GC.SuppressFinalize(this);
    }

    #endregion

    #region Properties

    private readonly bool _breakOnFirstTrueCondition;
    private ConcurrentDictionary<ScCondition<T>?, object> _caseBlocks;
    private readonly T? _defaultValue;
    private readonly T? _value;

    #endregion

    #region Static Methods

    public static Switch<T> Options(T? value, bool breakOnFirstTrueCondition = true)
    {
        return new Switch<T>(value, breakOnFirstTrueCondition);
    }

    public static Switch<T> _Case(T? value, ScCondition<T>? condition, ScBlock<T> block, bool breakOnFirstTrueCondition = true)
    {
        return new Switch<T>(value, [new KeyValuePair<ScCondition<T>?, ScBlock<T>>(condition, block)], breakOnFirstTrueCondition);
    }

    public static Switch<T> _Case(T? value, IEnumerable<KeyValuePair<ScCondition<T>?, ScBlock<T>>> caseBlocks, bool breakOnFirstTrueCondition = true)
    {
        return new Switch<T>(value, caseBlocks, breakOnFirstTrueCondition);
    }

    public static Switch<T> _Case(T? value, bool breakOnFirstTrueCondition = true, params KeyValuePair<ScCondition<T>?, ScBlock<T>>[] caseBlocks)
    {
        return new Switch<T>(value, breakOnFirstTrueCondition, caseBlocks);
    }

    #endregion

    #region Methods

    public Switch<T> Case(ScCondition<T>? condition, ScBlock<T> block)
    {
        _caseBlocks.TryAdd(condition, block);

        return this;
    }

    public Switch<T> Case(IEnumerable<KeyValuePair<ScCondition<T>?, ScBlock<T>>> caseBlocks)
    {
        foreach (var caseBlock in caseBlocks)
            _caseBlocks.TryAdd(caseBlock.Key, caseBlock.Value);

        return this;
    }

    public Switch<T> Case(params KeyValuePair<ScCondition<T>?, ScBlock<T>>[] caseBlocks)
    {
        foreach (var caseBlock in caseBlocks)
            _caseBlocks.TryAdd(caseBlock.Key, caseBlock.Value);

        return this;
    }

    public SwitchResults Execute()
    {
        if ((_caseBlocks?.Count ?? 0) < 1) return null;

        var results = new SwitchResults();
        var result = (object?)null;
        var match = false;

        foreach (var @case in _caseBlocks)
        {
            try
            {
                if (!(@case.Key switch
                    {
                        ScCondition<T> func => func(_value),
                        _ => false,
                    })) continue;

                match = true;

                result = @case.Value switch
                {
                    ScBlock<T> func => func(_value),
                    _ => @case.Value,
                };
                if (result != null)
                    results._Results.Add(result);
            }
            catch (Exception ex)
            {
                results._Exceptions.Add(ex);

                continue;
            }

            if (match &&
                _breakOnFirstTrueCondition)
                break;
        }

        if (match ||
            !_caseBlocks.TryGetValue(null, out var block)) return results;

        result = block switch
        {
            ScBlock<T> func => func(_value),
            _ => block,
        } ?? _defaultValue;

        if (result != null)
            results._Results.Add(result);

        return results;
    }

    #endregion
}

public sealed class SwitchResults
{
    ~SwitchResults()
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