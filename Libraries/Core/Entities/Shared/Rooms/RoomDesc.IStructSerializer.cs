using Lib.Core.Entities.Core;
using Lib.Core.Entities.Shared.Types;
using Lib.Core.Enums;
using Lib.Core.Interfaces.Data;
using Lib.Logging.Entities;
using AttributeExts = Lib.Core.Exts.AttributeExts;

namespace Lib.Core.Entities.Shared.Rooms;

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
            Flags = (RoomFlags)ReadInt32();
            FacesID = ReadInt32();
            RoomID = ReadInt16();
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
            LoggerHub.Current.Error(ex);
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
                h.Flags = (HotspotFlags)PeekSInt32(position += 4);
                PeekSInt32(position += 4); //secureInfo
                PeekSInt32(position += 4); //refCon

                var vAxis = PeekSInt16(position += 2);
                var hAxis = PeekSInt16(position += 2);
                h.Loc = new Point(vAxis, hAxis);

                h.HotspotID = PeekSInt16(position += 2);
                h.Dest = PeekSInt16(position += 2);
                h.NbrPts = PeekSInt16(position += 2);
                h.PtsOfst = PeekSInt16(position += 2);
                h.Type = (HotspotTypes)PeekSInt16(position += 2);
                PeekSInt16(position += 2); //groupID
                PeekSInt16(position += 2); //nbrScripts
                PeekSInt16(position += 2); //scriptRecOfst
                h.State = PeekSInt16(position += 2);
                h.NbrStates = PeekSInt16(position += 2);
                h.StateRecOfst = PeekSInt16(position += 2);
                h.NameOfst = PeekSInt16(position += 2);
                h.ScriptTextOfst = PeekSInt16(position += 2);
                PeekSInt16(position += 2); //alignReserved

                if (h.NameOfst > 0 && h.NameOfst < Count)
                    h.Name = PeekPString(32, 1, h.NameOfst);

                if (h.ScriptTextOfst > 0 && h.ScriptTextOfst < Count)
                    h.Script = ReadCString(h.ScriptTextOfst);

                if (h.NbrPts > 0 && h.PtsOfst > 0 && h.PtsOfst <
                    Count - AttributeExts.GetByteSize<Point?>() * h.NbrPts)
                    for (var s = 0; s < h.NbrPts; s++)
                    {
                        Seek(h.PtsOfst + s * AttributeExts.GetByteSize<Point?>());

                        position = _stream.Position;

                        vAxis = PeekSInt16(position += 2);
                        hAxis = PeekSInt16(position += 2);
                        var p = new Point(vAxis, hAxis);

                        h.Vortexes.Add(p);
                    }

                for (var s = 0; s < h.NbrStates; s++)
                {
                    Seek(h.StateRecOfst + s * AttributeExts.GetByteSize<HotspotStateRec>());

                    position = _stream.Position;

                    var hs = new HotspotStateDesc();
                    hs.PictID = PeekSInt16(position += 2);
                    PeekSInt16(position += 2); //reserved

                    vAxis = PeekSInt16(position += 2);
                    hAxis = PeekSInt16(position += 2);
                    hs.PicLoc = new Point(vAxis, hAxis);

                    h.States.Add(hs);
                }

                HotSpots.Add(h);
            }
        }
        catch (Exception ex)
        {
            LoggerHub.Current.Error(ex);
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
            LoggerHub.Current.Error(ex);
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
                ofst = drawCmd.NextOfst = PeekSInt16(position += 2);
                PeekSInt16(); //reserved
                drawCmd.DrawCmd = PeekSInt16(position += 2);
                drawCmd.CmdLength = PeekUInt16(position += 2);
                drawCmd.DataOfst = PeekSInt16(position += 2);
                drawCmd.Data = Data
                    .Skip(drawCmd.DataOfst)
                    .Take(drawCmd.CmdLength)
                    .ToArray();
                //drawCmd.DeserializeData();

                DrawCmds.Add(drawCmd);
            }
        }
        catch (Exception ex)
        {
            LoggerHub.Current.Error(ex);
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
            LoggerHub.Current.Error(ex);
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
            _blobData.WritePString(Name ?? $"Room {RoomID}", 32, 1);

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
                            spot.ScriptTextOfst = (short)_blobData.Count;
                            _blobData.WriteCString(spot.Script);
                        }
                        else
                        {
                            spot.ScriptTextOfst = 0;
                        }

                        //Buffer spot states
                        spot.NbrStates = (short)(spot.States?.Count ?? 0);

                        if (spot.NbrStates > 0)
                        {
                            spot.StateRecOfst = (short)(spot.NbrStates > 0 ? _blobData.Count : 0);

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
                            spot.StateRecOfst = 0;
                        }

                        spot.PtsOfst = 0;

                        if ((spot.Vortexes?.Count ?? 0) > 0)
                        {
                            spot.PtsOfst = (short)_blobData.Count;

                            if ((spot.Vortexes?.Count ?? 0) > 0)
                                foreach (var point in spot.Vortexes)
                                {
                                    _blobData.WriteInt16(point.HAxis);
                                    _blobData.WriteInt16(point.VAxis);
                                }
                        }
                        else
                        {
                            spot.PtsOfst = 0;
                        }

                        if (!string.IsNullOrEmpty(spot.Name))
                        {
                            spot.NameOfst = (short)_blobData.Count;
                            _blobData.WritePString(spot.Name, 32, 1);
                        }
                        else
                        {
                            spot.NameOfst = 0;
                        }

                        //Buffer spotrecs
                        tmp.WriteInt32((int)spot.ScriptEventMask);
                        tmp.WriteInt32((int)spot.Flags);
                        tmp.WriteInt32(0); //secureInfo
                        tmp.WriteInt32(0); //refCon

                        tmp.WriteInt16(spot.Loc.HAxis);
                        tmp.WriteInt16(spot.Loc.VAxis);

                        tmp.WriteInt16(spot.HotspotID);
                        tmp.WriteInt16(spot.Dest);
                        tmp.WriteInt16(spot.NbrPts);
                        tmp.WriteInt16(spot.PtsOfst);
                        tmp.WriteInt16((short)spot.Type);
                        tmp.WriteInt16(0); //groupID
                        tmp.WriteInt16(0); //nbrScripts
                        tmp.WriteInt16(0); //scriptRecOfst
                        tmp.WriteInt16(spot.State);
                        tmp.WriteInt16(spot.NbrStates);
                        tmp.WriteInt16(spot.StateRecOfst);
                        tmp.WriteInt16(spot.NameOfst);
                        tmp.WriteInt16(spot.ScriptTextOfst);
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
                        DrawCmds[i].CmdLength = (ushort)DrawCmds[i].Data.Length;
                        DrawCmds[i].DataOfst = (short)(firstDrawCmd + tmp2.Length +
                                                                   AttributeExts.GetByteSize<DrawCmdRec>() *
                                                                   DrawCmds.Count);
                        DrawCmds[i].NextOfst = (short)(i == DrawCmds.Count - 1
                            ? 0
                            : firstDrawCmd + tmp1.Length + AttributeExts.GetByteSize<DrawCmdRec>());

                        tmp1.WriteInt16(DrawCmds[i].NextOfst);
                        tmp1.WriteInt16(0); //reserved
                        tmp1.WriteInt16(DrawCmds[i].DrawCmd);
                        tmp1.WriteUInt16(DrawCmds[i].CmdLength);
                        tmp1.WriteInt16(DrawCmds[i].DataOfst);
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

                WriteInt32((int)Flags); // Room Flags
                WriteInt32(FacesID); // Default Face ID
                WriteInt16(RoomID); // The Rooms ID
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