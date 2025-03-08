using System.Runtime.Serialization;
using ThePalace.Core.Attributes.Strings;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Data;
using sint16 = short;
using uint8 = byte;

namespace ThePalace.Core.Entities.Shared.Rooms;

public partial class RoomDesc : RawStream, IStruct
{
    [IgnoreDataMember] public string? Artist;

    [IgnoreDataMember] public List<DrawCmdDesc>? DrawCmds;

    [IgnoreDataMember] public List<HotspotDesc>? HotSpots;

    [IgnoreDataMember] public DateTime? LastModified;

    [IgnoreDataMember] public List<LoosePropRec>? LooseProps;

    [IgnoreDataMember] public sint16 MaxOccupancy;

    [IgnoreDataMember] public string? Name;

    [IgnoreDataMember] [EncryptedString()] public string? Password;

    [IgnoreDataMember] public string? Picture;

    [IgnoreDataMember] public List<PictureRec>? Pictures;

    public RoomRec RoomInfo;

    public RoomDesc()
    {
        RoomInfo = new RoomRec();

        HotSpots = new List<HotspotDesc>();
        Pictures = new List<PictureRec>();
        DrawCmds = new List<DrawCmdDesc>();
        LooseProps = new List<LoosePropRec>();
    }

    public RoomDesc(RoomRec room)
    {
        RoomInfo = room;

        HotSpots = new List<HotspotDesc>();
        Pictures = new List<PictureRec>();
        DrawCmds = new List<DrawCmdDesc>();
        LooseProps = new List<LoosePropRec>();
    }

    public RoomDesc(uint8[]? data = null) : base(data)
    {
        RoomInfo = new RoomRec();

        HotSpots = new List<HotspotDesc>();
        Pictures = new List<PictureRec>();
        DrawCmds = new List<DrawCmdDesc>();
        LooseProps = new List<LoosePropRec>();
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
    }

    ~RoomDesc()
    {
        Dispose();
    }
}