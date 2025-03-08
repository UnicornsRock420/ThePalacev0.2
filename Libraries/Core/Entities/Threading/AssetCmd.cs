using ThePalace.Common.Interfaces.Threading;
using ThePalace.Core.Entities.Shared.Assets;

namespace ThePalace.Core.Entities.Threading;

public class AssetCmd : ICmd
{
    public AssetDesc AssetInfo { get; set; }
}