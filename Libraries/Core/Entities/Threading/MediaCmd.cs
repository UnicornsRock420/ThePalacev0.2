using Lib.Common.Interfaces.Threading;

namespace Lib.Core.Entities.Threading;

public class MediaCmd(string path) : ICmd
{
    public string Path { get; private set; } = path;
}