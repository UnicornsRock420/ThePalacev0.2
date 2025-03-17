using System.Collections.Concurrent;
using System.Reflection;
using Lib.Core.Entities.Core;

namespace Lib.Core.Factories.Core;

public class PluginManager : SingletonDisposable<PluginManager>
{
    private readonly PluginState _pluginContext = new();
    private ConcurrentDictionary<Guid, Assembly> _plugins = new();
    public IReadOnlyDictionary<Guid, Assembly> Plugins => _plugins.AsReadOnly();

    ~PluginManager()
    {
        Dispose();
    }

    public override void Dispose()
    {
        if (IsDisposed) return;

        base.Dispose();

        try
        {
            _plugins?.Clear();
            _plugins = null;
        }
        catch
        {
        }

        try
        {
            _pluginContext?.Unload();
        }
        catch
        {
        }
    }

    public void LoadPlugins()
    {
        if (IsDisposed) return;

        var path = Path.Combine(Environment.CurrentDirectory, "Plugins");
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        var files = Directory.GetFiles(path, "*PLUGIN*.DLL", SearchOption.TopDirectoryOnly);
        foreach (var file in files)
            _plugins.TryAdd(Guid.NewGuid(), _pluginContext.LoadFromAssemblyPath(file));
    }

    public Type GetType(string typeName)
    {
        if (IsDisposed) return null;

        foreach (var plugin in _plugins.Values)
            try
            {
                return plugin?.GetType(typeName);
            }
            catch
            {
            }

        return null;
    }

    public List<Type> GetTypes()
    {
        if (IsDisposed) return null;

        var result = new List<Type>();

        foreach (var plugin in _plugins.Values)
            try
            {
                result.AddRange(plugin?.GetTypes());
            }
            catch
            {
            }

        return result;
    }
}