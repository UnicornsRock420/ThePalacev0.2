using System.Buffers;
using System.Diagnostics;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Entities.Shared.Rooms;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Exts.Palace;
using ThePalace.Core.Interfaces.Data;
using ThePalace.Core.Entities.Shared.Types;

namespace ThePalace.Core.Entities.Shared
{
    public partial class RoomDesc : IDisposable, IStructSerializer
    {
        public void Deserialize(ref int refNum, Stream reader, SerializerOptions opts = SerializerOptions.None)
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
                this.RoomInfo.RoomFlags = (RoomFlags)this.ReadInt32();
                this.RoomInfo.FacesID = this.ReadInt32();
                this.RoomInfo.RoomID = this.ReadInt16();
                roomNameOfst = this.ReadInt16();
                pictNameOfst = this.ReadInt16();
                artistNameOfst = this.ReadInt16();
                passwordOfst = this.ReadInt16();
                nbrHotspots = this.ReadInt16();
                hotspotOfst = this.ReadInt16();
                nbrPictures = this.ReadInt16();
                pictureOfst = this.ReadInt16();
                nbrDrawCmds = this.ReadInt16();
                firstDrawCmd = this.ReadInt16();
                nbrPeople = this.ReadInt16();
                nbrLProps = this.ReadInt16();
                firstLProp = this.ReadInt16();
                reserved = this.ReadInt16();
                lenVars = this.ReadInt16();

                // Get the strings
                this.Name = this.PeekPString(32, 1, roomNameOfst);
                this.Picture = this.PeekPString(32, 1, pictNameOfst);
                this.Artist = this.PeekPString(32, 1, artistNameOfst);
                this.Password = this.PeekPString(32, 1, passwordOfst);
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
                    this.Seek(hotspotOfst + Exts.Palace.AttributeExts.GetByteSize<HotspotRec>() * i);

                    var h = new HotspotDesc
                    {
                        Vortexes = new(),
                        States = new(),
                    };
                    this.PeekSInt32(); //scriptEventMask
                    h.SpotInfo.Flags = (HotspotFlags)this.PeekSInt32();
                    this.PeekSInt32(); //secureInfo
                    this.PeekSInt32(); //refCon

                    var vAxis = this.PeekSInt16();
                    var hAxis = this.PeekSInt16();
                    h.SpotInfo.Loc = new Types.Point(vAxis, hAxis);

                    h.SpotInfo.HotspotID = this.PeekSInt16();
                    h.SpotInfo.Dest = this.PeekSInt16();
                    h.SpotInfo.NbrPts = this.PeekSInt16();
                    h.SpotInfo.PtsOfst = this.PeekSInt16();
                    h.SpotInfo.Type = (HotspotTypes)this.PeekSInt16();
                    this.PeekSInt16(); //groupID
                    this.PeekSInt16(); //nbrScripts
                    this.PeekSInt16(); //scriptRecOfst
                    h.SpotInfo.State = this.PeekSInt16();
                    h.SpotInfo.NbrStates = this.PeekSInt16();
                    h.SpotInfo.StateRecOfst = this.PeekSInt16();
                    h.SpotInfo.NameOfst = this.PeekSInt16();
                    h.SpotInfo.ScriptTextOfst = this.PeekSInt16();
                    this.PeekSInt16(); //alignReserved

                    if (h.SpotInfo.NameOfst > 0 && h.SpotInfo.NameOfst < this.Count)
                        h.Name = this.PeekPString(32, 1, h.SpotInfo.NameOfst);

                    if (h.SpotInfo.ScriptTextOfst > 0 && h.SpotInfo.ScriptTextOfst < this.Count)
                        h.Script = this.ReadCString(h.SpotInfo.ScriptTextOfst);

                    if (h.SpotInfo.NbrPts > 0 && h.SpotInfo.PtsOfst > 0 && h.SpotInfo.PtsOfst < this.Count - Exts.Palace.AttributeExts.GetByteSize<Types.Point?>() * h.SpotInfo.NbrPts)
                        for (var s = 0; s < h.SpotInfo.NbrPts; s++)
                        {
                            this.Seek(h.SpotInfo.PtsOfst + s * Exts.Palace.AttributeExts.GetByteSize<Types.Point?>());

                            vAxis = this.PeekSInt16();
                            hAxis = this.PeekSInt16();
                            var p = new Types.Point(vAxis, hAxis);

                            h.Vortexes.Add(p);
                        }

                    for (var s = 0; s < h.SpotInfo.NbrStates; s++)
                    {
                        this.Seek(h.SpotInfo.StateRecOfst + s * Exts.Palace.AttributeExts.GetByteSize<HotspotStateRec>());

                        var hs = new HotspotStateDesc();
                        hs.StateInfo.PictID = this.PeekSInt16();
                        this.PeekSInt16(); //reserved

                        vAxis = this.PeekSInt16();
                        hAxis = this.PeekSInt16();
                        hs.StateInfo.PicLoc = new Types.Point(vAxis, hAxis);

                        h.States.Add(hs);
                    }

