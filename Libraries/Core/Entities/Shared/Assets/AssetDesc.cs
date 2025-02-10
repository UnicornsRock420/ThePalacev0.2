using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Data;

namespace ThePalace.Core.Entities.Shared.Assets
{
    [ByteSize(40)]
    public partial class AssetDesc : RawStream, IStruct
    {
        ~AssetDesc() => Dispose();

        public override void Dispose()
        {
            //try { Image?.Dispose(); Image = null; } catch { }

            base.Dispose();

            GC.SuppressFinalize(this);
        }

        public AssetRec AssetInfo { get; set; }
    }
}