using System.Runtime.Serialization;
using Lib.Core.Enums;

namespace Lib.Core.Entities.Shared.Rooms;

public partial class RoomDesc
{
    [IgnoreDataMember]
    public bool IsAuthorLocked
    {
        get => RoomFlags.AuthorLocked.IsSet(RoomFlags);
        set => RoomFlags = RoomFlags.AuthorLocked.SetBit(RoomFlags, value);
    }

    [IgnoreDataMember]
    public bool IsPrivate
    {
        get => RoomFlags.Private.IsSet(RoomFlags);
        set => RoomFlags = RoomFlags.Private.SetBit(RoomFlags, value);
    }

    [IgnoreDataMember]
    public bool IsNoPainting
    {
        get => RoomFlags.NoPainting.IsSet(RoomFlags);
        set => RoomFlags = RoomFlags.NoPainting.SetBit(RoomFlags, value);
    }

    [IgnoreDataMember]
    public bool IsClosed
    {
        get => RoomFlags.Closed.IsSet(RoomFlags);
        set => RoomFlags = RoomFlags.Closed.SetBit(RoomFlags, value);
    }

    [IgnoreDataMember]
    public bool IsCyborgFreeZone
    {
        get => RoomFlags.CyborgFreeZone.IsSet(RoomFlags);
        set => RoomFlags = RoomFlags.CyborgFreeZone.SetBit(RoomFlags, value);
    }

    [IgnoreDataMember]
    public bool IsHidden
    {
        get => RoomFlags.Hidden.IsSet(RoomFlags);
        set => RoomFlags = RoomFlags.Hidden.SetBit(RoomFlags, value);
    }

    [IgnoreDataMember]
    public bool IsNoGuests
    {
        get => RoomFlags.NoGuests.IsSet(RoomFlags);
        set => RoomFlags = RoomFlags.NoGuests.SetBit(RoomFlags, value);
    }

    [IgnoreDataMember]
    public bool IsWizardsOnly
    {
        get => RoomFlags.WizardsOnly.IsSet(RoomFlags);
        set => RoomFlags = RoomFlags.WizardsOnly.SetBit(RoomFlags, value);
    }

    [IgnoreDataMember]
    public bool IsDropZone
    {
        get => RoomFlags.DropZone.IsSet(RoomFlags);
        set => RoomFlags = RoomFlags.DropZone.SetBit(RoomFlags, value);
    }
}