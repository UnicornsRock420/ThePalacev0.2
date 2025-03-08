using System.Reflection;
using System.Runtime.Loader;

namespace ThePalace.Core.Entities.Core;

public class PluginState : AssemblyLoadContext
{
    protected override Assembly Load(AssemblyName assemblyName)
    {
        return null;
    }
}