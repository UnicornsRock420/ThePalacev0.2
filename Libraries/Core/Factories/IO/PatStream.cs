﻿using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using Lib.Core.Entities.Shared.Rooms;
using Lib.Core.Entities.Shared.Types;
using Lib.Core.Enums;
using Lib.Core.Exts;
using Lib.Core.Helpers.Core;

namespace Lib.Core.Factories.IO;

public class PatStream : StreamBase
{
    private const string CONST_ROOMFLAGS_FORMAT = "\t{0}";
    private const string CONST_HOTSPOTFLAGS_FORMAT = "\t\t{0}";

    private static readonly string CONST_ROOMFLAGS_NOCYBORGS =
        string.Format(CONST_ROOMFLAGS_FORMAT, RoomFlags.CyborgFreeZone.GetDescription());

    private static readonly string CONST_ROOMFLAGS_DROPZONE =
        string.Format(CONST_ROOMFLAGS_FORMAT, RoomFlags.DropZone.GetDescription());

    private static readonly string CONST_ROOMFLAGS_HIDDEN =
        string.Format(CONST_ROOMFLAGS_FORMAT, RoomFlags.Hidden.GetDescription());

    private static readonly string CONST_ROOMFLAGS_NOGUESTS =
        string.Format(CONST_ROOMFLAGS_FORMAT, RoomFlags.NoGuests.GetDescription());

    private static readonly string CONST_ROOMFLAGS_NOLOOSEPROPS =
        string.Format(CONST_ROOMFLAGS_FORMAT, RoomFlags.RF_NoLooseProps.GetDescription());

    private static readonly string CONST_ROOMFLAGS_NOPAINTING =
        string.Format(CONST_ROOMFLAGS_FORMAT, RoomFlags.NoPainting.GetDescription());

    private static readonly string CONST_ROOMFLAGS_PRIVATE =
        string.Format(CONST_ROOMFLAGS_FORMAT, RoomFlags.Private.GetDescription());

    private static readonly string CONST_ROOMFLAGS_OPERATORSONLY =
        string.Format(CONST_ROOMFLAGS_FORMAT, RoomFlags.WizardsOnly.GetDescription());

    private static readonly string CONST_HOTSPOTFLAGS_DONTMOVEHERE =
        string.Format(CONST_HOTSPOTFLAGS_FORMAT, HotspotFlags.HS_DontMoveHere.GetDescription());

    private static readonly string CONST_HOTSPOTFLAGS_DRAGGABLE =
        string.Format(CONST_HOTSPOTFLAGS_FORMAT, HotspotFlags.HS_Draggable.GetDescription());

    private static readonly string CONST_HOTSPOTFLAGS_FILL =
        string.Format(CONST_HOTSPOTFLAGS_FORMAT, HotspotFlags.HS_Fill.GetDescription());

    private static readonly string CONST_HOTSPOTFLAGS_FORBIDDEN =
        string.Format(CONST_HOTSPOTFLAGS_FORMAT, HotspotFlags.HS_Forbidden.GetDescription());

    private static readonly string CONST_HOTSPOTFLAGS_INVISIBLE =
        string.Format(CONST_HOTSPOTFLAGS_FORMAT, HotspotFlags.HS_Invisible.GetDescription());

    private static readonly string CONST_HOTSPOTFLAGS_LANDINGPAD =
        string.Format(CONST_HOTSPOTFLAGS_FORMAT, HotspotFlags.HS_LandingPad.GetDescription());

    private static readonly string CONST_HOTSPOTFLAGS_MANDATORY =
        string.Format(CONST_HOTSPOTFLAGS_FORMAT, HotspotFlags.HS_Mandatory.GetDescription());

    private static readonly string CONST_HOTSPOTFLAGS_SHADOW =
        string.Format(CONST_HOTSPOTFLAGS_FORMAT, HotspotFlags.HS_Shadow.GetDescription());

    private static readonly string CONST_HOTSPOTFLAGS_SHOWFRAME =
        string.Format(CONST_HOTSPOTFLAGS_FORMAT, HotspotFlags.HS_ShowFrame.GetDescription());

    private static readonly string CONST_HOTSPOTFLAGS_SHOWNAME =
        string.Format(CONST_HOTSPOTFLAGS_FORMAT, HotspotFlags.HS_ShowName.GetDescription());

