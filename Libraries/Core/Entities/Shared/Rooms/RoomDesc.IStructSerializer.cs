using System;
using System.Diagnostics;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Entities.Shared.Types;
using ThePalace.Core.Enums;
using ThePalace.Core.Interfaces.Data;
using AttributeExts = ThePalace.Core.Exts.AttributeExts;

namespace ThePalace.Core.Entities.Shared.Rooms;

public partial class RoomDesc : IDisposable, IStructSerializer
{
    public void Deserialize(Stream reader, SerializerOptions opts = SerializerOptions.None)
    {
        var roomNameOfst = (short)0;
        var pictNameOfst = (short)0;
        var artistNameOfst = (short)0;
        var passwordOfst = (short)0;
        var nbrHotspots = (short)0;
        var hotspotOfst = (short)0;
        var nbrPictures = (short)0;
        var pictureOfst = (short)0;
        var nbrDrawCmds = (short)0;
        var firstDrawCmd = (short)0;
        var nbrPeople = (short)0;
        var nbrLProps = (short)0;
        var firstLProp = (short)0;
        var reserved = (short)0;
        var lenVars = (short)0;

        try
        {
            RoomInfo.RoomFlags = (RoomFlags)ReadInt32();
            RoomInfo.FacesID = ReadInt32();
            RoomInfo.RoomID = ReadInt16();
            roomNameOfst = ReadInt16();
            pictNameOfst = ReadInt16();
            artistNameOfst = ReadInt16();
            passwordOfst = ReadInt16();
            nbrHotspots = ReadInt16();
            hotspotOfst = ReadInt16();
            nbrPictures = ReadInt16();
            pictureOfst = ReadInt16();
            nbrDrawCmds = ReadInt16();
            firstDrawCmd = ReadInt16();
            nbrPeople = ReadInt16();
            nbrLProps = ReadInt16();
            firstLProp = ReadInt16();
            reserved = ReadInt16();
            lenVars = ReadInt16();

            // Get the strings
            Name = PeekPString(32, 1, roomNameOfst);
            Picture = PeekPString(32, 1, pictNameOfst);
            Artist = PeekPString(32, 1, artistNameOfst);
            Password = PeekPString(32, 1, passwordOfst);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"RoomRec.Header: {ex.Message}");
        }

        #region Hotspots

        try
        {
            var position = 0L;

            for (var i = 0; i < nbrHotspots; i++)
            {
                Seek(hotspotOfst + AttributeExts.GetByteSize<HotspotRec>() * i);

                position = _stream.Position;

                var h = new HotspotDesc
                {
                    Vortexes = [],
                    States = []
                };
                PeekSInt32(position += 4); //scriptEventMask
                h.SpotInfo.Flags = (HotspotFlags)PeekSInt32(position += 4);
                PeekSInt32(position += 4); //secureInfo
                PeekSInt32(position += 4); //refCon

                var vAxis = PeekSInt16(position += 2);
                var hAxis = PeekSInt16(position += 2);
                h.SpotInfo.Loc = new Point(vAxis, hAxis);

                h.SpotInfo.HotspotID = PeekSInt16(position += 2);
                h.SpotInfo.Dest = PeekSInt16(position += 2);
                h.SpotInfo.NbrPts = PeekSInt16(position += 2);
                h.SpotInfo.PtsOfst = PeekSInt16(position += 2);
                h.SpotInfo.Type = (HotspotTypes)PeekSInt16(position += 2);
                PeekSInt16(position += 2); //groupID
                PeekSInt16(position += 2); //nbrScripts
                PeekSInt16(position += 2); //scriptRecOfst
                h.SpotInfo.State = PeekSInt16(position += 2);
                h.SpotInfo.NbrStates = PeekSInt16(position += 2);
                h.SpotInfo.StateRecOfst = PeekSInt16(position += 2);
                h.SpotInfo.NameOfst = PeekSInt16(position += 2);
                h.SpotInfo.ScriptTextOfst = PeekSInt16(position += 2);
                PeekSInt16(position += 2); //alignReserved

                if (h.SpotInfo.NameOfst > 0 && h.SpotInfo.NameOfst < Count)
                    h.Name = PeekPString(32, 1, h.SpotInfo.NameOfst);

                if (h.SpotInfo.ScriptTextOfst > 0 && h.SpotInfo.ScriptTextOfst < Count)
                    h.Script = ReadCString(h.SpotInfo.ScriptTextOfst);

                if (h.SpotInfo.NbrPts > 0 && h.SpotInfo.PtsOfst > 0 && h.SpotInfo.PtsOfst <
                    Count - AttributeExts.GetByteSize<Point?>() * h.SpotInfo.NbrPts)
                    for (var s = 0; s < h.SpotInfo.NbrPts; s++)
                    {
                        Seek(h.SpotInfo.PtsOfst + s * AttributeExts.GetByteSize<Point?>());

                        position = _stream.Position;

                        vAxis = PeekSInt16(position += 2);
                        hAxis = PeekSInt16(position += 2);
                        var p = new Point(vAxis, hAxis);

                        h.Vortexes.Add(p);
                    }

                for (var s = 0; s < h.SpotInfo.NbrStates; s++)
                {
                    Seek(h.SpotInfo.StateRecOfst + s * AttributeExts.GetByteSize<HotspotStateRec>());

                    position = _stream.Position;

                    var hs = new HotspotStateDesc();
                    hs.StateInfo.PictID = PeekSInt16(position += 2);
                    PeekSInt16(position += 2); //reserved

                    vAxis = PeekSInt16(position += 2);
                    hAxis = PeekSInt16(position += 2);
                    hs.StateInfo.PicLoc = new Point(vAxis, hAxis);

                    h.States.Add(hs);
                }

                HotSpots.Add(h);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"RoomRec.Hotspots: {ex.Message}");
        }

