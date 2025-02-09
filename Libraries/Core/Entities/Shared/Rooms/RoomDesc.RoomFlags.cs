using System.Runtime.Serialization;
using ThePalace.Core.Enums.Palace;
using uint32 = System.UInt32;

namespace ThePalace.Core.Entities.Shared
{
    public partial class RoomDesc
    {
        [IgnoreDataMember]
        public bool IsAuthorLocked
        {
            get => RoomFlags.AuthorLocked.IsSet<RoomFlags, RoomFlags, uint32>(this.RoomInfo.RoomFlags);
            set => this.RoomInfo.RoomFlags = RoomFlags.AuthorLocked.SetBit<RoomFlags, RoomFlags, uint32, RoomFlags>(this.RoomInfo.RoomFlags, value);
        }
        [IgnoreDataMember]
        public bool IsPrivate
        {
            get => RoomFlags.Private.IsSet<RoomFlags, RoomFlags, uint32>(this.RoomInfo.RoomFlags);
            set => this.RoomInfo.RoomFlags = RoomFlags.Private.SetBit<RoomFlags, RoomFlags, uint32, RoomFlags>(this.RoomInfo.RoomFlags, value);
        }
        [IgnoreDataMember]
        public bool IsNoPainting
        {
            get => RoomFlags.NoPainting.IsSet<RoomFlags, RoomFlags, uint32>(this.RoomInfo.RoomFlags);
            set => this.RoomInfo.RoomFlags = RoomFlags.NoPainting.SetBit<RoomFlags, RoomFlags, uint32, RoomFlags>(this.RoomInfo.RoomFlags, value);
        }
        [IgnoreDataMember]
        public bool IsClosed
        {
            get => RoomFlags.Closed.IsSet<RoomFlags, RoomFlags, uint32>(this.RoomInfo.RoomFlags);
            set => this.RoomInfo.RoomFlags = RoomFlags.Closed.SetBit<RoomFlags, RoomFlags, uint32, RoomFlags>(this.RoomInfo.RoomFlags, value);
        }
        [IgnoreDataMember]
        public bool IsCyborgFreeZone
        {
            get => RoomFlags.CyborgFreeZone.IsSet<RoomFlags, RoomFlags, uint32>(this.RoomInfo.RoomFlags);
            set => this.RoomInfo.RoomFlags = RoomFlags.CyborgFreeZone.SetBit<RoomFlags, RoomFlags, uint32, RoomFlags>(this.RoomInfo.RoomFlags, value);
        }
        [IgnoreDataMember]
        public bool IsHidden
        {
            get => RoomFlags.Hidden.IsSet<RoomFlags, RoomFlags, uint32>(this.RoomInfo.RoomFlags);
            set => this.RoomInfo.RoomFlags = RoomFlags.Hidden.SetBit<RoomFlags, RoomFlags, uint32, RoomFlags>(this.RoomInfo.RoomFlags, value);
        }
        [IgnoreDataMember]
        public bool IsNoGuests
        {
            get => RoomFlags.NoGuests.IsSet<RoomFlags, RoomFlags, uint32>(this.RoomInfo.RoomFlags);
            set => this.RoomInfo.RoomFlags = RoomFlags.NoGuests.SetBit<RoomFlags, RoomFlags, uint32, RoomFlags>(this.RoomInfo.RoomFlags, value);
        }
        [IgnoreDataMember]
        public bool IsWizardsOnly
        {
            get => RoomFlags.WizardsOnly.IsSet<RoomFlags, RoomFlags, uint32>(this.RoomInfo.RoomFlags);
            set => this.RoomInfo.RoomFlags = RoomFlags.WizardsOnly.SetBit<RoomFlags, RoomFlags, uint32, RoomFlags>(this.RoomInfo.RoomFlags, value);
        }
        [IgnoreDataMember]
        public bool IsDropZone
        {
            get => RoomFlags.DropZone.IsSet<RoomFlags, RoomFlags, uint32>(this.RoomInfo.RoomFlags);
            set => this.RoomInfo.RoomFlags = RoomFlags.DropZone.SetBit<RoomFlags, RoomFlags, uint32, RoomFlags>(this.RoomInfo.RoomFlags, value);
        }
    }
}