    private static readonly Regex REGEX_WHITESPACE =
        new(@"\s+", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

    private static readonly Regex REGEX_TOKENS = new(@"^[a-z]+\s+""(.*)""$",
        RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

    private static readonly RoomDesc NULL_ROOMREC = default;
    private static readonly HotspotDesc NULL_HOTSPOTREC = default;
    private static readonly PictureRec NULL_PICTUREREC = default;
    private static readonly LoosePropRec NULL_LOOSEPROPREC = default;

    ~PatStream()
    {
        Dispose();
    }

    public override void Dispose()
    {
        base.Dispose();
    }

    public void Read(out List<RoomDesc> rooms)
    {
        rooms = [];

        var workingRoom = NULL_ROOMREC;
        var workingHotspot = default(HotspotDesc);
        var workingPicture = default(PictureRec);
        var workingLooseProp = default(LoosePropRec);

        var workingScript = (StringBuilder?)null;

        var insideRoom = false;
        var insideProp = false;
        var insideScript = false;
        var insidePicture = false;
        var insideHotspot = false;
        var insidePicts = false;

        var line = string.Empty;

        using (var reader = new StreamReader(_fileStream, Encoding.ASCII))
        {
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();

                var tokens = REGEX_WHITESPACE.Split(line);
                var value = tokens.Length > 1 ? REGEX_TOKENS.Match(line).Groups[1].Value : string.Empty;

                switch (tokens[0].ToUpper())
                {
                    case "ROOM":
                        insideRoom = true;
                        workingRoom = new RoomDesc();
                        workingRoom.HotSpots = [];
                        workingRoom.Pictures = [];
                        workingRoom.LooseProps = [];

                        break;
                    case "ENDROOM":
                        if (!insideHotspot) insideRoom = false;

                        if (workingRoom.RoomID != NULL_ROOMREC.RoomID)
                        {
                            rooms.Add(workingRoom);
                            workingRoom = NULL_ROOMREC;
                        }

                        break;
                    case "DOOR":
                        if (insideRoom)
                        {
                            insideHotspot = true;
                            workingHotspot = new HotspotDesc();
                            workingScript = new StringBuilder();

                            workingHotspot.Type = HotspotTypes.HS_Door;
                        }

                        break;
                    case "ENDDOOR":
                        if (insideRoom && insideHotspot && workingRoom.HotSpots != null &&
                            workingHotspot.HotspotID != NULL_HOTSPOTREC.HotspotID)
                        {
                            insideHotspot = false;

                            workingRoom.HotSpots.Add(workingHotspot);
                            workingHotspot = NULL_HOTSPOTREC;
                        }

                        break;
                    case "SPOT":
                    case "HOTSPOT":
                        if (insideRoom)
                        {
                            insideHotspot = true;
                            workingHotspot = new HotspotDesc();
                            workingScript = new StringBuilder();

                            workingHotspot.Type = HotspotTypes.HS_Normal;
                        }

                        break;
                    case "ENDSPOT":
                    case "ENDHOTSPOT":
                        if (insideRoom && insideHotspot && workingRoom.HotSpots != null &&
                            workingHotspot.HotspotID != NULL_HOTSPOTREC.HotspotID)
                        {
                            insideHotspot = false;

                            workingRoom.HotSpots.Add(workingHotspot);
                            workingHotspot = NULL_HOTSPOTREC;
                        }

                        break;
                    case "BOLT":
                        if (insideRoom)
                        {
                            insideHotspot = true;
                            workingHotspot = new HotspotDesc();
                            workingScript = new StringBuilder();

                            workingHotspot.Type = HotspotTypes.HS_Bolt;
                        }

                        break;
                    case "ENDBOLT":
                        if (insideRoom && insideHotspot && workingRoom.HotSpots != null &&
                            workingHotspot.HotspotID != NULL_HOTSPOTREC.HotspotID)
                        {
                            insideHotspot = false;

                            workingRoom.HotSpots.Add(workingHotspot);
                            workingHotspot = NULL_HOTSPOTREC;
                        }

                        break;
                    case "NAVAREA":
                        if (insideRoom)
                        {
                            insideHotspot = true;
                            workingHotspot = new HotspotDesc();
                            workingScript = new StringBuilder();

                            workingHotspot.Type = HotspotTypes.HS_NavArea;
                        }

                        break;
                    case "ENDNAVAREA":
                        if (insideRoom && insideHotspot && workingRoom.HotSpots != null &&
                            workingHotspot.HotspotID != NULL_HOTSPOTREC.HotspotID)
                        {
                            insideHotspot = false;

                            workingRoom.HotSpots.Add(workingHotspot);
                            workingHotspot = NULL_HOTSPOTREC;
                        }

                        break;
                    case "PICTURE":
                        if (insideRoom)
                        {
                            insidePicture = true;
                            workingPicture = new PictureRec();
                        }

                        if (insidePicture && workingPicture.PicID != NULL_PICTUREREC.PicID && tokens.Length > 2 &&
                            tokens[1] == "ID") workingPicture.PicID = tokens[2].TryParse<short>();

                        break;
                    case "ENDPICTURE":
                        if (insideRoom && insidePicture && workingRoom.Pictures != null &&
                            workingPicture.PicID != NULL_PICTUREREC.PicID)
                        {
                            insidePicture = false;

                            workingRoom.Pictures.Add(workingPicture);
                            workingPicture = NULL_PICTUREREC;
                        }

                        break;
                    case "PROP":
                        if (insideRoom)
                        {
                            insideProp = true;
                            workingLooseProp = new LoosePropRec();
                            workingLooseProp.AssetSpec = new AssetSpec();
                        }

                        break;
                    case "ENDPROP":
                        if (insideRoom && insideProp && workingRoom.LooseProps != null &&
                            workingLooseProp.AssetSpec.Id != NULL_LOOSEPROPREC.AssetSpec.Id)
                        {
                            insideProp = false;

                            workingRoom.LooseProps.Add(workingLooseProp);
                            workingLooseProp = NULL_LOOSEPROPREC;
                        }

                        break;
                    case "SCRIPT":
                        if (insideRoom && insideHotspot) insideScript = true;

                        break;
                    case "ENDSCRIPT":
                        if (insideRoom && insideHotspot && insideScript &&
                            workingHotspot.HotspotID != NULL_HOTSPOTREC.HotspotID &&
                            workingScript != null)
                        {
                            insideScript = false;

                            workingHotspot.Script = workingScript.ToString();
                            workingScript = null;
                        }

                        break;
                    case "PICTS":
                        if (insideRoom && insideHotspot &&
                            workingHotspot.HotspotID != NULL_HOTSPOTREC.HotspotID)
                        {
                            insidePicts = true;
                            workingHotspot.States = [];
                        }

                        break;
                    case "ENDPICTS":
                        if (insideRoom && insideHotspot && insidePicts) insidePicts = false;

                        break;
                    case "ID":
                        if (tokens.Length < 2) break;

                        if (insidePicture && workingPicture.PicID != NULL_PICTUREREC.PicID)
                            workingPicture.PicID = tokens[1].TryParse<short>();
                        else if (insideHotspot &&
                                 workingHotspot.HotspotID != NULL_HOTSPOTREC.HotspotID)
                            workingHotspot.HotspotID = tokens[1].TryParse<short>();
                        else if (insideRoom && workingRoom.RoomID != NULL_ROOMREC.RoomID)
                            workingRoom.RoomID = tokens[1].TryParse<short>();

                        break;
                    case "NAME":
                        if (tokens.Length < 2) break;

                        if (insidePicture && workingPicture.PicID != NULL_PICTUREREC.PicID)
                            workingPicture.Name = value;
                        else if (insideHotspot &&
                                 workingHotspot.HotspotID != NULL_HOTSPOTREC.HotspotID)
                            workingHotspot.Name = value;
                        else if (insideRoom && workingRoom.RoomID != NULL_ROOMREC.RoomID)
                            workingRoom.Name = value;

                        break;
                    case "ARTIST":
                        if (tokens.Length < 2) break;

                        if (insideRoom && workingRoom.RoomID != NULL_ROOMREC.RoomID)
                            workingRoom.Artist = value;

                        break;
                    case "PICT":
                        if (tokens.Length < 2) break;

                        if (insideRoom && workingRoom.RoomID != NULL_ROOMREC.RoomID)
                            workingRoom.Picture = value;

                        break;
                    case "LOCKED":
                        if (tokens.Length < 2) break;

                        if (insideRoom && workingRoom.RoomID != NULL_ROOMREC.RoomID &&
                            !string.IsNullOrWhiteSpace(value))
                        {
                            workingRoom.RoomFlags |= RoomFlags.AuthorLocked;
                            workingRoom.Password = value.GetBytes().ReadCString().GetBytes().DecryptString();
                        }

                        break;
                    case "MAXMEMBERS":
                        if (tokens.Length < 2) break;

                        if (insideRoom && workingRoom.RoomID != NULL_ROOMREC.RoomID)
                            workingRoom.MaxOccupancy = tokens[1].TryParse<short>();

                        break;
                    case "FACES":
                        if (tokens.Length < 2) break;

                        if (insideRoom && workingRoom.RoomID != NULL_ROOMREC.RoomID)
                            workingRoom.FacesID = tokens[1].TryParse<short>();

                        break;
                    case "DROPZONE":
                        if (insideRoom && workingRoom.RoomID != NULL_ROOMREC.RoomID)
                            workingRoom.RoomFlags |= RoomFlags.DropZone;

                        break;
                    case "NOLOOSEPROPS":
                        if (insideRoom && workingRoom.RoomID != NULL_ROOMREC.RoomID)
                            workingRoom.RoomFlags |= RoomFlags.RF_NoLooseProps;

                        break;
                    case "PRIVATE":
                        if (insideRoom && workingRoom.RoomID != NULL_ROOMREC.RoomID)
                            workingRoom.RoomFlags |= RoomFlags.Private;

                        break;
                    case "NOPAINTING":
                        if (insideRoom && workingRoom.RoomID != NULL_ROOMREC.RoomID)
                            workingRoom.RoomFlags |= RoomFlags.NoPainting;

                        break;
                    case "NOCYBORGS":
                        if (insideRoom && workingRoom.RoomID != NULL_ROOMREC.RoomID)
                            workingRoom.RoomFlags |= RoomFlags.CyborgFreeZone;

                        break;
                    case "HIDDEN":
                        if (insideRoom && workingRoom.RoomID != NULL_ROOMREC.RoomID)
                            workingRoom.RoomFlags |= RoomFlags.Hidden;

                        break;
                    case "NOGUESTS":
                        if (insideRoom && workingRoom.RoomID != NULL_ROOMREC.RoomID)
                            workingRoom.RoomFlags |= RoomFlags.NoGuests;

                        break;
                    case "WIZARDSONLY":
                    case "OPERATORSONLY":
                        if (insideRoom && workingRoom.RoomID != NULL_ROOMREC.RoomID)
                            workingRoom.RoomFlags |= RoomFlags.WizardsOnly;

                        break;
                    case "LOCKABLE":
                    case "SHUTABLE":
                        if (insideRoom && insideHotspot) workingHotspot.Type |= HotspotTypes.HS_LockableDoor;

                        break;
                    case "DRAGGABLE":
                        if (insideRoom && insideHotspot &&
                            workingHotspot.HotspotID != NULL_HOTSPOTREC.HotspotID)
                            workingHotspot.Flags |= HotspotFlags.HS_Draggable;

                        break;
                    case "FORBIDDEN":
                        if (insideRoom && insideHotspot &&
                            workingHotspot.HotspotID != NULL_HOTSPOTREC.HotspotID)
                            workingHotspot.Flags |= HotspotFlags.HS_Forbidden;

                        break;
                    case "MANDATORY":
                        if (insideRoom && insideHotspot &&
                            workingHotspot.HotspotID != NULL_HOTSPOTREC.HotspotID)
                            workingHotspot.Flags |= HotspotFlags.HS_Mandatory;

                        break;
                    case "LANDINGPAD":
                        if (insideRoom && insideHotspot &&
                            workingHotspot.HotspotID != NULL_HOTSPOTREC.HotspotID)
                            workingHotspot.Flags |= HotspotFlags.HS_LandingPad;

                        break;
                    case "DONTMOVEHERE":
                        if (insideRoom && insideHotspot &&
                            workingHotspot.HotspotID != NULL_HOTSPOTREC.HotspotID)
                            workingHotspot.Flags |= HotspotFlags.HS_DontMoveHere;

                        break;
                    case "INVISIBLE":
                        if (insideRoom && insideHotspot &&
                            workingHotspot.HotspotID != NULL_HOTSPOTREC.HotspotID)
                            workingHotspot.Flags |= HotspotFlags.HS_Invisible;

                        break;
                    case "SHOWNAME":
                        if (insideRoom && insideHotspot &&
                            workingHotspot.HotspotID != NULL_HOTSPOTREC.HotspotID)
                            workingHotspot.Flags |= HotspotFlags.HS_ShowName;

                        break;
                    case "SHOWFRAME":
                        if (insideRoom && insideHotspot &&
                            workingHotspot.HotspotID != NULL_HOTSPOTREC.HotspotID)
                            workingHotspot.Flags |= HotspotFlags.HS_ShowFrame;

                        break;
                    case "SHADOW":
                        if (insideRoom && insideHotspot &&
                            workingHotspot.HotspotID != NULL_HOTSPOTREC.HotspotID)
                            workingHotspot.Flags |= HotspotFlags.HS_Shadow;

                        break;
                    case "FILL":
                        if (insideRoom && insideHotspot &&
                            workingHotspot.HotspotID != NULL_HOTSPOTREC.HotspotID)
                            workingHotspot.Flags |= HotspotFlags.HS_Fill;

                        break;
                    case "DEST":
                        if (tokens.Length < 2) break;

                        if (insideHotspot && workingHotspot.Dest != null)
                            workingHotspot.Dest = tokens[1].TryParse<short>();

                        break;
                    case "OUTLINE":
                        if (tokens.Length < 3) break;

                        if (insideHotspot && workingHotspot.HotspotID != NULL_HOTSPOTREC.HotspotID)
                        {
                            workingHotspot.Vortexes = [];

                            for (var j = 1; j < tokens.Length; j++)
                            {
                                var coords = tokens[j].Split(',');
                                var h = coords[0].TryParse<short>();
                                var v = coords[1].TryParse<short>();

                                workingHotspot.Vortexes.Add(new Point(h, v));
                            }
                        }

                        break;
                    case "LOC":
                        if (tokens.Length < 2) break;

                    {
                        var coords = tokens[1].Split(',');
                        var h = coords[0].TryParse<short>();
                        var v = coords[1].TryParse<short>();

                        if (insideHotspot && workingHotspot.HotspotID != NULL_HOTSPOTREC.HotspotID)
                            workingHotspot.Loc = new Point(h, v);
                        else if (insideProp && workingLooseProp.AssetSpec.Id != NULL_LOOSEPROPREC.AssetSpec.Id)
                            workingLooseProp.Loc = new Point(h, v);
                    }

                        break;
                    case "PROPID":
                        if (tokens.Length < 2) break;

                        if (insideProp && workingLooseProp.AssetSpec.Id != NULL_LOOSEPROPREC.AssetSpec.Id)
                            workingLooseProp.AssetSpec = new AssetSpec(Convert.ToInt32(tokens[1], 16),
                                workingLooseProp.AssetSpec.Crc);

                        break;
                    case "CRC":
                        if (tokens.Length < 2) break;

                        if (insideProp && workingLooseProp.AssetSpec.Id != NULL_LOOSEPROPREC.AssetSpec.Id)
                            workingLooseProp.AssetSpec = new AssetSpec(workingLooseProp.AssetSpec.Id,
                                Convert.ToUInt32(tokens[1], 16));

                        break;
                    case "TRANSCOLOR":
                        if (tokens.Length < 2) break;

                        if (insideProp && workingPicture.PicID != NULL_PICTUREREC.PicID)
                            workingPicture.TransColor = tokens[1].TryParse<short>();

                        break;
                    default:
                        if (insidePicts && workingHotspot.States != null)
                            for (var j = 0; j < tokens.Length; j++)
                            {
                                var state = tokens[j].Split(',');

                                workingHotspot.States.Add(new HotspotStateDesc
                                {
                                    PictID = state[0].TryParse<short>(),
                                    PicLoc = new Point
                                    {
                                        HAxis = state[1].TryParse<short>(),
                                        VAxis = state[2].TryParse<short>()
                                    }
                                });
                            }
                        else if (insideScript && workingScript != null) workingScript.AppendLine(line);

                        break;
                }
            }
        }
    }

    public void Write(bool printHeader = true, params RoomDesc[] rooms)
    {
        using (var writer = new StreamWriter(_fileStream, Encoding.ASCII))
        {
            if (printHeader)
            {
                var entrance = rooms
                    .Where(r => RoomFlags.DropZone.IsSet(r.RoomFlags))
                    .OrderBy(r => r.RoomID)
                    .FirstOrDefault();

                if (entrance.RoomID > 0)
                {
                    writer.WriteLine($"ENTRANCE {entrance.RoomID}");
                    writer.WriteLine();
                }
            }

            rooms
                .ToList()
                .ForEach(r =>
                {
                    writer.WriteLine("ROOM");

                    if (!string.IsNullOrWhiteSpace(r.Password.ToString()))
                        writer.WriteLine(
                            $"\tLOCKED \"{r.Password.ToString().EncryptString().WritePalaceEscapedString()}\"");

                    writer.WriteLine($"\tID {r.RoomID}");

                    if (r.MaxOccupancy > 0) writer.WriteLine($"\tMAXMEMBERS {r.MaxOccupancy}");

                    if ((r.RoomFlags & RoomFlags.CyborgFreeZone) == RoomFlags.CyborgFreeZone)
                        writer.WriteLine(CONST_ROOMFLAGS_NOCYBORGS);
                    if ((r.RoomFlags & RoomFlags.DropZone) == RoomFlags.DropZone)
                        writer.WriteLine(CONST_ROOMFLAGS_DROPZONE);
                    if ((r.RoomFlags & RoomFlags.Hidden) == RoomFlags.Hidden)
                        writer.WriteLine(CONST_ROOMFLAGS_HIDDEN);
                    if ((r.RoomFlags & RoomFlags.NoGuests) == RoomFlags.NoGuests)
                        writer.WriteLine(CONST_ROOMFLAGS_NOGUESTS);
                    if ((r.RoomFlags & RoomFlags.RF_NoLooseProps) == RoomFlags.RF_NoLooseProps)
                        writer.WriteLine(CONST_ROOMFLAGS_NOLOOSEPROPS);
                    if ((r.RoomFlags & RoomFlags.NoPainting) == RoomFlags.NoPainting)
                        writer.WriteLine(CONST_ROOMFLAGS_NOPAINTING);
                    if ((r.RoomFlags & RoomFlags.Private) == RoomFlags.Private)
                        writer.WriteLine(CONST_ROOMFLAGS_PRIVATE);
                    if ((r.RoomFlags & RoomFlags.WizardsOnly) == RoomFlags.WizardsOnly)
                        writer.WriteLine(CONST_ROOMFLAGS_OPERATORSONLY);

                    writer.WriteLine($"\tNAME \"{r.Name}\"");
                    if (!string.IsNullOrWhiteSpace(r.Picture?.ToString()))
                        writer.WriteLine($"\tPICT \"{r.Picture?.ToString()}\"");
                    if (!string.IsNullOrWhiteSpace(r.Artist?.ToString()))
                        writer.WriteLine($"\tARTIST \"{r.Artist?.ToString()}\"");

                    if (r.LooseProps?.Count > 0)
                        foreach (var looseProp in r.LooseProps)
                        {
                            writer.WriteLine("\tPROP");
                            writer.WriteLine($"\t\tPROPID 0x{$"{looseProp.AssetSpec.Id:X8}"}");

                            if (looseProp.AssetSpec.Crc != 0)
                                writer.WriteLine($"\t\tCRC 0x{$"{looseProp.AssetSpec.Crc:X8}"}");

                            writer.WriteLine($"\t\tLOC {looseProp.Loc.HAxis},{looseProp.Loc.VAxis}");
                            writer.WriteLine("\tENDPROP");
                        }

                    if (r.Pictures != null && r.Pictures?.Count > 0)
                        foreach (var picture in r.Pictures)
                        {
                            writer.WriteLine($"\tPICTURE ID {picture.PicID}");
                            writer.WriteLine($"\t\tNAME \"{picture.Name}\"");
                            if (picture.TransColor > 0) writer.WriteLine($"\tTRANSCOLOR {picture.TransColor}");
                            writer.WriteLine("\tENDPICTURE");
                        }

                    if (r.HotSpots != null && r.HotSpots?.Count > 0)
                        foreach (var hotspot in r.HotSpots)
                        {
                            writer.WriteLine($"\t{hotspot.Type.GetDescription()}");

                            if (hotspot.Type == HotspotTypes.HS_LockableDoor ||
                                hotspot.Type == HotspotTypes.HS_ShutableDoor) writer.WriteLine("\t\tLOCKABLE");

                            writer.WriteLine($"\t\tID {hotspot.HotspotID}");

                            if (hotspot.Dest != 0) writer.WriteLine($"\t\tDEST {hotspot.Dest}");

                            if (!string.IsNullOrWhiteSpace(hotspot.Name))
                                writer.WriteLine($"\t\tID \"{hotspot.Name}\"");

                            if ((hotspot.Flags & HotspotFlags.HS_DontMoveHere) == HotspotFlags.HS_DontMoveHere)
                                writer.WriteLine(CONST_HOTSPOTFLAGS_DONTMOVEHERE);
                            if ((hotspot.Flags & HotspotFlags.HS_Draggable) == HotspotFlags.HS_Draggable)
                                writer.WriteLine(CONST_HOTSPOTFLAGS_DRAGGABLE);
                            if ((hotspot.Flags & HotspotFlags.HS_Fill) == HotspotFlags.HS_Fill)
                                writer.WriteLine(CONST_HOTSPOTFLAGS_FILL);
                            if ((hotspot.Flags & HotspotFlags.HS_Forbidden) == HotspotFlags.HS_Forbidden)
                                writer.WriteLine(CONST_HOTSPOTFLAGS_FORBIDDEN);
                            if ((hotspot.Flags & HotspotFlags.HS_Invisible) == HotspotFlags.HS_Invisible)
                                writer.WriteLine(CONST_HOTSPOTFLAGS_INVISIBLE);
                            if ((hotspot.Flags & HotspotFlags.HS_LandingPad) == HotspotFlags.HS_LandingPad)
                                writer.WriteLine(CONST_HOTSPOTFLAGS_LANDINGPAD);
                            if ((hotspot.Flags & HotspotFlags.HS_Mandatory) == HotspotFlags.HS_Mandatory)
                                writer.WriteLine(CONST_HOTSPOTFLAGS_MANDATORY);
                            if ((hotspot.Flags & HotspotFlags.HS_Shadow) == HotspotFlags.HS_Shadow)
                                writer.WriteLine(CONST_HOTSPOTFLAGS_SHADOW);
                            if ((hotspot.Flags & HotspotFlags.HS_ShowFrame) == HotspotFlags.HS_ShowFrame)
                                writer.WriteLine(CONST_HOTSPOTFLAGS_SHOWFRAME);
                            if ((hotspot.Flags & HotspotFlags.HS_ShowName) == HotspotFlags.HS_ShowName)
                                writer.WriteLine(CONST_HOTSPOTFLAGS_SHOWNAME);

                            if (hotspot.NbrPts > 0 && hotspot.Vortexes?.Count > 0)
                            {
                                writer.Write("\t\tOUTLINE");

                                foreach (var vortex in hotspot.Vortexes)
                                    writer.Write($"  {vortex.HAxis},{vortex.VAxis}");

                                writer.WriteLine();
                            }

                            writer.WriteLine($"\t\tLOC {hotspot.Loc.HAxis},{hotspot.Loc.VAxis}");

                            if (hotspot.States?.Count > 0)
                            {
                                writer.WriteLine("\t\tPICTS");

                                foreach (var state in hotspot.States)
                                    writer.WriteLine(
                                        $"\t\t\t{state.PictID},{state.PicLoc.HAxis},{state.PicLoc.VAxis}");

                                writer.WriteLine("\t\tENDPICTS");
                            }

                            if (!string.IsNullOrWhiteSpace(hotspot.Script))
                            {
                                writer.WriteLine("\t\t\tSCRIPT");

                                writer.WriteLine(hotspot.Script.Trim());

                                writer.WriteLine("\t\t\tENDSCRIPT");
                            }

                            writer.WriteLine($"\tEND{hotspot.Type.GetDescription()}");
                        }

                    writer.WriteLine("ENDROOM");
                    writer.WriteLine();
                });

            if (printHeader) writer.WriteLine("END");
        }
    }
}