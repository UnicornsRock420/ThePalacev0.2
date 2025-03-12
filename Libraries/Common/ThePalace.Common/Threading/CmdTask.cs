using System.Collections;
using System.Collections.Concurrent;
using ThePalace.Common.Interfaces.Plugins;

namespace ThePalace.Common.Threading;

public class CmdTask : Disposable
{
    public List<IConsumer> Consumers = [];
    public List<IProvider> Providers = [];
    public ConcurrentQueue<ActionCmd> Queue = new();
    public ManualResetEvent SignalEvent = new(false);
    public Task Task;

    ~CmdTask()
    {
        Dispose();
    }

    public override void Dispose()
    {
        if (IsDisposed) return;

        if ((Queue?.Count ?? 0) > 0)
        {
            Queue
                .ToList()
                .ForEach(c =>
                {
                    try
                    {
                        c.Dispose();
                    }
                    catch
                    {
                    }
                });
            Queue.Clear();
        }

        Queue = null;

        if ((Providers?.Count ?? 0) > 0)
        {
            Providers.ForEach(p =>
            {
                try
                {
                    p.Dispose();
                }
                catch
                {
                }
            });
            Providers.Clear();
        }

        Providers = null;

        if ((Consumers?.Count ?? 0) > 0)
        {
            Consumers.ForEach(c =>
            {
                try
                {
                    c.Dispose();
                }
                catch
                {
                }
            });
            Consumers.Clear();
        }

        Consumers = null;

        SignalEvent?.Set();
        SignalEvent = null;

        base.Dispose();

        GC.SuppressFinalize(this);
    }
}