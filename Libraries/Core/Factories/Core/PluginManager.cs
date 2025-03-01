﻿using System.Collections.Concurrent;
using System.Reflection;
using ThePalace.Common.Factories;
using ThePalace.Core.Entities.Core;

namespace ThePalace.Core;

public partial class PluginManager : SingletonDisposable<PluginManager>
{
    private ConcurrentDictionary<Guid, Assembly> _plugins = new();
    public IReadOnlyDictionary<Guid, Assembly> Plugins => _plugins.AsReadOnly();

    private readonly PluginState _pluginContext = new();

    public PluginManager() { }
    ~PluginManager() => this.Dispose(false);

    public override void Dispose()
    {
        if (this.IsDisposed) return;

        base.Dispose();

        try { _plugins?.Clear(); _plugins = null; } catch { }
        try { _pluginContext?.Unload(); } catch { }
    }

    public void LoadPlugins()
    {
        if (this.IsDisposed) return;

        var path = Path.Combine(Environment.CurrentDirectory, "Plugins");
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        var files = Directory.GetFiles(path, "*PLUGIN*.DLL", SearchOption.TopDirectoryOnly);
        foreach (var file in files)
            _plugins.TryAdd(Guid.NewGuid(), _pluginContext.LoadFromAssemblyPath(file));
    }

    public Type GetType(string typeName)
    {
        if (this.IsDisposed) return null;

        foreach (var plugin in _plugins.Values)
            try
            {
                return plugin?.GetType(typeName);
            }
            catch { }

        return null;
    }

    public List<Type> GetTypes()
    {
        if (this.IsDisposed) return null;

        var result = new List<Type>();

        foreach (var plugin in _plugins.Values)
            try
            {
                result.AddRange(plugin?.GetTypes());
            }
            catch { }

        return result;
    }
}