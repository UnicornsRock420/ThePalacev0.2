using Lib.Core.Attributes.Serialization;
using Lib.Core.Interfaces.Data;

namespace Lib.Core.Entities.Shared.Assets;

[ByteSize(40)]
public class AssetDesc : AssetRec, IStruct
{
}