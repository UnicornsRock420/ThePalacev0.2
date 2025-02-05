using System.Collections.Concurrent;
using ThePalace.Client.Desktop.Interfaces;
using ThePalace.Core.Entities.System;

namespace ThePalace.Core.Client.Core.Models.Threads
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
            this.Dispose(false);

        public override void Dispose()
        {
            if (this.IsDisposed) return;

            base.Dispose();

            if ((this.Queue?.Count ?? 0) > 0)
            {
                this.Queue
                    .ToList()
                    .ForEach(c => { try { c.Dispose(); } catch { } });
                this.Queue.Clear();
            }
            this.Queue = null;

            if ((this.Providers?.Count ?? 0) > 0)
            {
                this.Providers.ForEach(p => { try { p.Dispose(); } catch { } });
                this.Providers.Clear();
            }
            this.Providers = null;

            if ((this.Consumers?.Count ?? 0) > 0)
            {
                this.Consumers.ForEach(c => { try { c.Dispose(); } catch { } });
                this.Consumers.Clear();
            }
            this.Consumers = null;

            this.SignalEvent?.Set();
            this.SignalEvent = null;
        }
    }
}