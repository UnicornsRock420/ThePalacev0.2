using ThePalace.Common.Client.Entities.Business.Server.ServerInfo;
using ThePalace.Common.Helpers;
using ThePalace.Core.Entities.EventsBus.EventArgs;
using ThePalace.Core.Factories.Core;
using ThePalace.Core.Interfaces.EventsBus;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Testing;

[TestClass]
public partial class TestEventBus
{
    private static readonly Type CONST_TYPE_IEventHandler = typeof(IEventHandler);
    private static readonly EventBus CONST_EventBus = EventBus.Instance;

    public Type? GetBOType(IProtocol msg)
    {
        var msgType = msg.GetType();

        return AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(t => t.GetTypes())
            .Where(t =>
            {
                if (t.IsInterface) return false;

                var itrfs = t.GetInterfaces();

                if (!itrfs.Contains(CONST_TYPE_IEventHandler)) return false;

                if (!itrfs.Any(i => i.IsGenericType && i.GetGenericArguments().Contains(msgType))) return false;

                return true;
            })
            .Select(t =>
            {
                foreach (var i in t.GetInterfaces() ?? [])
                    if (i == CONST_TYPE_IEventHandler)
                        return t;

                return null;
            })
            .FirstOrDefault();
    }

    [TestInitialize]
    public void TestInitialize()
    {
        BO_LISTOFALLROOMS? test = null;

        var types = AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(a =>
            {
                return a.FullName?.Contains("ThePalace.Common.Server") == true;
            })
            .SelectMany(a => a.GetTypes())
            .Where(t =>
            {
                return
                    t.GetInterfaces().Contains(typeof(IEventHandler)) &&
                    t.Namespace?.StartsWith("ThePalace.Common.Server.Entities.Business") == true;
            })
            .ToList();

        var eventBus = EventBus.Instance;
        foreach (var type in types)
        {
            eventBus.Subscribe(type);
        }
    }

    [TestMethod]
    public void MSG_LISTOFALLROOMS()
    {
        var srcMsg = TestIStruct.MSG_LISTOFALLROOMS;
        var srcMsgType = (Type?)TestIStruct.MSG_LISTOFALLROOMS.GetType();

        var boType = GetBOType(srcMsg);
        CONST_EventBus.Publish(
            null,
            boType,
            new ProtocolEventParams
            {
                SourceID = 123,
                RefNum = RndGenerator.Next(1337),
                Request = srcMsg,
            });

        Assert.IsNotNull(boType);
    }

    [TestMethod]
    public void MSG_LOGON()
    {
        var srcMsg = TestIStruct.MSG_LOGON;
        var srcMsgType = (Type?)TestIStruct.MSG_LOGON.GetType();

        var boType = GetBOType(srcMsg);
        CONST_EventBus.Publish(
            null,
            boType,
            new ProtocolEventParams
            {
                SourceID = 123,
                RefNum = RndGenerator.Next(1337),
                Request = srcMsg,
            });

        Assert.IsNotNull(boType);
    }

    [TestMethod]
    public void MSG_USERDESC()
    {
        var srcMsg = TestIStruct.MSG_USERDESC;
        var srcMsgType = (Type?)TestIStruct.MSG_USERDESC.GetType();

        var boType = GetBOType(srcMsg);
        CONST_EventBus.Publish(
            null,
            boType,
            new ProtocolEventParams
            {
                SourceID = 123,
                RefNum = RndGenerator.Next(1337),
                Request = srcMsg,
            });

        Assert.IsNotNull(boType);
    }
}