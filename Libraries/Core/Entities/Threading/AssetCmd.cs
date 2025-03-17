using Lib.Common.Interfaces.Threading;
using Lib.Core.Entities.Shared.Assets;

namespace Lib.Core.Entities.Threading;

public class AssetCmd : ICmd
{
    public AssetDesc AssetDesc { get; set; }
}