using System.Collections.Concurrent;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Core.Singletons;

public class EventBus : Singleton<EventBus>, IEventsBus
{
    private bool IsDisposed { get; set; }
    private static readonly Type CONST_TYPE_IEventHandler = typeof(IEventHandler);

    private readonly ConcurrentDictionary<string, List<IEventHandler>> _handlersDictionary = new();

    ~EventBus()
    {
        Dispose();
    }

    public void Dispose()
    {
        if (IsDisposed) return;

        IsDisposed = true;
        
        _handlersDictionary?.Clear();

        GC.SuppressFinalize(this);
    }

    public static Type? GetType(IEventParams @params)
    {
        return GetType(@params.GetType());
    }

    public static Type? GetType(Type type)
    {
        return AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(t => t.GetTypes())
            .Where(t =>
            {
                if (t.IsInterface) return false;

                var ntrs = t.GetInterfaces();

                return ntrs.Contains(CONST_TYPE_IEventHandler) &&
                       ntrs.Any(i => i.IsGenericType && i.GetGenericArguments().Contains(type));
            })
            .Select(t => t.GetInterfaces().Contains(CONST_TYPE_IEventHandler) ? t : null)
            .FirstOrDefault();
    }

    public void Subscribe(params IEventHandler[] handlers)
    {
        if (IsDisposed) return;

        handlers?.ToList()?.ForEach(h => Subscribe(h));
    }

    public void Subscribe(IEnumerable<IEventHandler> handlers)
    {
        if (IsDisposed) return;

        handlers?.ToList()?.ForEach(h => Subscribe(h));
    }

    public void Subscribe(IEventHandler handler)
    {
        if (IsDisposed) return;

        var eventType = handler.GetType();
        if (eventType == null) return;

        if (eventType.IsGenericType &&
            eventType.GetGenericArguments().Length > 0)
        {
            eventType = eventType.GetGenericArguments().FirstOrDefault();
            if (eventType == null) return;
        }

        var eventTypeName = eventType.FullName;
        if (string.IsNullOrWhiteSpace(eventTypeName)) return;

        if (!_handlersDictionary.TryAdd(eventTypeName, [handler])) _handlersDictionary[eventTypeName].Add(handler);
    }

    public void Subscribe<TEventParams>(params IEventHandler<TEventParams>[] handlers)
        where TEventParams : IEventParams
    {
        if (IsDisposed) return;

        handlers?.ToList()?.ForEach(h => Subscribe(h));
    }

    public void Subscribe<TEventParams>(IEnumerable<IEventHandler<TEventParams>> handlers)
        where TEventParams : IEventParams
    {
        if (IsDisposed) return;

        handlers?.ToList()?.ForEach(h => Subscribe(h));
    }

    public void Subscribe<TEventParams>(IEventHandler<TEventParams> handler)
        where TEventParams : IEventParams
    {
        if (IsDisposed) return;

        var eventType = typeof(TEventParams);
        if (eventType == null) return;

        var eventTypeName = eventType.FullName;
        if (string.IsNullOrWhiteSpace(eventTypeName)) return;

        if (!_handlersDictionary.TryAdd(eventTypeName, [handler])) _handlersDictionary[eventTypeName].Add(handler);
    }

    public void Subscribe<TEventParams, TEventHandler>()
        where TEventParams : IEventParams
        where TEventHandler : IEventHandler<TEventParams>, new()
    {
        if (IsDisposed) return;

        var eventType = typeof(TEventParams);
        if (eventType == null) return;

        var eventTypeName = eventType.FullName;
        if (string.IsNullOrWhiteSpace(eventTypeName)) return;

        var handler = new TEventHandler();
        if (handler == null) return;

        if (!_handlersDictionary.TryAdd(eventTypeName, [handler])) _handlersDictionary[eventTypeName].Add(handler);
    }

    public void Subscribe(params Type[] eventTypes)
    {
        if (IsDisposed) return;

        eventTypes?.ToList()?.ForEach(t => Subscribe(t));
    }

    public void Subscribe(IEnumerable<Type> eventTypes)
    {
        if (IsDisposed) return;

        eventTypes?.ToList()?.ForEach(t => Subscribe(t));
    }

