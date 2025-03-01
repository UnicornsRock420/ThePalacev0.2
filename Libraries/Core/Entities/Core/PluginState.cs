using System.Reflection;
using System.Runtime.Loader;

namespace ThePalace.Core.Entities.Core;

public partial class PluginState : AssemblyLoadContext
{
    protected override Assembly Load(AssemblyName assemblyName) => null;
}