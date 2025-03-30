using Lib.Core.Attributes.Strings;
using Lib.Core.Entities.Core;
using Lib.Core.Enums;
using Lib.Core.Interfaces.Data;
using RoomID = short;
using sint16 = short;
using sint32 = int;
using uint8 = byte;

namespace Lib.Core.Entities.Shared.Rooms;

public partial class RoomDesc : RawStream, IStruct
{
    public RoomID RoomID;
    public RoomFlags Flags;
    public string? Name;
    [EncryptedString(1, 128)] public string? Password;
    public string? Artist;
    public string? Picture;
    public sint32 FacesID;
    public DateTime? LastModified;
    public sint16 MaxOccupancy;
    public List<PictureRec>? Pictures;
    public List<HotspotDesc>? HotSpots;
    public List<DrawCmdDesc>? DrawCmds;
    public List<LoosePropRec>? LooseProps;

    public RoomDesc()
    {
        HotSpots = [];
        Pictures = [];
        DrawCmds = [];
        LooseProps = [];
    }

    public RoomDesc(RoomRec room)
    {
        HotSpots = [];
        Pictures = [];
        DrawCmds = [];
        LooseProps = [];
    }

    public RoomDesc(uint8[]? data = null) : base(data)
    {
        HotSpots = [];
        Pictures = [];
        DrawCmds = [];
        LooseProps = [];
    }

    public override void Dispose()
    {
        HotSpots?.Clear();
        HotSpots = null;

        Pictures?.Clear();
        Pictures = null;

        DrawCmds?.Clear();
        DrawCmds = null;

        LooseProps?.Clear();
        LooseProps = null;

        base.Dispose();

        GC.SuppressFinalize(this);
    }

    ~RoomDesc()
    {
        Dispose();
    }
}