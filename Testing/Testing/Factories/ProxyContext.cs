using Lib.Common.Entities.EventArgs;

namespace ThePalace.Testing.Factories;

[TestClass]
public class ProxyContext
{
    public class Test123
    {
        public Test123()
        {
        }

        public Test123(int field)
        {
            IntField = field;
        }

        public int IntField;

        public int IntProp
        {
            get => IntField;
            set => IntField = value;
        }
    }

    public void HookEvents(object s, EventArgs e)
    {
        switch (e)
        {
            case MemberChangedEventArgs mc:
                break;
            case MemberAccessedEventArgs ma:
                break;
            case MethodInvokedEventArgs mi:
                break;
        }
    }

    public void HookExceptions(object s, EventArgs e)
    {
        if (e is not ExceptionEventArgs ea) return;

        // TODO
    }

    [TestInitialize]
    public void TestInitialize()
    {
        Lib.Common.Factories.Core.ProxyContext.HookEvents += HookEvents;
        Lib.Common.Factories.Core.ProxyContext.HookExceptions += HookExceptions;
    }

    [TestMethod]
    public void ProxyContext_Create_Test123()
    {
        var test123 = Lib.Common.Factories.Core.ProxyContext.Create<Test123>();
        test123.IntField = 123;

        Assert.AreEqual("1", "1");
    }
}