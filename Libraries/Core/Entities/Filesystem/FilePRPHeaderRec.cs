using ThePalace.Core.Attributes.Serialization;
using sint32 = System.Int32;

namespace ThePalace.Core.Entities.Filesystem;

[ByteSize(16)]
public partial class FilePRPHeaderRec
{
    public sint32 dataOffset;
    public sint32 dataSize;
    public sint32 assetMapOffset;
    public sint32 assetMapSize;
}