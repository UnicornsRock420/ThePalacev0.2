using System.Runtime.Serialization;
using Lib.Core.Enums;

namespace Lib.Core.Entities.Shared.Users;

public partial class UserDesc
{
    [IgnoreDataMember] public UserFlags Flags;

    [IgnoreDataMember]
    public bool IsGagged
    {
        get => UserFlags.U_Gag.IsSet(Flags);
        set => Flags = UserFlags.U_Gag.SetBit(Flags, value);
    }

    [IgnoreDataMember]
    public bool IsPinned
    {
        get => UserFlags.U_Pin.IsSet(Flags);
        set => Flags = UserFlags.U_Pin.SetBit(Flags, value);
    }

    [IgnoreDataMember]
    public bool IsRejectWhisper
    {
        get => UserFlags.U_RejectWhisper.IsSet(Flags);
        set => Flags = UserFlags.U_RejectWhisper.SetBit(Flags, value);
    }

    [IgnoreDataMember]
    public bool IsRejectEsp
    {
        get => UserFlags.U_RejectEsp.IsSet(Flags);
        set => Flags = UserFlags.U_RejectEsp.SetBit(Flags, value);
    }

    [IgnoreDataMember]
    public bool IsPropGagged
    {
        get => UserFlags.U_PropGag.IsSet(Flags);
        set => Flags = UserFlags.U_PropGag.SetBit(Flags, value);
    }

    [IgnoreDataMember]
    public bool IsNameGagged
    {
        get => UserFlags.U_NameGag.IsSet(Flags);
        set => Flags = UserFlags.U_NameGag.SetBit(Flags, value);
    }

    [IgnoreDataMember]
    public bool IsModerator
    {
        get => UserFlags.U_Moderator.IsSet(Flags);
        set => Flags = UserFlags.U_Moderator.SetBit(Flags, value);
    }

    [IgnoreDataMember]
    public bool IsAdministrator
    {
        get => UserFlags.U_Administrator.IsSet(Flags);
        set => Flags = UserFlags.U_Administrator.SetBit(Flags, value);
    }
}