    public void Subscribe(Type eventType)
    {
        if (IsDisposed) return;

        if (eventType == null) return;

        var _eventType = eventType;

        if (!_eventType.IsGenericType)
            foreach (var type in _eventType.GetInterfaces())
                if (type.IsGenericType)
                {
                    _eventType = type;

                    break;
                }

        if (_eventType.IsGenericType &&
            _eventType.GetGenericArguments().Length > 0)
        {
            _eventType = _eventType.GetGenericArguments().FirstOrDefault();
            if (_eventType == null) return;
        }

        var eventTypeName = _eventType.FullName;
        if (string.IsNullOrWhiteSpace(eventTypeName)) return;

        if (eventType.GetInstance() is not IEventHandler handler) return;

        if (!_handlersDictionary.TryAdd(eventTypeName, [handler])) _handlersDictionary[eventTypeName].Add(handler);
    }

    public async Task Publish(object? sender, IEventParams @event)
    {
        if (IsDisposed) return;

        var eventType = @event.GetType();
        if (eventType == null) return;

        await Publish(sender, eventType, @event);
    }

    public async Task Publish<TEventType, TEventParams>(object? sender, TEventParams @event)
        where TEventParams : IEventParams
    {
        if (IsDisposed) return;

        var eventType = typeof(TEventType);
        if (eventType == null) return;

        await Publish(sender, eventType, @event);
    }

    public async Task Publish<TEventParams>(object? sender, TEventParams @event)
        where TEventParams : IEventParams
    {
        if (IsDisposed) return;

        var eventType = typeof(TEventParams);
        if (eventType == null) return;

        await Publish(sender, eventType, @event);
    }

    public async Task Publish(object? sender, Type eventType, IEventParams @event)
    {
        if (IsDisposed) return;

        ArgumentNullException.ThrowIfNull(eventType, nameof(eventType));

        var _eventType = eventType;

        if (!_eventType.IsGenericType)
            foreach (var type in _eventType.GetInterfaces())
                if (type.IsGenericType)
                {
                    _eventType = type;

                    break;
                }

        if (_eventType.IsGenericType &&
            _eventType.GetGenericArguments().Length > 0)
        {
            _eventType = _eventType.GetGenericArguments().FirstOrDefault();
            if (_eventType == null) return;
        }

        var eventTypeName = _eventType.FullName;
        if (string.IsNullOrWhiteSpace(eventTypeName)) return;

        if (!_handlersDictionary.TryGetValue(eventTypeName, out var handlers)) return;

        if (handlers.Count < 1) return;

        foreach (var eventHandler in handlers) await eventHandler.Handle(sender, @event);
    }
}

public class EventBus<TEventParams> : Singleton<EventBus<TEventParams>>, IEventsBus<TEventParams>
    where TEventParams : IEventParams
{
    private bool IsDisposed { get; set; }
    private static readonly Type CONST_TYPE_IEventHandler = typeof(IEventHandler);

    private readonly ConcurrentDictionary<string, List<IEventHandler<TEventParams>>> _handlersDictionary = new();

    ~EventBus()
    {
        Dispose();
    }

    public void Dispose()
    {
        if (IsDisposed) return;
        
        IsDisposed = true;
        
        _handlersDictionary?.Clear();
        
        GC.SuppressFinalize(this);
    }

    public Type? GetType(TEventParams @params)
    {
        if (IsDisposed) return null;

        var paramsType = @params.GetType();

        return AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(t => t.GetTypes())
            .Where(t =>
            {
                if (t.IsInterface) return false;

                var ntrs = t.GetInterfaces();

                return ntrs.Contains(CONST_TYPE_IEventHandler) &&
                       ntrs.Any(i => i.IsGenericType && i.GetGenericArguments().Contains(paramsType));
            })
            .Select(t => t.GetInterfaces().Contains(CONST_TYPE_IEventHandler) ? t : null)
            .FirstOrDefault();
    }

    public void Subscribe(IEventHandler<TEventParams> handler)
    {
        if (IsDisposed) return;

        var eventType = typeof(TEventParams);
        if (eventType == null) return;

        var eventTypeName = eventType.FullName;
        if (string.IsNullOrWhiteSpace(eventTypeName)) return;

        if (!_handlersDictionary.TryAdd(eventTypeName, [handler])) _handlersDictionary[eventTypeName].Add(handler);
    }

    public async Task Publish(object? sender, TEventParams @event)
    {
        if (IsDisposed) return;

        var eventType = typeof(TEventParams);
        if (eventType == null) return;

        var eventTypeName = eventType.FullName;
        if (string.IsNullOrWhiteSpace(eventTypeName)) return;

        if (!_handlersDictionary.TryGetValue(eventTypeName, out var handlers)) return;

        if (handlers.Count < 1) return;

        foreach (var eventHandler in handlers) await eventHandler.Handle(sender, @event);
    }
}