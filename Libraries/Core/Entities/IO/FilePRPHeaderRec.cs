using Lib.Core.Attributes.Serialization;
using sint32 = int;

namespace Lib.Core.Entities.IO;

[ByteSize(16)]
public class FilePRPHeaderRec
{
    public sint32 assetMapOffset;
    public sint32 assetMapSize;
    public sint32 dataOffset;
    public sint32 dataSize;
}