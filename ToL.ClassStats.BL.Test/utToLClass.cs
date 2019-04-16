using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ToL.ClassStats.BL.Test
{
    [TestClass]
    public class utToLClass
    {
        [TestMethod]
        public void LoadTest()
        {
            ToLClassList tolClasss = new ToLClassList();
            tolClasss.Load();
            Assert.AreEqual(32, tolClasss.Count);
            tolClasss = null;
        }

        [TestMethod]
        public void InsertTest()
        {
            ToLClass tolClass = new ToLClass();
            tolClass.Name = "Test Class";
            tolClass.Faction = "Test Faction";
            tolClass.TimesPlayed = -1;
            tolClass.Insert();
            tolClass.LoadById();
            Assert.AreEqual("Test Class", tolClass.Name);
        }

        [TestMethod]
        public void LoadByIdTest()
        {
            ToLClass tolClass = new ToLClass();
            tolClass.Id = 47;
            tolClass.LoadById();
            Assert.AreEqual("Test Class", tolClass.Name);
        }

        [TestMethod]
        public void UpdateTest()
        {
            ToLClass tolClass = new ToLClass();
            tolClass.Id = 47;
            tolClass.LoadById();
            tolClass.Name = "Updated Class";
            tolClass.Update();
            tolClass.LoadById();
            Assert.AreEqual("Updated Class", tolClass.Name);
        }

        [TestMethod]
        public void DeleteTest()
        {
            ToLClassList tolClasssInitial = new ToLClassList();
            tolClasssInitial.Load();

            ToLClass tolClass = new ToLClass();
            tolClass.Id = 47;
            tolClass.LoadById();
            tolClass.Delete();
            tolClass.LoadById();

            ToLClassList tolClasssFinal = new ToLClassList();
            tolClasssFinal.Load();

            Assert.AreEqual(tolClasssFinal.Count, (tolClasssInitial.Count - 1));
        }
    }
}
