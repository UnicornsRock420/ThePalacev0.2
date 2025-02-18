using System.Collections;
using System.Collections.Concurrent;
using ThePalace.Common.Interfaces.Plugins;

namespace ThePalace.Common.Entities.Plugins
{
    public partial class CmdTask : Disposable
    {
        public Task Task;
        public List<IProvider> Providers = new();
        public List<IConsumer> Consumers = new();
        public ConcurrentQueue<Cmd> Queue = new();
        public ManualResetEvent SignalEvent = new(false);

        public CmdTask() { }
        ~CmdTask() =>
            Dispose(false);

        public override void Dispose()
        {
            if (IsDisposed) return;

            base.Dispose();

            if ((Queue?.Count ?? 0) > 0)
            {
                Queue
                    .ToList()
                    .ForEach(c => { try { c.Dispose(); } catch { } });
                Queue.Clear();
            }
            Queue = null;

            if ((Providers?.Count ?? 0) > 0)
            {
                Providers.ForEach(p => { try { p.Dispose(); } catch { } });
                Providers.Clear();
            }
            Providers = null;

            if ((Consumers?.Count ?? 0) > 0)
            {
                Consumers.ForEach(c => { try { c.Dispose(); } catch { } });
                Consumers.Clear();
            }
            Consumers = null;

            SignalEvent?.Set();
            SignalEvent = null;
        }
    }
}