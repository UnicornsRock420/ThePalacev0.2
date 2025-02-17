using System.Runtime.Serialization;
using ThePalace.Core.Enums.Palace;
using sint32 = System.Int32;

namespace ThePalace.Core.Entities.Shared.Rooms
{
    public partial class RoomDesc
    {
        [IgnoreDataMember]
        public bool IsAuthorLocked
        {
            get => RoomFlags.AuthorLocked.IsSet<RoomFlags, RoomFlags, sint32>(this.RoomInfo.RoomFlags);
            set => this.RoomInfo.RoomFlags = RoomFlags.AuthorLocked.SetBit<RoomFlags, RoomFlags, sint32, RoomFlags>(this.RoomInfo.RoomFlags, value);
        }
        [IgnoreDataMember]
        public bool IsPrivate
        {
            get => RoomFlags.Private.IsSet<RoomFlags, RoomFlags, sint32>(this.RoomInfo.RoomFlags);
            set => this.RoomInfo.RoomFlags = RoomFlags.Private.SetBit<RoomFlags, RoomFlags, sint32, RoomFlags>(this.RoomInfo.RoomFlags, value);
        }
        [IgnoreDataMember]
        public bool IsNoPainting
        {
            get => RoomFlags.NoPainting.IsSet<RoomFlags, RoomFlags, sint32>(this.RoomInfo.RoomFlags);
            set => this.RoomInfo.RoomFlags = RoomFlags.NoPainting.SetBit<RoomFlags, RoomFlags, sint32, RoomFlags>(this.RoomInfo.RoomFlags, value);
        }
        [IgnoreDataMember]
        public bool IsClosed
        {
            get => RoomFlags.Closed.IsSet<RoomFlags, RoomFlags, sint32>(this.RoomInfo.RoomFlags);
            set => this.RoomInfo.RoomFlags = RoomFlags.Closed.SetBit<RoomFlags, RoomFlags, int, RoomFlags>(this.RoomInfo.RoomFlags, value);
        }
        [IgnoreDataMember]
        public bool IsCyborgFreeZone
        {
            get => RoomFlags.CyborgFreeZone.IsSet<RoomFlags, RoomFlags, sint32>(this.RoomInfo.RoomFlags);
            set => this.RoomInfo.RoomFlags = RoomFlags.CyborgFreeZone.SetBit<RoomFlags, RoomFlags, sint32, RoomFlags>(this.RoomInfo.RoomFlags, value);
        }
        [IgnoreDataMember]
        public bool IsHidden
        {
            get => RoomFlags.Hidden.IsSet<RoomFlags, RoomFlags, sint32>(this.RoomInfo.RoomFlags);
            set => this.RoomInfo.RoomFlags = RoomFlags.Hidden.SetBit<RoomFlags, RoomFlags, sint32, RoomFlags>(this.RoomInfo.RoomFlags, value);
        }
        [IgnoreDataMember]
        public bool IsNoGuests
        {
            get => RoomFlags.NoGuests.IsSet<RoomFlags, RoomFlags, sint32>(this.RoomInfo.RoomFlags);
            set => this.RoomInfo.RoomFlags = RoomFlags.NoGuests.SetBit<RoomFlags, RoomFlags, sint32, RoomFlags>(this.RoomInfo.RoomFlags, value);
        }
        [IgnoreDataMember]
        public bool IsWizardsOnly
        {
            get => RoomFlags.WizardsOnly.IsSet<RoomFlags, RoomFlags, sint32>(this.RoomInfo.RoomFlags);
            set => this.RoomInfo.RoomFlags = RoomFlags.WizardsOnly.SetBit<RoomFlags, RoomFlags, sint32, RoomFlags>(this.RoomInfo.RoomFlags, value);
        }
        [IgnoreDataMember]
        public bool IsDropZone
        {
            get => RoomFlags.DropZone.IsSet<RoomFlags, RoomFlags, sint32>(this.RoomInfo.RoomFlags);
            set => this.RoomInfo.RoomFlags = RoomFlags.DropZone.SetBit<RoomFlags, RoomFlags, sint32, RoomFlags>(this.RoomInfo.RoomFlags, value);
        }
    }
}