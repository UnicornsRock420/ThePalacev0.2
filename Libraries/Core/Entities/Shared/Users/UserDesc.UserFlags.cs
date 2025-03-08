using System.Runtime.Serialization;
using ThePalace.Common.Exts.System;
using ThePalace.Core.Enums;
using uint16 = ushort;

namespace ThePalace.Core.Entities.Shared.Users;

public partial class UserDesc
{
    [IgnoreDataMember] public UserFlags UserFlags;

    [IgnoreDataMember]
    public bool IsGagged
    {
        get => UserFlags.U_Gag.IsSet<UserFlags, UserFlags, uint16>(UserFlags);
        set => UserFlags = UserFlags.U_Gag.SetBit<UserFlags, UserFlags, uint16, UserFlags>(UserFlags, value);
    }

    [IgnoreDataMember]
    public bool IsPinned
    {
        get => UserFlags.U_Pin.IsSet<UserFlags, UserFlags, uint16>(UserFlags);
        set => UserFlags = UserFlags.U_Pin.SetBit<UserFlags, UserFlags, uint16, UserFlags>(UserFlags, value);
    }

    [IgnoreDataMember]
    public bool IsRejectWhisper
    {
        get => UserFlags.U_RejectWhisper.IsSet<UserFlags, UserFlags, uint16>(UserFlags);
        set => UserFlags = UserFlags.U_RejectWhisper.SetBit<UserFlags, UserFlags, uint16, UserFlags>(UserFlags, value);
    }

    [IgnoreDataMember]
    public bool IsRejectEsp
    {
        get => UserFlags.U_RejectEsp.IsSet<UserFlags, UserFlags, uint16>(UserFlags);
        set => UserFlags = UserFlags.U_RejectEsp.SetBit<UserFlags, UserFlags, uint16, UserFlags>(UserFlags, value);
    }

    [IgnoreDataMember]
    public bool IsPropGagged
    {
        get => UserFlags.U_PropGag.IsSet<UserFlags, UserFlags, uint16>(UserFlags);
        set => UserFlags = UserFlags.U_PropGag.SetBit<UserFlags, UserFlags, uint16, UserFlags>(UserFlags, value);
    }

    [IgnoreDataMember]
    public bool IsNameGagged
    {
        get => UserFlags.U_NameGag.IsSet<UserFlags, UserFlags, uint16>(UserFlags);
        set => UserFlags = UserFlags.U_NameGag.SetBit<UserFlags, UserFlags, uint16, UserFlags>(UserFlags, value);
    }

    [IgnoreDataMember]
    public bool IsModerator
    {
        get => UserFlags.U_Moderator.IsSet<UserFlags, UserFlags, uint16>(UserFlags);
        set => UserFlags = UserFlags.U_Moderator.SetBit<UserFlags, UserFlags, uint16, UserFlags>(UserFlags, value);
    }

    [IgnoreDataMember]
    public bool IsAdministrator
    {
        get => UserFlags.U_Administrator.IsSet<UserFlags, UserFlags, uint16>(UserFlags);
        set => UserFlags = UserFlags.U_Administrator.SetBit<UserFlags, UserFlags, uint16, UserFlags>(UserFlags, value);
    }
}