using System;
using ThePalace.Common.Helpers;
using ThePalace.Core.Entities.Network.Client.Network;
using ThePalace.Core.Entities.Network.Server.ServerInfo;
using ThePalace.Core.Entities.Network.Shared.Network;
using ThePalace.Core.Entities.Network.Shared.Users;
using ThePalace.Core.Entities.Shared.Types;
using ThePalace.Core.Enums;
using ThePalace.Core.Exts;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Testing;

[TestClass]
public class TestSerialization
{
    private static readonly Type CONST_TYPE_IProtocolS2C = typeof(IProtocolS2C);
    private static readonly Type CONST_TYPE_AssetSpec = typeof(AssetSpec);
    private static readonly Type CONST_TYPE_MSG_Header = typeof(MSG_Header);

    [TestInitialize]
    public void TestInitialize()
    {
    }

    [TestMethod]
    public void MSG_LISTOFALLROOMS()
    {
        var packetBytes = (byte[]?)null;

        var srcMsg = TestIStruct.MSG_LISTOFALLROOMS;

        var dstHdr = new MSG_Header();
        var dstMsg = (IProtocol?)null;
        var dstMsgType = (Type?)null;

        using (var ms = new MemoryStream())
        {
            ms.PalaceSerialize(
                0,
                srcMsg,
                opts: SerializerOptions.IncludeHeader);

            packetBytes = ms.ToArray();

            ms.Seek(0, SeekOrigin.Begin);

            ms.PalaceDeserialize(dstHdr, CONST_TYPE_MSG_Header);

            if (ms.Length - ms.Position != dstHdr.Length) throw new InvalidDataException(nameof(dstHdr));

            var eventType = dstHdr.EventType.ToString();
            dstMsgType = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a => a.FullName.Contains("ThePalace"))
                .SelectMany(t => t.GetTypes())
                .Where(t =>
                    t.GetInterfaces().Contains(CONST_TYPE_IProtocolS2C) &&
                    t.Name == eventType)
                .FirstOrDefault();
            if (dstMsgType != null)
            {
                dstMsg = (IProtocol?)dstMsgType.GetInstance();

                ms.PalaceDeserialize(
                    dstMsg,
                    dstMsgType);
            }
        }

        Assert.IsNotNull(dstMsg as MSG_LISTOFALLROOMS);

        Assert.IsTrue(packetBytes.Length == 52);

