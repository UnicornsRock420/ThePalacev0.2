﻿using Lib.Core.Attributes.Serialization;
using Lib.Core.Enums;
using Lib.Core.Interfaces.Data;
using RoomID = short;
using sint16 = short;
using sint32 = int;

namespace Lib.Core.Entities.Shared.Rooms;

[ByteSize(40)]
public class RoomRec : IStruct
{
    public RoomFlags RoomFlags;
    public sint32 FacesID;
    public RoomID RoomID;
    public sint16 RoomNameOfst;
    public sint16 PictNameOfst;
    public sint16 ArtistNameOfst;
    public sint16 PasswordOfst;
    public sint16 NbrHotspots;
    public sint16 HotspotOfst;
    public sint16 NbrPictures;
    public sint16 PictureOfst;
    public sint16 NbrDrawCmds;
    public sint16 FirstDrawCmd;
    public sint16 NbrPeople;
    public sint16 NbrLProps;
    public sint16 FirstLProp;
    public sint16 Reserved;
    public sint16 LenVars;
}