using Lib.Core.Entities.Shared.Types;

namespace Lib.Core.Entities.Shared.Rooms;

public class HotspotDesc : IDisposable
{
    public string? Name;
    public string? Script;

    public HotspotRec? SpotInfo;

    public List<HotspotStateDesc>? States;
    public List<Point>? Vortexes;

    public HotspotDesc()
    {
        SpotInfo = new HotspotRec();
        States = [];
        Vortexes = [];
    }

    public void Dispose()
    {
        SpotInfo = null;

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