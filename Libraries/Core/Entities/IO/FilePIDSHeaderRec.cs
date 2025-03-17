using Lib.Core.Attributes.Serialization;
using Lib.Core.Entities.Shared.Types;
using sint32 = int;

namespace Lib.Core.Entities.IO;

[ByteSize(16)]
public class FilePIDSHeaderRec
{
    public AssetSpec AssetSpec;
    public sint32 dataOffset;
    public sint32 dataSize;
}