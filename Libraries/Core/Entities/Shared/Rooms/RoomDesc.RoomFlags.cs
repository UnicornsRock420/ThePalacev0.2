using System.Runtime.Serialization;
using ThePalace.Core.Enums;

namespace ThePalace.Core.Entities.Shared.Rooms;

public partial class RoomDesc
{
    [IgnoreDataMember]
    public bool IsAuthorLocked
    {
        get => RoomFlags.AuthorLocked.IsSet(RoomInfo.RoomFlags);
        set => RoomInfo.RoomFlags = RoomFlags.AuthorLocked.SetBit(RoomInfo.RoomFlags, value);
    }

    [IgnoreDataMember]
    public bool IsPrivate
    {
        get => RoomFlags.Private.IsSet(RoomInfo.RoomFlags);
        set => RoomInfo.RoomFlags = RoomFlags.Private.SetBit(RoomInfo.RoomFlags, value);
    }

    [IgnoreDataMember]
    public bool IsNoPainting
    {
        get => RoomFlags.NoPainting.IsSet(RoomInfo.RoomFlags);
        set => RoomInfo.RoomFlags = RoomFlags.NoPainting.SetBit(RoomInfo.RoomFlags, value);
    }

    [IgnoreDataMember]
    public bool IsClosed
    {
        get => RoomFlags.Closed.IsSet(RoomInfo.RoomFlags);
        set => RoomInfo.RoomFlags = RoomFlags.Closed.SetBit(RoomInfo.RoomFlags, value);
    }

    [IgnoreDataMember]
    public bool IsCyborgFreeZone
    {
        get => RoomFlags.CyborgFreeZone.IsSet(RoomInfo.RoomFlags);
        set => RoomInfo.RoomFlags = RoomFlags.CyborgFreeZone.SetBit(RoomInfo.RoomFlags, value);
    }

    [IgnoreDataMember]
    public bool IsHidden
    {
        get => RoomFlags.Hidden.IsSet(RoomInfo.RoomFlags);
        set => RoomInfo.RoomFlags = RoomFlags.Hidden.SetBit(RoomInfo.RoomFlags, value);
    }

    [IgnoreDataMember]
    public bool IsNoGuests
    {
        get => RoomFlags.NoGuests.IsSet(RoomInfo.RoomFlags);
        set => RoomInfo.RoomFlags = RoomFlags.NoGuests.SetBit(RoomInfo.RoomFlags, value);
    }

    [IgnoreDataMember]
    public bool IsWizardsOnly
    {
        get => RoomFlags.WizardsOnly.IsSet(RoomInfo.RoomFlags);
        set => RoomInfo.RoomFlags = RoomFlags.WizardsOnly.SetBit(RoomInfo.RoomFlags, value);
    }

    [IgnoreDataMember]
    public bool IsDropZone
    {
        get => RoomFlags.DropZone.IsSet(RoomInfo.RoomFlags);
        set => RoomInfo.RoomFlags = RoomFlags.DropZone.SetBit(RoomInfo.RoomFlags, value);
    }
}