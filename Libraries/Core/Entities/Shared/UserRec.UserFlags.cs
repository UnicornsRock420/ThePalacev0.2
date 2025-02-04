using System.Runtime.Serialization;
using ThePalace.Core.Enums;
using uint16 = System.UInt16;

namespace ThePalace.Core.Entities.Shared
{
    public partial class UserRec
    {
        [IgnoreDataMember]
        public UserFlags UserFlags;
        [IgnoreDataMember]
        public bool IsGagged
        {
            get => UserFlags.U_Gag.IsBit<UserFlags, UserFlags, uint16>(this.UserFlags);
            set => this.UserFlags = UserFlags.U_Gag.SetBit<UserFlags, UserFlags, uint16, UserFlags>(this.UserFlags, value);
        }
        [IgnoreDataMember]
        public bool IsPinned
        {
            get => UserFlags.U_Pin.IsBit<UserFlags, UserFlags, uint16>(this.UserFlags);
            set => this.UserFlags = UserFlags.U_Pin.SetBit<UserFlags, UserFlags, uint16, UserFlags>(this.UserFlags, value);
        }
        [IgnoreDataMember]
        public bool IsRejectWhisper
        {
            get => UserFlags.U_RejectWhisper.IsBit<UserFlags, UserFlags, uint16>(this.UserFlags);
            set => this.UserFlags = UserFlags.U_RejectWhisper.SetBit<UserFlags, UserFlags, uint16, UserFlags>(this.UserFlags, value);
        }
        [IgnoreDataMember]
        public bool IsRejectEsp
        {
            get => UserFlags.U_RejectEsp.IsBit<UserFlags, UserFlags, uint16>(this.UserFlags);
            set => this.UserFlags = UserFlags.U_RejectEsp.SetBit<UserFlags, UserFlags, uint16, UserFlags>(this.UserFlags, value);
        }
        [IgnoreDataMember]
        public bool IsPropGagged
        {
            get => UserFlags.U_PropGag.IsBit<UserFlags, UserFlags, uint16>(this.UserFlags);
            set => this.UserFlags = UserFlags.U_PropGag.SetBit<UserFlags, UserFlags, uint16, UserFlags>(this.UserFlags, value);
        }
        [IgnoreDataMember]
        public bool IsNameGagged
        {
            get => UserFlags.U_NameGag.IsBit<UserFlags, UserFlags, uint16>(this.UserFlags);
            set => this.UserFlags = UserFlags.U_NameGag.SetBit<UserFlags, UserFlags, uint16, UserFlags>(this.UserFlags, value);
        }
        [IgnoreDataMember]
        public bool IsModerator
        {
            get => UserFlags.U_Moderator.IsBit<UserFlags, UserFlags, uint16>(this.UserFlags);
            set => this.UserFlags = UserFlags.U_Moderator.SetBit<UserFlags, UserFlags, uint16, UserFlags>(this.UserFlags, value);
        }
        [IgnoreDataMember]
        public bool IsAdministrator
        {
            get => UserFlags.U_Administrator.IsBit<UserFlags, UserFlags, uint16>(this.UserFlags);
            set => this.UserFlags = UserFlags.U_Administrator.SetBit<UserFlags, UserFlags, uint16, UserFlags>(this.UserFlags, value);
        }
    }
}