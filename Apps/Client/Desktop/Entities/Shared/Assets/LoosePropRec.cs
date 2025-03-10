using System.Runtime.Serialization;
using ThePalace.Common.Factories.System.Collections;
using ThePalace.Core.Entities.Shared.Rooms;

namespace ThePalace.Client.Desktop.Entities.Shared.Assets;

public class LoosePropDesc : Disposable
{
    [IgnoreDataMember] public Bitmap Image;

    ~LoosePropDesc()
    {
        Dispose();
    }

    public override void Dispose()
    {
        Unload();

        base.Dispose();

        GC.SuppressFinalize(this);
    }

    public LoosePropRec PropInfo { get; set; }

    public void Unload()
    {
        try
        {
            Image?.Dispose();
            Image = null;
        }
        catch
        {
        }
    }
}