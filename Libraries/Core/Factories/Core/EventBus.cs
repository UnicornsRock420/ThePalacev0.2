using System.Collections.Concurrent;
using ThePalace.Common.Exts.System;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Core.Factories.Core;

public class EventBus : SingletonDisposable<EventBus>, IEventsBus
{
    private static readonly Type CONST_TYPE_IEventHandler = typeof(IEventHandler);

    private readonly ConcurrentDictionary<string, List<IEventHandler>> _handlersDictionary = new();

    ~EventBus()
    {
        Dispose();
    }

    public override void Dispose()
    {
        _handlersDictionary?.Clear();

        base.Dispose();

        GC.SuppressFinalize(this);
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
        handlers?.ToList()?.ForEach(h => Subscribe(h));
    }

    public void Subscribe(IEnumerable<IEventHandler> handlers)
    {
        handlers?.ToList()?.ForEach(h => Subscribe(h));
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
        handlers?.ToList()?.ForEach(h => Subscribe(h));
    }

    public void Subscribe<TEventParams>(IEnumerable<IEventHandler<TEventParams>> handlers)
        where TEventParams : IEventParams
    {
        handlers?.ToList()?.ForEach(h => Subscribe(h));
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
        eventTypes?.ToList()?.ForEach(t => Subscribe(t));
    }

    public void Subscribe(IEnumerable<Type> eventTypes)
    {
        eventTypes?.ToList()?.ForEach(t => Subscribe(t));
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

        if (!_handlersDictionary.TryGetValue(eventTypeName, out var handlers)) return;

        if (handlers.Count < 1) return;

        foreach (var eventHandler in handlers) await eventHandler.Handle(sender, @event);
    }
}

public class EventBus<TEventParams> : SingletonDisposable<EventBus<TEventParams>>, IEventsBus<TEventParams>
    where TEventParams : IEventParams
{
    private static readonly Type CONST_TYPE_IEventHandler = typeof(IEventHandler);

    private readonly ConcurrentDictionary<string, List<IEventHandler<TEventParams>>> _handlersDictionary = new();

    ~EventBus()
    {
        Dispose();
    }

    public override void Dispose()
    {
        _handlersDictionary?.Clear();

        base.Dispose();
    }

    public Type? GetType(TEventParams @params)
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

        if (!_handlersDictionary.TryGetValue(eventTypeName, out var handlers)) return;

        if (handlers.Count < 1) return;

        foreach (var eventHandler in handlers) await eventHandler.Handle(sender, @event);
    }
}