                    this.HotSpots.Add(h);
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
                    this.Seek(pictureOfst + Exts.Palace.AttributeExts.GetByteSize<PictureRec>() * i);

                    var pict = new PictureRec();
                    pict.RefCon = this.PeekSInt32();
                    pict.PicID = this.PeekSInt16();
                    pict.PicNameOfst = this.PeekSInt16();
                    pict.TransColor = this.PeekSInt16();
                    this.PeekSInt16(); //reserved

                    if (pict.PicNameOfst > 0 &&
                        pict.PicNameOfst < this.Count)
                    {
                        pict.Name = this.PeekPString(32, 1, pict.PicNameOfst);

                        this.Pictures.Add(pict);
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
                    this.Seek(ofst);

                    var drawCmd = new DrawCmdDesc();
                    ofst = drawCmd.DrawCmdInfo.NextOfst = this.PeekSInt16();
                    this.PeekSInt16(); //reserved
                    drawCmd.DrawCmdInfo.DrawCmd = this.PeekSInt16();
                    drawCmd.DrawCmdInfo.CmdLength = this.PeekUInt16();
                    drawCmd.DrawCmdInfo.DataOfst = this.PeekSInt16();
                    drawCmd.Data = this.Data
                        .Skip(drawCmd.DrawCmdInfo.DataOfst)
                        .Take(drawCmd.DrawCmdInfo.CmdLength)
                        .ToArray();
                    //drawCmd.DeserializeData();

                    this.DrawCmds.Add(drawCmd);
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
                    this.Seek(ofst);

                    var prop = new LoosePropRec();
                    ofst = prop.NextOfst = this.PeekSInt16();
                    this.PeekSInt16(); //reserved

                    var id = this.PeekSInt32();
                    var crc = (uint)this.PeekSInt32();
                    prop.AssetSpec = new AssetSpec(id, crc);

                    prop.Flags = this.PeekSInt32();
                    this.PeekSInt32(); //refCon

                    var vAxis = this.PeekSInt16();
                    var hAxis = this.PeekSInt16();
                    prop.Loc = new Types.Point(hAxis, vAxis);

                    this.LooseProps.Add(prop);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"RoomRec.LooseProps: {ex.Message}");
            }

            #endregion

