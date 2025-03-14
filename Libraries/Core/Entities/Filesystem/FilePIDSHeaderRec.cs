using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Shared.Types;
using sint32 = int;

namespace ThePalace.Core.Entities.Filesystem;

[ByteSize(16)]
public class FilePIDSHeaderRec
{
    public AssetSpec AssetSpec;
    public sint32 dataOffset;
    public sint32 dataSize;
}