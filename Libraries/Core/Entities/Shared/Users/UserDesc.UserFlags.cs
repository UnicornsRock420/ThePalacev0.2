using System.Runtime.Serialization;
using Lib.Core.Enums;

namespace Lib.Core.Entities.Shared.Users;

public partial class UserDesc
{
    [IgnoreDataMember] public UserFlags UserFlags;

    [IgnoreDataMember]
    public bool IsGagged
    {
        get => UserFlags.U_Gag.IsSet(UserFlags);
        set => UserFlags = UserFlags.U_Gag.SetBit(UserFlags, value);
    }

    [IgnoreDataMember]
    public bool IsPinned
    {
        get => UserFlags.U_Pin.IsSet(UserFlags);
        set => UserFlags = UserFlags.U_Pin.SetBit(UserFlags, value);
    }

    [IgnoreDataMember]
    public bool IsRejectWhisper
    {
        get => UserFlags.U_RejectWhisper.IsSet(UserFlags);
        set => UserFlags = UserFlags.U_RejectWhisper.SetBit(UserFlags, value);
    }

    [IgnoreDataMember]
    public bool IsRejectEsp
    {
        get => UserFlags.U_RejectEsp.IsSet(UserFlags);
        set => UserFlags = UserFlags.U_RejectEsp.SetBit(UserFlags, value);
    }

    [IgnoreDataMember]
    public bool IsPropGagged
    {
        get => UserFlags.U_PropGag.IsSet(UserFlags);
        set => UserFlags = UserFlags.U_PropGag.SetBit(UserFlags, value);
    }

    [IgnoreDataMember]
    public bool IsNameGagged
    {
        get => UserFlags.U_NameGag.IsSet(UserFlags);
        set => UserFlags = UserFlags.U_NameGag.SetBit(UserFlags, value);
    }

    [IgnoreDataMember]
    public bool IsModerator
    {
        get => UserFlags.U_Moderator.IsSet(UserFlags);
        set => UserFlags = UserFlags.U_Moderator.SetBit(UserFlags, value);
    }

    [IgnoreDataMember]
    public bool IsAdministrator
    {
        get => UserFlags.U_Administrator.IsSet(UserFlags);
        set => UserFlags = UserFlags.U_Administrator.SetBit(UserFlags, value);
    }
}