            return;
        }

        public void Serialize(ref int refNum, Stream writer, SerializerOptions opts = SerializerOptions.None)
        {
            using (var _data = new RawStream()) //RawData()
            using (var _blobData = new RawStream()) //RawData()
            {
                // ALIGN header
                _blobData.PadBytes(4);

                // Room Name
                var roomNameOfst = (short)_blobData.Count;
                _blobData.WritePString(this.Name ?? $"Room {this.RoomInfo.RoomID}", 32, 1);

                // Artist Name
                var artistNameOfst = (short)_blobData.Count;
                _blobData.WritePString(this.Artist ?? string.Empty, 32, 1);

                var pictNameOfst = (short)_blobData.Count;
                _blobData.WritePString(this.Picture ?? "clouds.gif", 32, 1);

                // Password
                var passwordOfst = (short)_blobData.Count;
                _blobData.WritePString(this.Password ?? string.Empty, 32, 1);

                //Start Spots
                var hotspotOfst = (short)0;

                using (var tmp = new MemoryStream())
                {
                    if ((this.HotSpots?.Count ?? 0) > 0)
                    {
                        foreach (var spot in this.HotSpots)
                        {
                            // Buffer spot scripts

                            if (!string.IsNullOrEmpty(spot.Script))
                            {
                                spot.SpotInfo.ScriptTextOfst = (short)_blobData.Count;
                                _blobData.WriteCString(spot.Script);
                            }
                            else
                                spot.SpotInfo.ScriptTextOfst = 0;

                            //Buffer spot states
                            spot.SpotInfo.NbrStates = (short)(spot.States?.Count ?? 0);

                            if (spot.SpotInfo.NbrStates > 0)
                            {
                                spot.SpotInfo.StateRecOfst = (short)(spot.SpotInfo.NbrStates > 0 ? _blobData.Count : 0);

                                using (var ms = new MemoryStream())
                                {
                                    foreach (var state in spot.States)
                                    {
                                        state.Serialize(ref refNum, ms, opts);

                                        //_blobData.WriteInt16(state.PictID);
                                        //_blobData.WriteInt16(0); //reserved
                                        //_blobData.WriteBytes(MessagePackSerializer.Serialize(state.PicLoc));
                                    }

                                    _blobData.WriteBytes(ms.ToArray());
                                }
                            }
                            else
                                spot.SpotInfo.StateRecOfst = 0;

                            spot.SpotInfo.PtsOfst = 0;

                            if ((spot.Vortexes?.Count ?? 0) > 0)
                            {
                                spot.SpotInfo.PtsOfst = (short)_blobData.Count;

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
                                spot.SpotInfo.PtsOfst = 0;

                            if (!string.IsNullOrEmpty(spot.Name))
                            {
                                spot.SpotInfo.NameOfst = (short)_blobData.Count;
                                _blobData.WritePString(spot.Name, 32, 1);
                            }
                            else
                                spot.SpotInfo.NameOfst = 0;

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
                    }

                    _blobData.PadBytes(4);

                    hotspotOfst = (short)(this.HotSpots.Count > 0 ? _blobData.Count : 0);

                    _blobData.WriteBytes(tmp.ToArray());
                }

                //Start Pictures
                var pictureOfst = (short)0;

                using (var tmp = new MemoryStream())
                {
                    if ((this.Pictures?.Count ?? 0) > 0)
                    {
                        foreach (var pict in this.Pictures)
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

                    pictureOfst = (short)(this.Pictures.Count > 0 ? _blobData.Count : 0);

                    _blobData.WriteBytes(tmp.ToArray());
                }


                // Start DrawCmds
                var firstDrawCmd = (short)0;

                using (var tmp1 = new MemoryStream())
                {
                    _blobData.PadBytes(4);

                    firstDrawCmd = (short)((this.DrawCmds?.Count ?? 0) > 0 ? _blobData.Count : 0);

                    using (var tmp2 = new MemoryStream())
                    {
                        for (var i = 0; i < (this.DrawCmds?.Count ?? 0); i++)
                        {
                            this.DrawCmds[i].DrawCmdInfo.CmdLength = (ushort)this.DrawCmds[i].Data.Length;
                            this.DrawCmds[i].DrawCmdInfo.DataOfst = (short)(firstDrawCmd + tmp2.Length + Exts.Palace.AttributeExts.GetByteSize<DrawCmdRec>() * this.DrawCmds.Count);
                            this.DrawCmds[i].DrawCmdInfo.NextOfst = (short)(i == this.DrawCmds.Count - 1 ? 0 : firstDrawCmd + tmp1.Length + Exts.Palace.AttributeExts.GetByteSize<DrawCmdRec>());

                            tmp1.WriteInt16(this.DrawCmds[i].DrawCmdInfo.NextOfst);
                            tmp1.WriteInt16(0); //reserved
                            tmp1.WriteInt16(this.DrawCmds[i].DrawCmdInfo.DrawCmd);
                            tmp1.WriteUInt16(this.DrawCmds[i].DrawCmdInfo.CmdLength);
                            tmp1.WriteInt16(this.DrawCmds[i].DrawCmdInfo.DataOfst);
                            tmp2.Write(this.DrawCmds[i].Data);
                        }

                        tmp1.Write(tmp2.ToArray());
                    }

                    _blobData.WriteBytes(tmp1.ToArray());
                }

                // Start Loose Props
                var firstLProp = (short)((this.LooseProps?.Count ?? 0) > 0 ? _blobData.Count : 0);

                for (var i = 0; i < (this.LooseProps?.Count ?? 0); i++)
                {
                    this.LooseProps[i].NextOfst = (short)(i == this.LooseProps.Count - 1 ? 0 : firstLProp + (i + 1) * Exts.Palace.AttributeExts.GetByteSize<LoosePropRec>());

                    using (var ms = new MemoryStream())
                    {
                        this.LooseProps[i].Serialize(ref refNum, ms);

                        _blobData.WriteBytes(ms.ToArray());
                    }
                }

                // Write Map Header
                {
                    var lenVars = (short)_blobData.Count;

                    this.WriteInt32((int)this.RoomInfo.RoomFlags);                // Room Flags
                    this.WriteInt32(this.RoomInfo.FacesID);                  // Default Face ID
                    this.WriteInt16(this.RoomInfo.RoomID);                   // The Rooms ID
                    this.WriteInt16(roomNameOfst);      // Room Name
                    this.WriteInt16(pictNameOfst);      // Background Image Offset
                    this.WriteInt16(artistNameOfst);    // Artist
                    this.WriteInt16(passwordOfst);      // Password
                    this.WriteInt16((short)this.HotSpots.Count);    // Number of Hotspots
                    this.WriteInt16(hotspotOfst);       // Hotspot Offset
                    this.WriteInt16((short)this.HotSpots.Count);    // Number of Pictures
                    this.WriteInt16(pictureOfst);       // Pictures Offset
                    this.WriteInt16((short)this.HotSpots.Count);    // Number of Draw Commands
                    this.WriteInt16(firstDrawCmd);      // Draw Command Offset
                    this.WriteInt16(0);                        // Number of People ( Obsolete )
                    this.WriteInt16((short)this.LooseProps.Count);  // Number of Props
                    this.WriteInt16(firstLProp);        // Loose Props Offset
                    this.WriteInt16(0);                        // Reserved Padding
                    this.WriteInt16(lenVars);                  // Length of Data Blob
                }

                this._stream.Write(_blobData.Stream.ToArray());

                writer.Write(this.Stream.ToArray());
            }
        }
    }
}