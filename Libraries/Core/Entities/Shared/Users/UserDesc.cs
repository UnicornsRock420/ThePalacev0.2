using System.Collections.Concurrent;
using System.Runtime.Serialization;
using Lib.Common.Factories.Core;
using Lib.Core.Interfaces.Data;

namespace Lib.Core.Entities.Shared.Users;

public partial class UserDesc : UserRec, IDisposable, IStruct
{
    [IgnoreDataMember] public ConcurrentDictionary<string, object> Extended;

    public UserDesc()
    {
        Extended = new ConcurrentDictionary<string, object>();
    }

    public UserDesc(UserRec rec) : this()
    {
        UserId = rec.UserId;
        RoomPos = rec.RoomPos;
        PropSpec = rec.PropSpec;
        RoomID = rec.RoomID;
        FaceNbr = rec.FaceNbr;
        ColorNbr = rec.ColorNbr;
        AwayFlag = rec.AwayFlag;
        OpenToMsgs = rec.OpenToMsgs;
        NbrProps = rec.NbrProps;
        Name = rec.Name;
    }

    public UserDesc(UserDesc desc) : this()
    {
        UserId = desc.UserId;
        RoomPos = desc.RoomPos;
        PropSpec = desc.PropSpec;
        RoomID = desc.RoomID;
        FaceNbr = desc.FaceNbr;
        ColorNbr = desc.ColorNbr;
        AwayFlag = desc.AwayFlag;
        OpenToMsgs = desc.OpenToMsgs;
        NbrProps = desc.NbrProps;
        Name = desc.Name;
    }

    public void Dispose()
    {
        Extended
            ?.Values
            ?.Where(_ => _ is IDisposable)
            ?.Cast<IDisposable>()
            ?.ToList()
            ?.ForEach(_ => TCF
                .Options(false)
                .Try(() => _.Dispose())
                .Execute());
        Extended?.Clear();
        Extended = null;

        GC.SuppressFinalize(this);
    }

    ~UserDesc()
    {
        Dispose();
    }
}