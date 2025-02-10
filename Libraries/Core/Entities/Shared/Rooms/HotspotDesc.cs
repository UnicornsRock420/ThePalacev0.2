using ThePalace.Core.Entities.Shared.Types;

namespace ThePalace.Core.Entities.Shared
{
    public partial class HotspotDesc : IDisposable
    {
        public HotspotDesc()
        {
            SpotInfo = new();
            States = new();
            Vortexes = new();
        }

        ~HotspotDesc() => this.Dispose();

        public void Dispose()
        {
            SpotInfo = null;

            this.States?.Clear();
            this.States = null;

            this.Vortexes?.Clear();
            this.Vortexes = null;

            GC.SuppressFinalize(this);
        }

        public HotspotRec? SpotInfo;

        public string? Name;
        public string? Script;

        public List<HotspotStateDesc>? States;
        public List<Point>? Vortexes;
    }
}