        #endregion

        #region Pictures

        try
        {
            var position = 0L;

            for (var i = 0; i < nbrPictures; i++)
            {
                Seek(pictureOfst + AttributeExts.GetByteSize<PictureRec>() * i);

                position = _stream.Position;

                var pict = new PictureRec();
                pict.RefCon = PeekSInt32(position += 4);
                pict.PicID = PeekSInt16(position += 2);
                pict.PicNameOfst = PeekSInt16(position += 2);
                pict.TransColor = PeekSInt16(position += 2);
                PeekSInt16(); //reserved

                if (pict.PicNameOfst > 0 &&
                    pict.PicNameOfst < Count)
                {
                    pict.Name = PeekPString(32, 1, pict.PicNameOfst);

                    Pictures.Add(pict);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"RoomRec.Pictures: {ex.Message}");
        }

        #endregion

        #region DrawCmds

        try
        {
            var position = 0L;
            var ofst = firstDrawCmd;

            for (var i = 0; i < nbrDrawCmds; i++)
            {
                Seek(ofst);

                position = _stream.Position;

                var drawCmd = new DrawCmdDesc();
                ofst = drawCmd.DrawCmdInfo.NextOfst = PeekSInt16(position += 2);
                PeekSInt16(); //reserved
                drawCmd.DrawCmdInfo.DrawCmd = PeekSInt16(position += 2);
                drawCmd.DrawCmdInfo.CmdLength = PeekUInt16(position += 2);
                drawCmd.DrawCmdInfo.DataOfst = PeekSInt16(position += 2);
                drawCmd.Data = Data
                    .Skip(drawCmd.DrawCmdInfo.DataOfst)
                    .Take(drawCmd.DrawCmdInfo.CmdLength)
                    .ToArray();
                //drawCmd.DeserializeData();

                DrawCmds.Add(drawCmd);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"RoomRec.DrawCmds: {ex.Message}");
        }

        #endregion

        #region Loose Props

        try
        {
            var position = 0L;
            var ofst = firstLProp;

            for (var i = 0; i < nbrLProps; i++)
            {
                Seek(ofst);

                position = _stream.Position;

                var prop = new LoosePropRec();
                ofst = prop.NextOfst = PeekSInt16(position += 2);
                PeekSInt16(position += 2); //reserved

                var id = PeekSInt32(position += 4);
                var crc = (uint)PeekSInt32(position += 4);
                prop.AssetSpec = new AssetSpec(id, crc);

                prop.Flags = PeekSInt32(position += 4);
                PeekSInt32(position += 4); //refCon

                var vAxis = PeekSInt16(position += 2);
                var hAxis = PeekSInt16(position += 2);
                prop.Loc = new Point(hAxis, vAxis);

                LooseProps.Add(prop);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"RoomRec.LooseProps: {ex.Message}");
        }

        #endregion
    }

