using Lib.Core.Entities.Shared.Types;

namespace Lib.Core.Entities.Shared.Rooms;

public class HotspotDesc : HotspotRec, IDisposable
{
    public string? Name;
    public string? Script;

    public List<HotspotStateDesc>? States;
    public List<Point>? Vortexes;

    public HotspotDesc()
    {
        States = [];
        Vortexes = [];
    }

    public void Dispose()
    {
        States?.Clear();
        States = null;

        Vortexes?.Clear();
        Vortexes = null;

        GC.SuppressFinalize(this);
    }

    ~HotspotDesc()
    {
        Dispose();
    }
}