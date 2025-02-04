using System.Runtime.Serialization;
using ThePalace.Core.Enums;
using uint32 = System.UInt32;

namespace ThePalace.Core.Entities.Shared
{
    public partial class RoomRec
    {
        [IgnoreDataMember]
        public bool IsAuthorLocked
        {
            get => RoomFlags.AuthorLocked.IsBit<RoomFlags, RoomFlags, uint32>(this.RoomFlags);
            set => this.RoomFlags = RoomFlags.AuthorLocked.SetBit<RoomFlags, RoomFlags, uint32, RoomFlags>(this.RoomFlags, value);
        }
        [IgnoreDataMember]
        public bool IsPrivate
        {
            get => RoomFlags.Private.IsBit<RoomFlags, RoomFlags, uint32>(this.RoomFlags);
            set => this.RoomFlags = RoomFlags.Private.SetBit<RoomFlags, RoomFlags, uint32, RoomFlags>(this.RoomFlags, value);
        }
        [IgnoreDataMember]
        public bool IsNoPainting
        {
            get => RoomFlags.NoPainting.IsBit<RoomFlags, RoomFlags, uint32>(this.RoomFlags);
            set => this.RoomFlags = RoomFlags.NoPainting.SetBit<RoomFlags, RoomFlags, uint32, RoomFlags>(this.RoomFlags, value);
        }
        [IgnoreDataMember]
        public bool IsClosed
        {
            get => RoomFlags.Closed.IsBit<RoomFlags, RoomFlags, uint32>(this.RoomFlags);
            set => this.RoomFlags = RoomFlags.Closed.SetBit<RoomFlags, RoomFlags, uint32, RoomFlags>(this.RoomFlags, value);
        }
        [IgnoreDataMember]
        public bool IsCyborgFreeZone
        {
            get => RoomFlags.CyborgFreeZone.IsBit<RoomFlags, RoomFlags, uint32>(this.RoomFlags);
            set => this.RoomFlags = RoomFlags.CyborgFreeZone.SetBit<RoomFlags, RoomFlags, uint32, RoomFlags>(this.RoomFlags, value);
        }
        [IgnoreDataMember]
        public bool IsHidden
        {
            get => RoomFlags.Hidden.IsBit<RoomFlags, RoomFlags, uint32>(this.RoomFlags);
            set => this.RoomFlags = RoomFlags.Hidden.SetBit<RoomFlags, RoomFlags, uint32, RoomFlags>(this.RoomFlags, value);
        }
        [IgnoreDataMember]
        public bool IsNoGuests
        {
            get => RoomFlags.NoGuests.IsBit<RoomFlags, RoomFlags, uint32>(this.RoomFlags);
            set => this.RoomFlags = RoomFlags.NoGuests.SetBit<RoomFlags, RoomFlags, uint32, RoomFlags>(this.RoomFlags, value);
        }
        [IgnoreDataMember]
        public bool IsWizardsOnly
        {
            get => RoomFlags.WizardsOnly.IsBit<RoomFlags, RoomFlags, uint32>(this.RoomFlags);
            set => this.RoomFlags = RoomFlags.WizardsOnly.SetBit<RoomFlags, RoomFlags, uint32, RoomFlags>(this.RoomFlags, value);
        }
        [IgnoreDataMember]
        public bool IsDropZone
        {
            get => RoomFlags.DropZone.IsBit<RoomFlags, RoomFlags, uint32>(this.RoomFlags);
            set => this.RoomFlags = RoomFlags.DropZone.SetBit<RoomFlags, RoomFlags, uint32, RoomFlags>(this.RoomFlags, value);
        }
    }
}