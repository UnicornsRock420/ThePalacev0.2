using Lib.Common.Client.Entities.Business.ServerInfo;
using Lib.Common.Helpers;
using Lib.Core.Entities.EventsBus.EventArgs;
using Lib.Core.Factories.Core;
using Lib.Core.Interfaces.EventsBus;

namespace ThePalace.Testing;

[TestClass]
public class TestEventBus
{
    private static readonly Type CONST_TYPE_IEventHandler = typeof(IEventHandler);
    private static readonly EventBus CONST_EventBus = EventBus.Current;

    [TestInitialize]
    public void TestInitialize()
    {
        BO_LISTOFALLROOMS? test = null;

        var types = AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(a => { return a.FullName?.Contains("Lib.Common.Server") == true; })
            .SelectMany(a => a.GetTypes())
            .Where(t =>
            {
                return
                    t.GetInterfaces().Contains(typeof(IEventHandler)) &&
                    t.Namespace?.StartsWith("Lib.Common.Server.Entities.Business") == true;
            })
            .ToArray();

        CONST_EventBus.Subscribe(types);
    }

    [TestMethod]
    public void MSG_LISTOFALLROOMS()
    {
        var srcMsg = TestIStruct.MSG_LISTOFALLROOMS;

        var boType = CONST_EventBus.GetType(srcMsg);
        CONST_EventBus.Publish(
            null,
            boType,
            new ProtocolEventParams
            {
                SourceID = 123,
                RefNum = RndGenerator.Next(1337),
                Request = srcMsg
            });

        Assert.IsNotNull(boType);
    }

    [TestMethod]
    public void MSG_LOGON()
    {
        var srcMsg = TestIStruct.MSG_LOGON;

        var boType = CONST_EventBus.GetType(srcMsg);
        CONST_EventBus.Publish(
            null,
            boType,
            new ProtocolEventParams
            {
                SourceID = 123,
                RefNum = RndGenerator.Next(1337),
                Request = srcMsg
            });

        Assert.IsNotNull(boType);
    }

    [TestMethod]
    public void MSG_USERDESC()
    {
        var srcMsg = TestIStruct.MSG_USERDESC;

        var boType = CONST_EventBus.GetType(srcMsg);
        CONST_EventBus.Publish(
            null,
            boType,
            new ProtocolEventParams
            {
                SourceID = 123,
                RefNum = RndGenerator.Next(1337),
                Request = srcMsg
            });

        Assert.IsNotNull(boType);
    }
}