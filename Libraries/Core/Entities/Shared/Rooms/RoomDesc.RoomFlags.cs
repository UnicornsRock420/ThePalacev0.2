using System.Runtime.Serialization;
using ThePalace.Common.Exts.System;
using ThePalace.Core.Enums;
using sint32 = int;

namespace ThePalace.Core.Entities.Shared.Rooms;

public partial class RoomDesc
{
    [IgnoreDataMember]
    public bool IsAuthorLocked
    {
        get => RoomFlags.AuthorLocked.IsSet<RoomFlags, RoomFlags, sint32>(RoomInfo.RoomFlags);
        set => RoomInfo.RoomFlags =
            RoomFlags.AuthorLocked.SetBit<RoomFlags, RoomFlags, sint32, RoomFlags>(RoomInfo.RoomFlags, value);
    }

    [IgnoreDataMember]
    public bool IsPrivate
    {
        get => RoomFlags.Private.IsSet<RoomFlags, RoomFlags, sint32>(RoomInfo.RoomFlags);
        set => RoomInfo.RoomFlags =
            RoomFlags.Private.SetBit<RoomFlags, RoomFlags, sint32, RoomFlags>(RoomInfo.RoomFlags, value);
    }

    [IgnoreDataMember]
    public bool IsNoPainting
    {
        get => RoomFlags.NoPainting.IsSet<RoomFlags, RoomFlags, sint32>(RoomInfo.RoomFlags);
        set => RoomInfo.RoomFlags =
            RoomFlags.NoPainting.SetBit<RoomFlags, RoomFlags, sint32, RoomFlags>(RoomInfo.RoomFlags, value);
    }

    [IgnoreDataMember]
    public bool IsClosed
    {
        get => RoomFlags.Closed.IsSet<RoomFlags, RoomFlags, sint32>(RoomInfo.RoomFlags);
        set => RoomInfo.RoomFlags =
            RoomFlags.Closed.SetBit<RoomFlags, RoomFlags, int, RoomFlags>(RoomInfo.RoomFlags, value);
    }

    [IgnoreDataMember]
    public bool IsCyborgFreeZone
    {
        get => RoomFlags.CyborgFreeZone.IsSet<RoomFlags, RoomFlags, sint32>(RoomInfo.RoomFlags);
        set => RoomInfo.RoomFlags =
            RoomFlags.CyborgFreeZone.SetBit<RoomFlags, RoomFlags, sint32, RoomFlags>(RoomInfo.RoomFlags, value);
    }

    [IgnoreDataMember]
    public bool IsHidden
    {
        get => RoomFlags.Hidden.IsSet<RoomFlags, RoomFlags, sint32>(RoomInfo.RoomFlags);
        set => RoomInfo.RoomFlags =
            RoomFlags.Hidden.SetBit<RoomFlags, RoomFlags, sint32, RoomFlags>(RoomInfo.RoomFlags, value);
    }

    [IgnoreDataMember]
    public bool IsNoGuests
    {
        get => RoomFlags.NoGuests.IsSet<RoomFlags, RoomFlags, sint32>(RoomInfo.RoomFlags);
        set => RoomInfo.RoomFlags =
            RoomFlags.NoGuests.SetBit<RoomFlags, RoomFlags, sint32, RoomFlags>(RoomInfo.RoomFlags, value);
    }

    [IgnoreDataMember]
    public bool IsWizardsOnly
    {
        get => RoomFlags.WizardsOnly.IsSet<RoomFlags, RoomFlags, sint32>(RoomInfo.RoomFlags);
        set => RoomInfo.RoomFlags =
            RoomFlags.WizardsOnly.SetBit<RoomFlags, RoomFlags, sint32, RoomFlags>(RoomInfo.RoomFlags, value);
    }

    [IgnoreDataMember]
    public bool IsDropZone
    {
        get => RoomFlags.DropZone.IsSet<RoomFlags, RoomFlags, sint32>(RoomInfo.RoomFlags);
        set => RoomInfo.RoomFlags =
            RoomFlags.DropZone.SetBit<RoomFlags, RoomFlags, sint32, RoomFlags>(RoomInfo.RoomFlags, value);
    }
}