    public void Serialize(Stream writer, SerializerOptions opts = SerializerOptions.None)
    {
        using (var _data = new RawStream()) //RawData()
        using (var _blobData = new RawStream()) //RawData()
        {
            // ALIGN header
            _blobData.PadBytes(4);

            // Room Name
            var roomNameOfst = (short)_blobData.Count;
            _blobData.WritePString(Name ?? $"Room {RoomInfo.RoomID}", 32, 1);

            // Artist Name
            var artistNameOfst = (short)_blobData.Count;
            _blobData.WritePString(Artist ?? string.Empty, 32, 1);

            var pictNameOfst = (short)_blobData.Count;
            _blobData.WritePString(Picture ?? "clouds.gif", 32, 1);

            // Password
            var passwordOfst = (short)_blobData.Count;
            _blobData.WritePString(Password ?? string.Empty, 32, 1);

            //Start Spots
            var hotspotOfst = (short)0;

            using (var tmp = new MemoryStream())
            {
                if ((HotSpots?.Count ?? 0) > 0)
                    foreach (var spot in HotSpots)
                    {
                        // Buffer spot scripts
                        if (!string.IsNullOrEmpty(spot.Script))
                        {
                            spot.SpotInfo.ScriptTextOfst = (short)_blobData.Count;
                            _blobData.WriteCString(spot.Script);
                        }
                        else
                        {
                            spot.SpotInfo.ScriptTextOfst = 0;
                        }

                        //Buffer spot states
                        spot.SpotInfo.NbrStates = (short)(spot.States?.Count ?? 0);

                        if (spot.SpotInfo.NbrStates > 0)
                        {
                            spot.SpotInfo.StateRecOfst = (short)(spot.SpotInfo.NbrStates > 0 ? _blobData.Count : 0);

                            using (var ms = new MemoryStream())
                            {
                                foreach (var state in spot.States) state.Serialize(ms, opts);
                                //_blobData.WriteInt16(state.PictID);
                                //_blobData.WriteInt16(0); //reserved
                                //_blobData.WriteBytes(MessagePackSerializer.Serialize(state.PicLoc));
                                _blobData.WriteBytes(ms.ToArray());
                            }
                        }
                        else
                        {
                            spot.SpotInfo.StateRecOfst = 0;
                        }

                        spot.SpotInfo.PtsOfst = 0;

                        if ((spot.Vortexes?.Count ?? 0) > 0)
                        {
                            spot.SpotInfo.PtsOfst = (short)_blobData.Count;

                            if ((spot.Vortexes?.Count ?? 0) > 0)
                                foreach (var point in spot.Vortexes)
                                {
                                    _blobData.WriteInt16(point.HAxis);
                                    _blobData.WriteInt16(point.VAxis);
                                }
                        }
                        else
                        {
                            spot.SpotInfo.PtsOfst = 0;
                        }

                        if (!string.IsNullOrEmpty(spot.Name))
                        {
                            spot.SpotInfo.NameOfst = (short)_blobData.Count;
                            _blobData.WritePString(spot.Name, 32, 1);
                        }
                        else
                        {
                            spot.SpotInfo.NameOfst = 0;
                        }

                        //Buffer spotrecs
                        tmp.WriteInt32((int)spot.SpotInfo.ScriptEventMask);
                        tmp.WriteInt32((int)spot.SpotInfo.Flags);
                        tmp.WriteInt32(0); //secureInfo
                        tmp.WriteInt32(0); //refCon

                        tmp.WriteInt16(spot.SpotInfo.Loc.HAxis);
                        tmp.WriteInt16(spot.SpotInfo.Loc.VAxis);

                        tmp.WriteInt16(spot.SpotInfo.HotspotID);
                        tmp.WriteInt16(spot.SpotInfo.Dest);
                        tmp.WriteInt16(spot.SpotInfo.NbrPts);
                        tmp.WriteInt16(spot.SpotInfo.PtsOfst);
                        tmp.WriteInt16((short)spot.SpotInfo.Type);
                        tmp.WriteInt16(0); //groupID
                        tmp.WriteInt16(0); //nbrScripts
                        tmp.WriteInt16(0); //scriptRecOfst
                        tmp.WriteInt16(spot.SpotInfo.State);
                        tmp.WriteInt16(spot.SpotInfo.NbrStates);
                        tmp.WriteInt16(spot.SpotInfo.StateRecOfst);
                        tmp.WriteInt16(spot.SpotInfo.NameOfst);
                        tmp.WriteInt16(spot.SpotInfo.ScriptTextOfst);
                        tmp.WriteInt16(0); //alignReserved
                    }

                _blobData.PadBytes(4);

                hotspotOfst = (short)(HotSpots.Count > 0 ? _blobData.Count : 0);

                _blobData.WriteBytes(tmp.ToArray());
            }

            //Start Pictures
            var pictureOfst = (short)0;

            using (var tmp = new MemoryStream())
            {
                if ((Pictures?.Count ?? 0) > 0)
                    foreach (var pict in Pictures)
                    {
                        pict.PicNameOfst = (short)_blobData.Count;
                        _blobData.WritePString(pict.Name, 32, 1);

                        tmp.WriteInt32(pict.RefCon);
                        tmp.WriteInt16(pict.PicID);
                        tmp.WriteInt16(pict.PicNameOfst);
                        tmp.WriteInt16(pict.TransColor);
                        tmp.WriteInt16(0); //reserved
                    }

                pictureOfst = (short)(Pictures.Count > 0 ? _blobData.Count : 0);

                _blobData.WriteBytes(tmp.ToArray());
            }


            // Start DrawCmds
            var firstDrawCmd = (short)0;

            using (var tmp1 = new MemoryStream())
            {
                _blobData.PadBytes(4);

                firstDrawCmd = (short)((DrawCmds?.Count ?? 0) > 0 ? _blobData.Count : 0);

                using (var tmp2 = new MemoryStream())
                {
                    for (var i = 0; i < (DrawCmds?.Count ?? 0); i++)
                    {
                        DrawCmds[i].DrawCmdInfo.CmdLength = (ushort)DrawCmds[i].Data.Length;
                        DrawCmds[i].DrawCmdInfo.DataOfst = (short)(firstDrawCmd + tmp2.Length +
                                                                   AttributeExts.GetByteSize<DrawCmdRec>() *
                                                                   DrawCmds.Count);
                        DrawCmds[i].DrawCmdInfo.NextOfst = (short)(i == DrawCmds.Count - 1
                            ? 0
                            : firstDrawCmd + tmp1.Length + AttributeExts.GetByteSize<DrawCmdRec>());

                        tmp1.WriteInt16(DrawCmds[i].DrawCmdInfo.NextOfst);
                        tmp1.WriteInt16(0); //reserved
                        tmp1.WriteInt16(DrawCmds[i].DrawCmdInfo.DrawCmd);
                        tmp1.WriteUInt16(DrawCmds[i].DrawCmdInfo.CmdLength);
                        tmp1.WriteInt16(DrawCmds[i].DrawCmdInfo.DataOfst);
                        tmp2.Write(DrawCmds[i].Data);
                    }

                    tmp1.Write(tmp2.ToArray());
                }

                _blobData.WriteBytes(tmp1.ToArray());
            }

            // Start Loose Props
            var firstLProp = (short)((LooseProps?.Count ?? 0) > 0 ? _blobData.Count : 0);

            for (var i = 0; i < (LooseProps?.Count ?? 0); i++)
            {
                LooseProps[i].NextOfst = (short)(i == LooseProps.Count - 1
                    ? 0
                    : firstLProp + (i + 1) * AttributeExts.GetByteSize<LoosePropRec>());

                using (var ms = new MemoryStream())
                {
                    LooseProps[i].Serialize(ms, opts);

                    _blobData.WriteBytes(ms.ToArray());
                }
            }

            // Write Map Header
            {
                var lenVars = (short)_blobData.Count;

                WriteInt32((int)RoomInfo.RoomFlags); // Room Flags
                WriteInt32(RoomInfo.FacesID); // Default Face ID
                WriteInt16(RoomInfo.RoomID); // The Rooms ID
                WriteInt16(roomNameOfst); // Room Name
                WriteInt16(pictNameOfst); // Background Image Offset
                WriteInt16(artistNameOfst); // Artist
                WriteInt16(passwordOfst); // Password
                WriteInt16((short)HotSpots.Count); // Number of Hotspots
                WriteInt16(hotspotOfst); // Hotspot Offset
                WriteInt16((short)HotSpots.Count); // Number of Pictures
                WriteInt16(pictureOfst); // Pictures Offset
                WriteInt16((short)HotSpots.Count); // Number of Draw Commands
                WriteInt16(firstDrawCmd); // Draw Command Offset
                WriteInt16(0); // Number of People ( Obsolete )
                WriteInt16((short)LooseProps.Count); // Number of Props
                WriteInt16(firstLProp); // Loose Props Offset
                WriteInt16(0); // Reserved Padding
                WriteInt16(lenVars); // Length of Data Blob
            }

            _stream.Write(_blobData.Stream.ToArray());

            writer.Write(Stream.ToArray());
        }
    }
}