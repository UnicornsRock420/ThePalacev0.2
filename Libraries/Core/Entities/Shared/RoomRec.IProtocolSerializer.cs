using System.Buffers;
using System.Diagnostics;
using ThePalace.Core.Entities.Network.Shared.Core;
using ThePalace.Core.Enums;
using ThePalace.Core.Exts.Palace;
using ThePalace.Core.Interfaces;
using ThePalace.Core.Types;

namespace ThePalace.Core.Entities.Shared
{
    public partial class RoomRec : IDisposable, IData, IProtocolSerializer
    {
        public void Deserialize(int refNum, Stream reader, SerializerOptions opts = SerializerOptions.None)
        {
            var bytes = new byte[reader.Length];
            reader.Read(bytes, 0, bytes.Length);

            _data = new RawData(bytes);

            var result = new RoomRec();

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
                result.RoomFlags = (RoomFlags)_data.ReadSInt32();
                result.FacesID = _data.ReadSInt32();
                result.RoomID = _data.ReadSInt16();
                roomNameOfst = _data.ReadSInt16();
                pictNameOfst = _data.ReadSInt16();
                artistNameOfst = _data.ReadSInt16();
                passwordOfst = _data.ReadSInt16();
                nbrHotspots = _data.ReadSInt16();
                hotspotOfst = _data.ReadSInt16();
                nbrPictures = _data.ReadSInt16();
                pictureOfst = _data.ReadSInt16();
                nbrDrawCmds = _data.ReadSInt16();
                firstDrawCmd = _data.ReadSInt16();
                nbrPeople = _data.ReadSInt16();
                nbrLProps = _data.ReadSInt16();
                firstLProp = _data.ReadSInt16();
                reserved = _data.ReadSInt16();
                lenVars = _data.ReadSInt16();

                // Get the strings
                result.Name = _data.PeekPString(32, 1, roomNameOfst);
                result.Picture = _data.PeekPString(32, 1, pictNameOfst);
                result.Artist = _data.PeekPString(32, 1, artistNameOfst);
                result.Password = _data.PeekPString(32, 1, passwordOfst);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"RoomRec.Header: {ex.Message}");
            }

            #region Hotspots

