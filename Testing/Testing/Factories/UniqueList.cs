using System.Collections;

namespace ThePalace.Testing.Factories;

[TestClass]
public class UniqueList
{
    [TestInitialize]
    public void TestInitialize()
    {
    }

    [TestMethod]
    public void UniqueList_MaxCapacity()
    {
        var maxCapacity = 5;
        var list = new UniqueList<string>(maxCapacity);
        list.Add("1");
        list.Add("2");
        list.Add("3");
        list.Add("4");
        list.Add("5");
        list.Add("1");
        
        Assert.AreEqual(maxCapacity, list.Count);
        Assert.AreEqual("1", list[0]);
    }

    [TestMethod]
    public void UniqueList_Positiioning()
    {
        var list = new UniqueList<string>();
        list.Add("1");
        list.Add("2");
        list.Add("1");
        
        Assert.AreEqual(2, list.Count);
        Assert.AreEqual("1", list[0]);
    }
}