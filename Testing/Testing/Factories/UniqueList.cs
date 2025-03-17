using System.Collections;

namespace ThePalace.Testing.Factories;

[TestClass]
public class UniqueList
{
    private const int CONST_INT_MaxCapacity = 3;

    [TestInitialize]
    public void TestInitialize()
    {
    }

    [TestMethod]
    public void UniqueList_MaxCapacity()
    {
        var list = new UniqueList<string>(CONST_INT_MaxCapacity);
        list.Add("1");
        list.Add("2");
        list.Add("3");
        list.Add("4");
        list.Add("5");
        list.Add("1");

        Assert.AreEqual(CONST_INT_MaxCapacity, list.Count);
        Assert.AreEqual("1", list[0]);
    }

    [TestMethod]
    public void UniqueList_Positioning()
    {
        var list = new UniqueList<string>();
        list.Add("1");
        list.Add("2");
        list.Add("1");

        Assert.AreEqual(2, list.Count);
        Assert.AreEqual("1", list[0]);
        
        list.Add("2");
        list.Add("1");

        Assert.AreEqual(2, list.Count);
        Assert.AreEqual("1", list[0]);
    }

    [TestMethod]
    public void UniqueList_CapacityAndPositioning()
    {
        var list = new UniqueList<string>(CONST_INT_MaxCapacity);
        list.Add("1");
        list.Add("2");
        list.Add("3");
        list.Add("4");
        list.Add("5");
        list.Add("2");
        list.Add("1");
        list.Add("2");

        Assert.AreEqual(CONST_INT_MaxCapacity, list.Count);
        Assert.AreEqual("2", list[0]);
        Assert.AreEqual("1", list[1]);
    }
}