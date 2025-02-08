using System.Collections.Concurrent;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Core;

namespace CompanyName.MyMeetings.BuildingBlocks.Infrastructure.EventBus
{
    public sealed class EventBus : IEventsBus
    {
        private static readonly Type CONST_TYPE_IINTEGRATIONEVENTHANDLER = typeof(IIntegrationEventHandler);

        private readonly IDictionary<string, List<IIntegrationEventHandler>> _handlersDictionary;

        public static EventBus Instance { get; } = new();

        private EventBus()
        {
            _handlersDictionary = new ConcurrentDictionary<string, List<IIntegrationEventHandler>>();
        }

        public void Dispose()
        {
            _handlersDictionary?.Clear();
        }

        public void Subscribe<T>(IIntegrationEventHandler<T> handler)
            where T : IntegrationEvent
        {
            var eventType = typeof(T).FullName;
            if (eventType != null)
            {
                if (!_handlersDictionary.TryAdd(eventType, [handler]))
                {
                    _handlersDictionary[eventType].Add(handler);
                }
            }
        }

        public async Task Publish<T>(T @event)
            where T : IntegrationEvent
        {
            var eventType = typeof(T).FullName;
            if (eventType == null) return;

            foreach (var integrationEventHandler in _handlersDictionary[eventType])
            {
                if (integrationEventHandler is IIntegrationEventHandler<T> handler)
                {
                    await handler.Handle(@event);
                }
            }
        }

        //public async Task Publish(object @event)
        //{
        //    var eventType = @event.GetType();
        //    if (eventType == null) return;

        //    var eventTypeName = eventType.FullName;

        //    if (eventType.DeclaringType != typeof(IntegrationEvent)) return;

        //    foreach (var integrationEventHandler in _handlersDictionary[eventTypeName])
        //    {
        //        var handlerType = _handlersDictionary[eventTypeName]
        //            .Where(i =>
        //            {
        //                var t = i.GetType();

        //                if (t.IsInterface) return false;

        //                var itrfs = t.GetInterfaces();

        //                if (!itrfs.Contains(CONST_TYPE_IINTEGRATIONEVENTHANDLER)) return false;

        //                if (!itrfs.Any(i => i.IsGenericType && i.GetGenericArguments().Contains(eventType))) return false;

        //                return true;
        //            })
        //            .Select(i1 =>
        //            {
        //                var t = i1.GetType();

        //                foreach (var i2 in t.GetInterfaces() ?? [])
        //                    if (i2 == CONST_TYPE_IINTEGRATIONEVENTHANDLER)
        //                        return t;

        //                return null;
        //            })
        //            .FirstOrDefault();

        //        if (handlerType == null) continue;

        //        var handler = (IIntegrationEventHandler)handlerType.GetInstance();
        //        if (handler == null) continue;

        //        await handler.Handle(@event);
        //    }
        //}

        public void StartConsuming()
        {
            throw new NotImplementedException();
        }
    }
}