            try
            {
                for (var i = 0; i < nbrHotspots; i++)
                {
                    _data.Seek(hotspotOfst + Exts.Palace.AttributeExts.GetByteSize<HotspotRec>(null) * i);

                    var h = new HotspotRec
                    {
                        Vortexes = new(),
                        States = new(),
                    };
                    _data.PeekSInt32(); //scriptEventMask
                    h.Flags = _data.PeekSInt32();
                    _data.PeekSInt32(); //secureInfo
                    _data.PeekSInt32(); //refCon

                    var vAxis = _data.PeekSInt16();
                    var hAxis = _data.PeekSInt16();
                    h.Loc = new Types.Point(hAxis, vAxis);

                    h.HotspotID = _data.PeekSInt16();
                    h.Dest = _data.PeekSInt16();
                    h.NbrPts = _data.PeekSInt16();
                    h.PtsOfst = _data.PeekSInt16();
                    h.Type = (HotspotTypes)_data.PeekSInt16();
                    _data.PeekSInt16(); //groupID
                    _data.PeekSInt16(); //nbrScripts
                    _data.PeekSInt16(); //scriptRecOfst
                    h.State = _data.PeekSInt16();
                    h.NbrStates = _data.PeekSInt16();
                    h.StateRecOfst = _data.PeekSInt16();
                    h.NameOfst = _data.PeekSInt16();
                    h.ScriptTextOfst = _data.PeekSInt16();
                    _data.PeekSInt16(); //alignReserved

                    if (h.NameOfst > 0 && h.NameOfst < _data.Count)
                        h.Name = _data.PeekPString(32, 1, h.NameOfst);

                    if (h.ScriptTextOfst > 0 && h.ScriptTextOfst < _data.Count)
                        h.Script = _data.ReadCString(h.ScriptTextOfst, true);

                    if (h.NbrPts > 0 && h.PtsOfst > 0 && h.PtsOfst < _data.Count - Exts.Palace.AttributeExts.GetByteSize<Types.Point?>(null) * h.NbrPts)
                        for (var s = 0; s < h.NbrPts; s++)
                        {
                            _data.Seek(h.PtsOfst + s * Exts.Palace.AttributeExts.GetByteSize<Types.Point?>(null));

                            vAxis = _data.PeekSInt16();
                            hAxis = _data.PeekSInt16();
                            var p = new Types.Point(hAxis, vAxis);

                            h.Vortexes.Add(p);
                        }

                    for (var s = 0; s < h.NbrStates; s++)
                    {
                        _data.Seek(h.StateRecOfst + s * Exts.Palace.AttributeExts.GetByteSize<HotspotStateRec>(null));

                        var hs = new HotspotStateRec();
                        hs.PictID = _data.PeekSInt16();
                        _data.PeekSInt16(); //reserved

                        vAxis = _data.PeekSInt16();
                        hAxis = _data.PeekSInt16();
                        hs.PicLoc = new Types.Point(hAxis, vAxis);

                        h.States.Add(hs);
                    }

                    result.HotSpots.Add(h);
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
                for (var i = 0; i < nbrPictures; i++)
                {
                    _data.Seek(pictureOfst + Exts.Palace.AttributeExts.GetByteSize<PictureRec>(null) * i);

                    var pict = new PictureRec();
                    pict.RefCon = _data.PeekSInt32();
                    pict.PicID = _data.PeekSInt16();
                    pict.PicNameOfst = _data.PeekSInt16();
                    pict.TransColor = _data.PeekSInt16();
                    _data.PeekSInt16(); //reserved

                    if (pict.PicNameOfst > 0 &&
                        pict.PicNameOfst < _data.Count)
                    {
                        pict.Name = _data.PeekPString(32, 1, pict.PicNameOfst);

                        result.Pictures.Add(pict);
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
                var ofst = firstDrawCmd;

                for (var i = 0; i < nbrDrawCmds; i++)
                {
                    _data.Seek(ofst);

                    var drawCmd = new DrawCmdRec();
                    ofst = drawCmd.NextOfst = _data.PeekSInt16();
                    _data.PeekSInt16(); //reserved
                    drawCmd.DrawCmd = _data.PeekSInt16();
                    drawCmd.CmdLength = _data.PeekUInt16();
                    drawCmd.DataOfst = _data.PeekSInt16();
                    drawCmd.Data = _data.Data
                        .Skip(drawCmd.DataOfst)
                        .Take(drawCmd.CmdLength)
                        .ToArray();
                    //drawCmd.DeserializeData();

                    result.DrawCmds.Add(drawCmd);
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
                var ofst = firstLProp;

                for (var i = 0; i < nbrLProps; i++)
                {
                    _data.Seek(ofst);

                    var prop = new LoosePropRec();
                    ofst = prop.NextOfst = _data.PeekSInt16();
                    _data.PeekSInt16(); //reserved

                    var id = _data.PeekSInt32();
                    var crc = (uint)_data.PeekSInt32();
                    prop.AssetSpec = new AssetSpec(id, crc);

                    prop.Flags = _data.PeekSInt32();
                    _data.PeekSInt32(); //refCon

                    var vAxis = _data.PeekSInt16();
                    var hAxis = _data.PeekSInt16();
                    prop.Loc = new Types.Point(hAxis, vAxis);

                    result.LooseProps.Add(prop);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"RoomRec.LooseProps: {ex.Message}");
            }

            #endregion

            return;
        }

        public void Serialize(Stream writer, SerializerOptions opts = SerializerOptions.None)
        {
            using (var _data = new RawData())
            using (var _blobData = new RawData())
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
                    {
                        foreach (var spot in HotSpots)
                        {
                            // Buffer spot scripts

                            if (!string.IsNullOrEmpty(spot.Script))
                            {
                                spot.ScriptTextOfst = (short)_blobData.Count;
                                _blobData.WriteCString(spot.Script);
                            }
                            else
                                spot.ScriptTextOfst = 0;

                            //Buffer spot states
                            spot.NbrStates = (short)(spot.States?.Count ?? 0);

                            if (spot.NbrStates > 0)
                            {
                                spot.StateRecOfst = (short)(spot.NbrStates > 0 ? _blobData.Count : 0);

                                using (var ms = new MemoryStream())
                                {
                                    foreach (var state in spot.States)
                                    {
                                        state.Serialize(ms);

                                        //_blobData.WriteInt16(state.PictID);
                                        //_blobData.WriteInt16(0); //reserved
                                        //_blobData.WriteBytes(MessagePackSerializer.Serialize(state.PicLoc));
                                    }

                                    _blobData.WriteBytes(ms.ToArray());
                                }
                            }
                            else
                                spot.StateRecOfst = 0;

                            spot.PtsOfst = 0;

                            if ((spot.Vortexes?.Count ?? 0) > 0)
                            {
                                spot.PtsOfst = (short)_blobData.Count;

                                if ((spot.Vortexes?.Count ?? 0) > 0)
                                {
                                    foreach (var point in spot.Vortexes)
                                    {
                                        _blobData.WriteInt16(point.HAxis);
                                        _blobData.WriteInt16(point.VAxis);
                                    }
                                }
                            }
                            else
                                spot.PtsOfst = 0;

                            if (!string.IsNullOrEmpty(spot.Name))
                            {
                                spot.NameOfst = (short)_blobData.Count;
                                _blobData.WritePString(spot.Name, 32, 1);
                            }
                            else
                                spot.NameOfst = 0;

                            //Buffer spotrecs
                            tmp.WriteInt32(spot.ScriptEventMask);
                            tmp.WriteInt32(spot.Flags);
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
                    {
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
                            DrawCmds[i].DataOfst = (short)(firstDrawCmd + tmp2.Length + Exts.Palace.AttributeExts.GetByteSize<DrawCmdRec>(null) * DrawCmds.Count);
                            DrawCmds[i].NextOfst = (short)(i == DrawCmds.Count - 1 ? 0 : firstDrawCmd + tmp1.Length + Exts.Palace.AttributeExts.GetByteSize<DrawCmdRec>(null));

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
                    LooseProps[i].NextOfst = (short)(i == LooseProps.Count - 1 ? 0 : firstLProp + (i + 1) * Exts.Palace.AttributeExts.GetByteSize<LoosePropRec>(null));

                    using (var ms = new MemoryStream())
                    {
                        LooseProps[i].Serialize(ms);

                        _blobData.WriteBytes(ms.ToArray());
                    }
                }

                // Write Map Header
                {
                    var lenVars = (short)_blobData.Count;

                    _data.WriteInt32((int)RoomFlags);                // Room Flags
                    _data.WriteInt32(FacesID);                  // Default Face ID
                    _data.WriteInt16(RoomID);                   // The Rooms ID
                    _data.WriteInt16(roomNameOfst);      // Room Name
                    _data.WriteInt16(pictNameOfst);      // Background Image Offset
                    _data.WriteInt16(artistNameOfst);    // Artist
                    _data.WriteInt16(passwordOfst);      // Password
                    _data.WriteInt16((short)HotSpots.Count);    // Number of Hotspots
                    _data.WriteInt16(hotspotOfst);       // Hotspot Offset
                    _data.WriteInt16((short)HotSpots.Count);    // Number of Pictures
                    _data.WriteInt16(pictureOfst);       // Pictures Offset
                    _data.WriteInt16((short)HotSpots.Count);    // Number of Draw Commands
                    _data.WriteInt16(firstDrawCmd);      // Draw Command Offset
                    _data.WriteInt16(0);                        // Number of People ( Obsolete )
                    _data.WriteInt16((short)LooseProps.Count);  // Number of Props
                    _data.WriteInt16(firstLProp);        // Loose Props Offset
                    _data.WriteInt16(0);                        // Reserved Padding
                    _data.WriteInt16(lenVars);                  // Length of Data Blob
                }

                _data.WriteBytes(_blobData.GetData());

                writer.Write(_data.GetData());
            }
        }
    }
}