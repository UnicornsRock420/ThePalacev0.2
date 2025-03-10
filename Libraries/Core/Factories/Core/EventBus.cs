using System.Collections.Concurrent;
using ThePalace.Common.Exts.System;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Core.Factories.Core;

public class EventBus : SingletonDisposable<EventBus>, IEventsBus
{
    private static readonly Type CONST_TYPE_IEventHandler = typeof(IEventHandler);
    
    private readonly ConcurrentDictionary<string, List<IEventHandler>> _handlersDictionary = new();

    public static EventBus Instance { get; } = new();

    ~EventBus()
    {
        Dispose();
    }

    public override void Dispose()
    {
        _handlersDictionary?.Clear();

        base.Dispose();
    }

    public Type? GetType(IEventParams @params)
    {
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

    public void Subscribe(params IEventHandler[] handlers)
    {
        if ((handlers?.Length ?? 0) > 0) return;
        
        foreach (var handler in handlers)
            Subscribe(handler);
    }

    public void Subscribe(IEnumerable<IEventHandler> handlers)
    {
        if ((handlers?.Count() ?? 0) > 0) return;
        
        foreach (var handler in handlers)
            Subscribe(handler);
    }

    public void Subscribe(IEventHandler handler)
    {
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
        if ((handlers?.Length ?? 0) > 0) return;
        
        foreach (var handler in handlers)
            Subscribe(handler);
    }

    public void Subscribe<TEventParams>(IEnumerable<IEventHandler<TEventParams>> handlers)
        where TEventParams : IEventParams
    {
        if ((handlers?.Count() ?? 0) > 0) return;
        
        foreach (var handler in handlers)
            Subscribe(handler);
    }

    public void Subscribe<TEventParams>(IEventHandler<TEventParams> handler)
        where TEventParams : IEventParams
    {
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
        if ((eventTypes?.Length ?? 0) > 0) return;
        
        foreach (var eventType in eventTypes)
            Subscribe(eventType);
    }

    public void Subscribe(IEnumerable<Type> eventTypes)
    {
        if ((eventTypes?.Count() ?? 0) > 0) return;
        
        foreach (var eventType in eventTypes)
            Subscribe(eventType);
    }

    public void Subscribe(Type eventType)
    {
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
        var eventType = @event.GetType();
        if (eventType == null) return;

        await Publish(sender, eventType, @event);
    }

    public async Task Publish<TEventType, TEventParams>(object? sender, TEventParams @event)
        where TEventParams : IEventParams
    {
        var eventType = typeof(TEventType);
        if (eventType == null) return;

        await Publish(sender, eventType, @event);
    }

    public async Task Publish<TEventParams>(object? sender, TEventParams @event)
        where TEventParams : IEventParams
    {
        var eventType = typeof(TEventParams);
        if (eventType == null) return;

        await Publish(sender, eventType, @event);
    }

    public async Task Publish(object? sender, Type eventType, IEventParams @event)
    {
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

        if (!_handlersDictionary.ContainsKey(eventTypeName)) return;

        var handlers = _handlersDictionary[eventTypeName];
        if (handlers.Count < 1) return;

        foreach (var eventHandler in handlers) await eventHandler.Handle(sender, @event);
    }
}

public class EventBus<TEventParams> : IEventsBus<TEventParams>
    where TEventParams : IEventParams
{
    private readonly ConcurrentDictionary<string, List<IEventHandler<TEventParams>>> _handlersDictionary;

    private EventBus()
    {
        _handlersDictionary = new ConcurrentDictionary<string, List<IEventHandler<TEventParams>>>();
    }

    public static EventBus<TEventParams> Instance { get; } = new();

    public void Dispose()
    {
        _handlersDictionary?.Clear();

        GC.SuppressFinalize(this);
    }

    public void Subscribe(IEventHandler<TEventParams> handler)
    {
        var eventType = typeof(TEventParams);
        if (eventType == null) return;

        var eventTypeName = eventType.FullName;
        if (string.IsNullOrWhiteSpace(eventTypeName)) return;

        if (!_handlersDictionary.TryAdd(eventTypeName, [handler])) _handlersDictionary[eventTypeName].Add(handler);
    }

    public async Task Publish(object? sender, TEventParams @event)
    {
        var eventType = typeof(TEventParams);
        if (eventType == null) return;

        var eventTypeName = eventType.FullName;
        if (string.IsNullOrWhiteSpace(eventTypeName)) return;

        if (!_handlersDictionary.ContainsKey(eventTypeName)) return;

        var handlers = _handlersDictionary[eventTypeName];
        if (handlers.Count < 1) return;

        foreach (var eventHandler in handlers) await eventHandler.Handle(sender, @event);
    }

    ~EventBus()
    {
        Dispose();
    }
}