using Lib.Core.Attributes.Serialization;
using Lib.Core.Attributes.Strings;
using Lib.Core.Entities.Shared.Types;
using Lib.Core.Interfaces.Data;
using RoomID = short;
using sint16 = short;
using UserID = int;

namespace Lib.Core.Entities.Shared.Users;

public class UserRec : IDisposable, IStruct
{
    public sint16 AwayFlag;
    public sint16 ColorNbr;
    public sint16 FaceNbr;

    [Str31] public string? Name;

    public sint16 NbrProps;
    public sint16 OpenToMsgs;

    [ByteSize(8 * 9)] // AssetSpec(8) * Props(9)
    public AssetSpec[] PropSpec;

    public RoomID RoomID;
    public Point RoomPos;

    public UserID UserId;

    public UserRec()
    {
        RoomPos = new Point();
        PropSpec = new AssetSpec[9];
    }

    public void Dispose()
    {
        PropSpec = null;

        GC.SuppressFinalize(this);
    }
}