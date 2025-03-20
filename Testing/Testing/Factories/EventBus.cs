using Lib.Common.Client.Entities.Business.ServerInfo;
using Lib.Common.Helpers;
using Lib.Common.Server.Entities.Business.Auth;
using Lib.Common.Server.Entities.Business.Users;
using Lib.Core.Entities.EventsBus.EventArgs;
using Lib.Core.Interfaces.EventsBus;
using ThePalace.Testing.Data;

namespace ThePalace.Testing.Factories;

[TestClass]
public class EventBus
{
    private static readonly Type CONST_TYPE_IEventHandler = typeof(IEventHandler);
    private static readonly Lib.Core.Singletons.EventBus CONST_EventBus = Lib.Core.Singletons.EventBus.Current;

    [TestInitialize]
    public void TestInitialize()
    {
        var test = new Type[]
        {
            typeof(BO_LISTOFALLROOMS),
            typeof(BO_LOGON),
            typeof(BO_USERDESC),
        };

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
        var srcMsg = IStruct.MSG_LISTOFALLROOMS;

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
        var srcMsg = IStruct.MSG_LOGON;

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
        var srcMsg = IStruct.MSG_USERDESC;

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