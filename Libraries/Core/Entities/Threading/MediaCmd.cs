using ThePalace.Common.Interfaces.Threading;

namespace ThePalace.Core.Entities.Threading;

public class MediaCmd(string path) : ICmd
{
    public string Path { get; private set; } = path;
}