        if (dstMsg is MSG_LISTOFALLROOMS _msg)
        {
            Assert.IsTrue(_msg.Rooms.Count == dstHdr.RefNum);
            Assert.IsTrue(_msg.Rooms.Count == srcMsg.Rooms.Count);
            Assert.IsTrue(_msg.Rooms[0].PrimaryID == srcMsg.Rooms[0].PrimaryID);
            Assert.IsTrue(_msg.Rooms[0].Flags == srcMsg.Rooms[0].Flags);
            Assert.IsTrue(_msg.Rooms[0].RefNum == srcMsg.Rooms[0].RefNum);
            Assert.IsTrue(_msg.Rooms[1].PrimaryID == srcMsg.Rooms[1].PrimaryID);
            Assert.IsTrue(_msg.Rooms[1].Flags == srcMsg.Rooms[1].Flags);
            Assert.IsTrue(_msg.Rooms[1].RefNum == srcMsg.Rooms[1].RefNum);
        }
    }

    [TestMethod]
    public void MSG_LOGON()
    {
        var packetBytes = (byte[]?)null;

        var refNum = RndGenerator.Next(1337);
        var srcMsg = TestIStruct.MSG_LOGON;

        var dstHdr = new MSG_Header();
        var dstMsg = (IProtocol?)null;
        var dstMsgType = (Type?)null;

        using (var ms = new MemoryStream())
        {
            ms.PalaceSerialize(
                refNum,
                srcMsg,
                opts: SerializerOptions.IncludeHeader);

            packetBytes = ms.ToArray();

            ms.Seek(0, SeekOrigin.Begin);

            ms.PalaceDeserialize(dstHdr, CONST_TYPE_MSG_Header);

            if (ms.Length - ms.Position != dstHdr.Length) throw new InvalidDataException(nameof(dstHdr));

            var eventType = dstHdr.EventType.ToString();
            dstMsgType = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a => a.FullName.Contains("ThePalace"))
                .SelectMany(t => t.GetTypes())
                .Where(t => t.Name == eventType)
                .FirstOrDefault();
            if (dstMsgType != null)
            {
                dstMsg = (IProtocol?)dstMsgType.GetInstance();

                ms.PalaceDeserialize(
                    dstMsg,
                    dstMsgType);
            }
        }

        Assert.IsTrue(dstHdr.RefNum == refNum);

        Assert.IsNotNull(dstMsg as MSG_LOGON);

        Assert.IsTrue(packetBytes.Length == CONST_TYPE_MSG_Header.GetByteSize() + dstMsgType.GetByteSize());

        if (dstMsg is MSG_LOGON _msg)
        {
            Assert.IsTrue(_msg.RegInfo.UserName == srcMsg.RegInfo.UserName);
            Assert.IsTrue(_msg.RegInfo.Reserved == srcMsg.RegInfo.Reserved);
            Assert.IsTrue(_msg.RegInfo.UlUploadCaps == srcMsg.RegInfo.UlUploadCaps);
            Assert.IsTrue(_msg.RegInfo.UlDownloadCaps == srcMsg.RegInfo.UlDownloadCaps);
            Assert.IsTrue(_msg.RegInfo.Ul2DEngineCaps == srcMsg.RegInfo.Ul2DEngineCaps);
            Assert.IsTrue(_msg.RegInfo.Ul2DGraphicsCaps == srcMsg.RegInfo.Ul2DGraphicsCaps);
        }
    }

    [TestMethod]
    public void MSG_USERDESC()
    {
        var packetBytes = (byte[]?)null;

        var refNum = RndGenerator.Next(1337);
        var srcMsg = TestIStruct.MSG_USERDESC;

        var dstHdr = new MSG_Header();
        var dstMsg = (IProtocol?)null;
        var dstMsgType = (Type?)null;

        using (var ms = new MemoryStream())
        {
            ms.PalaceSerialize(
                refNum,
                srcMsg,
                opts: SerializerOptions.IncludeHeader);

            packetBytes = ms.ToArray();

            ms.Seek(0, SeekOrigin.Begin);

            ms.PalaceDeserialize(dstHdr, CONST_TYPE_MSG_Header);

            if (ms.Length - ms.Position != dstHdr.Length) throw new InvalidDataException(nameof(dstHdr));

            var eventType = dstHdr.EventType.ToString();
            dstMsgType = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a => a.FullName.Contains("ThePalace"))
                .SelectMany(t => t.GetTypes())
                .Where(t => t.Name == eventType)
                .FirstOrDefault();
            if (dstMsgType != null)
            {
                dstMsg = (IProtocol?)dstMsgType.GetInstance();

                ms.PalaceDeserialize(
                    dstMsg,
                    dstMsgType);
            }
        }

        Assert.IsTrue(dstHdr.RefNum == refNum);

        Assert.IsNotNull(dstMsg as MSG_USERDESC);

        if (dstMsg is MSG_USERDESC _msg)
        {
            Assert.IsTrue(packetBytes.Length == CONST_TYPE_MSG_Header.GetByteSize() + 8 +
                CONST_TYPE_AssetSpec.GetByteSize() * _msg.NbrProps);

            Assert.IsTrue(_msg.ColorNbr == srcMsg.ColorNbr);
            Assert.IsTrue(_msg.FaceNbr == srcMsg.FaceNbr);
            Assert.IsTrue(_msg.PropSpec.Length == srcMsg.PropSpec.Length);
            Assert.IsTrue(_msg.PropSpec[0].Id == srcMsg.PropSpec[0].Id);
            Assert.IsTrue(_msg.PropSpec[1].Id == srcMsg.PropSpec[1].Id);
            Assert.IsTrue(_msg.PropSpec[2].Id == srcMsg.PropSpec[2].Id);
        }
    }
}