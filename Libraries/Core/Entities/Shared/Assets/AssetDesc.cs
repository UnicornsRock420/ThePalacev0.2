using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Data;

namespace ThePalace.Core.Entities.Shared.Assets;

[ByteSize(40)]
public class AssetDesc : RawStream, IStruct
{
    public AssetRec AssetInfo { get; set; }
}