using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ToL.ClassStats.PL.Test
{
    [TestClass]
    public class utToLClass
    {
        [TestMethod]
        public void LoadTest()
        {
            using (ToLClassStatsEntities oDc = new ToLClassStatsEntities())
            {
                int expected = 45;
                var tolC = (from tc in oDc.tblToLClasses
                            select tc).ToList();
                int actual = tolC.Count;
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void InsertTest()
        {
            using (ToLClassStatsEntities oDc = new ToLClassStatsEntities())
            {
                tblToLClass newRow = new tblToLClass();
                newRow.Name = "Test Class";
                newRow.Faction = "Test Faction";
                newRow.TimesPlayed = -1;
                oDc.tblToLClasses.Add(newRow);
                oDc.SaveChanges();
                tblToLClass tolC = (from tc in oDc.tblToLClasses
                                    where tc.Id == 46
                                    select tc).FirstOrDefault();
                Assert.IsNotNull(tolC);
            }
        }

        [TestMethod]
        public void UpdateTest()
        {
            using (ToLClassStatsEntities oDc = new ToLClassStatsEntities())
            {
                tblToLClass tolC = oDc.tblToLClasses.FirstOrDefault(tc => tc.Id == 46);
                tolC.Name = "Updated class";
                oDc.SaveChanges();
                tblToLClass updatedClass = oDc.tblToLClasses.FirstOrDefault(tc => tc.Id == 46);
                Assert.AreEqual(updatedClass.Name, tolC.Name);
            }
        }

        [TestMethod]
        public void DeleteTest()
        {
            using (ToLClassStatsEntities oDc = new ToLClassStatsEntities())
            {
                tblToLClass tolC = oDc.tblToLClasses.FirstOrDefault(tc => tc.Id == 46);
                oDc.tblToLClasses.Remove(tolC);
                oDc.SaveChanges();
                tblToLClass deletedClass = oDc.tblToLClasses.FirstOrDefault(tc => tc.Id == 46);
                Assert.IsNull(deletedClass);
            }
        }
    }
}
