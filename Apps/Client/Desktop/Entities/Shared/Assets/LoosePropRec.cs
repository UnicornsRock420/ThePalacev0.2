using System.Runtime.Serialization;
using ThePalace.Core.Entities.Shared.Rooms;

namespace ThePalace.Client.Desktop.Entities.Shared.Assets;

public class LoosePropDesc : LoosePropRec
{
    [IgnoreDataMember] public Bitmap Image;

    ~LoosePropDesc()
    {
        Dispose();
    }

    public void Dispose()
    {
        try
        {
            Image?.Dispose();
            Image = null;
        }
        catch
        {
        }

        GC.SuppressFinalize(this);
    }
}