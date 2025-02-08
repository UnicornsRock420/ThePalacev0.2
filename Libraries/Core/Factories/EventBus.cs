using System.Collections.Concurrent;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Core.Factories
{
    public sealed class EventBus : IEventsBus
    {
        private readonly ConcurrentDictionary<string, List<IEventHandler>> _handlersDictionary;

        public static EventBus Instance { get; } = new();

        private EventBus()
        {
            _handlersDictionary = new();
        }

        ~EventBus() => this.Dispose();

        public void Dispose()
        {
            _handlersDictionary?.Clear();
        }

        public void Subscribe<T>(IEventHandler<T> handler)
            where T : IEventParams
        {
            var eventType = typeof(T);
            if (eventType == null) return;

            var eventTypeName = eventType.FullName;
            if (string.IsNullOrWhiteSpace(eventTypeName)) return;

            if (!_handlersDictionary.TryAdd(eventTypeName, [handler]))
            {
                _handlersDictionary[eventTypeName].Add(handler);
            }
        }

        public void Subscribe(IEventHandler handler)
        {
            var eventType = handler.GetType();
            if (eventType == null) return;

            var eventTypeName = eventType.FullName;
            if (string.IsNullOrWhiteSpace(eventTypeName)) return;

            if (!_handlersDictionary.TryAdd(eventTypeName, [handler]))
            {
                _handlersDictionary[eventTypeName].Add(handler);
            }
        }

        public async Task Publish<T>(object? sender, T @event)
            where T : IEventParams
        {
            var eventType = typeof(T);
            if (eventType == null) return;

            var eventTypeName = eventType.FullName;
            if (string.IsNullOrWhiteSpace(eventTypeName)) return;

            if (!_handlersDictionary.ContainsKey(eventTypeName)) return;

            var handlers = _handlersDictionary[eventTypeName];
            if (handlers.Count < 1) return;

            foreach (var eventHandler in handlers)
            {
                if (eventHandler is IEventHandler<T> handler)
                {
                    await eventHandler.Handle(sender, @event);
                }
            }
        }

        public async Task Publish(object? sender, IEventParams @event)
        {
            var eventType = @event.GetType();
            if (eventType == null) return;

            var eventTypeName = eventType.FullName;
            if (string.IsNullOrWhiteSpace(eventTypeName)) return;

            if (!_handlersDictionary.ContainsKey(eventTypeName)) return;

            var handlers = _handlersDictionary[eventTypeName];
            if (handlers.Count < 1) return;

            foreach (var eventHandler in handlers)
            {
                if (@event.Is(eventType))
                {
                    await eventHandler.Handle(sender, @event);
                }
            }
        }
    }

    public class EventBus<T> : IEventsBus<T>
        where T : IEventParams
    {
        private readonly ConcurrentDictionary<string, List<IEventHandler<T>>> _handlersDictionary;

        public static EventBus<T> Instance { get; } = new();

        private EventBus()
        {
            _handlersDictionary = new();
        }

        ~EventBus() => this.Dispose();

        public void Dispose()
        {
            _handlersDictionary?.Clear();
        }

        public void Subscribe(IEventHandler<T> handler)
        {
            var eventType = typeof(T);
            if (eventType == null) return;

            var eventTypeName = eventType.FullName;
            if (string.IsNullOrWhiteSpace(eventTypeName)) return;

            if (!_handlersDictionary.TryAdd(eventTypeName, [handler]))
            {
                _handlersDictionary[eventTypeName].Add(handler);
            }
        }

        public async Task Publish(object? sender, T @event)
        {
            var eventType = typeof(T);
            if (eventType == null) return;

            var eventTypeName = eventType.FullName;
            if (string.IsNullOrWhiteSpace(eventTypeName)) return;

            if (!_handlersDictionary.ContainsKey(eventTypeName)) return;

            var handlers = _handlersDictionary[eventTypeName];
            if (handlers.Count < 1) return;

            foreach (var eventHandler in handlers)
            {
                if (eventHandler is IEventHandler<T> handler)
                {
                    await eventHandler.Handle(sender, @event);
                }
            }
        }
    }
}