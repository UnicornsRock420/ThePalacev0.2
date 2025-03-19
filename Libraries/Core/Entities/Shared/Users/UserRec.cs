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
    public UserID UserId;
    public Point RoomPos;
    [ByteSize(8 * 9)] // AssetSpec(8) * Props(9)
    public AssetSpec[] PropSpec;
    public RoomID RoomID;
    public sint16 FaceNbr;
    public sint16 ColorNbr;
    public sint16 AwayFlag;
    public sint16 OpenToMsgs;
    public sint16 NbrProps;
    [Str31] public string? Name;

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