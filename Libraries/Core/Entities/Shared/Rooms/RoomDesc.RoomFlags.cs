using System.Runtime.Serialization;
using Lib.Core.Enums;

namespace Lib.Core.Entities.Shared.Rooms;

public partial class RoomDesc
{
    [IgnoreDataMember]
    public bool IsAuthorLocked
    {
        get => RoomFlags.AuthorLocked.IsSet(Flags);
        set => Flags = RoomFlags.AuthorLocked.SetBit(Flags, value);
    }

    [IgnoreDataMember]
    public bool IsPrivate
    {
        get => RoomFlags.Private.IsSet(Flags);
        set => Flags = RoomFlags.Private.SetBit(Flags, value);
    }

    [IgnoreDataMember]
    public bool IsNoPainting
    {
        get => RoomFlags.NoPainting.IsSet(Flags);
        set => Flags = RoomFlags.NoPainting.SetBit(Flags, value);
    }

    [IgnoreDataMember]
    public bool IsClosed
    {
        get => RoomFlags.Closed.IsSet(Flags);
        set => Flags = RoomFlags.Closed.SetBit(Flags, value);
    }

    [IgnoreDataMember]
    public bool IsCyborgFreeZone
    {
        get => RoomFlags.CyborgFreeZone.IsSet(Flags);
        set => Flags = RoomFlags.CyborgFreeZone.SetBit(Flags, value);
    }

    [IgnoreDataMember]
    public bool IsHidden
    {
        get => RoomFlags.Hidden.IsSet(Flags);
        set => Flags = RoomFlags.Hidden.SetBit(Flags, value);
    }

    [IgnoreDataMember]
    public bool IsNoGuests
    {
        get => RoomFlags.NoGuests.IsSet(Flags);
        set => Flags = RoomFlags.NoGuests.SetBit(Flags, value);
    }

    [IgnoreDataMember]
    public bool IsWizardsOnly
    {
        get => RoomFlags.WizardsOnly.IsSet(Flags);
        set => Flags = RoomFlags.WizardsOnly.SetBit(Flags, value);
    }

    [IgnoreDataMember]
    public bool IsDropZone
    {
        get => RoomFlags.DropZone.IsSet(Flags);
        set => Flags = RoomFlags.DropZone.SetBit(Flags, value);
    }
}