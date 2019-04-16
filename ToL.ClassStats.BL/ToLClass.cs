using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ToL.ClassStats.PL;

namespace ToL.ClassStats.BL
{
    public class ToLClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Faction { get; set; }
        public int TimesPlayed { get; set; }
        public List<ToLClassData> ClassData { get; set; }

        public ToLClass()
        {
            ClassData = new List<ToLClassData>();
        }

        public void LoadById()
        {
            try
            {
                using (ToLClassStatsEntities oDc = new ToLClassStatsEntities())
                {
                    var tolClass = oDc.tblToLClasses.FirstOrDefault(tc => tc.Id == Id);
                    if (tolClass != null)
                    {
                        Id = tolClass.Id;
                        Name = tolClass.Name;
                        Faction = tolClass.Faction;
                        TimesPlayed = tolClass.TimesPlayed;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertDatePlayed(int classId, DateTime datePlayed)
        {
            try
            {
                using (ToLClassStatsEntities oDc = new ToLClassStatsEntities())
                {
                    tblToLClassDate toLClassDate = new tblToLClassDate();
                    toLClassDate.ClassId = classId;
                    toLClassDate.DatePlayed = datePlayed;
                    oDc.tblToLClassDates.Add(toLClassDate);
                    oDc.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Insert()
        {
            try
            {
                using (ToLClassStatsEntities oDc = new ToLClassStatsEntities())
                {
                    tblToLClass tolClass = new tblToLClass();
                    tolClass.Name = Name;
                    tolClass.Faction = Faction;
                    tolClass.TimesPlayed = TimesPlayed;
                    oDc.tblToLClasses.Add(tolClass);
                    oDc.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(int classId, DateTime datePlayed)
        {
            try
            {
                using (ToLClassStatsEntities oDc = new ToLClassStatsEntities())
                {
                    tblToLClass tolClass = oDc.tblToLClasses.FirstOrDefault(tc => tc.Id == Id);
                    tolClass.Name = Name;
                    tolClass.Faction = Faction;
                    tolClass.TimesPlayed = TimesPlayed;
                    oDc.SaveChanges();
                }
                InsertDatePlayed(classId, datePlayed);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete()
        {
            try
            {
                using (ToLClassStatsEntities oDc = new ToLClassStatsEntities())
                {
                    tblToLClass tolClass = oDc.tblToLClasses.FirstOrDefault(tc => tc.Id == Id);
                    oDc.tblToLClasses.Remove(tolClass);
                    oDc.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void LoadDatesPlayed()
        {
            try
            {
                using (ToLClassStatsEntities oDc = new ToLClassStatsEntities())
                {
                    var datesPlayed = (from cd in oDc.tblToLClassDates
                                       where cd.ClassId == Id
                                       orderby cd.Id descending
                                       select cd).ToList();

                    DateTimeFormatInfo info = new DateTimeFormatInfo();

                    foreach (var dp in datesPlayed)
                    {
                        ToLClassData tempData = new ToLClassData();
                        tempData.Month = info.GetMonthName(dp.DatePlayed.Month);
                        tempData.Day = dp.DatePlayed.Day;
                        tempData.Year = dp.DatePlayed.Year;
                        tempData.GameNumber = dp.Id;
                        ClassData.Add(tempData);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class ToLClassList : List<ToLClass>
    {
        public void Load()
        {
            try
            {
                using (ToLClassStatsEntities oDc = new ToLClassStatsEntities())
                {
                    var tolClasses = (from tc in oDc.tblToLClasses
                                      select tc).ToList();

                    foreach (var tc in tolClasses)
                    {
                        ToLClass tempClass = new ToLClass();
                        tempClass.Id = tc.Id;
                        tempClass.Name = tc.Name;
                        tempClass.Faction = tc.Faction;
                        tempClass.TimesPlayed = tc.TimesPlayed;
                        Add(tempClass);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class ToLClassData
    {
        public string Month { get; set; }
        public int Day { get; set; }
        public int Year { get; set; }
        public int GameNumber { get; set; }
    }

    public class Match
    {
        public int GameNumber { get; set; }
        public string ClassName { get; set; }
        public string DatePlayed { get; set; }
    }

    public class Matches : List<Match>
    {
        public void Load()
        {
            try
            {
                using (ToLClassStatsEntities oDc = new ToLClassStatsEntities())
                {
                    var matchHistory = from cd in oDc.tblToLClassDates
                                       join tc in oDc.tblToLClasses on cd.ClassId equals tc.Id
                                       orderby cd.Id descending
                                       select new
                                       {
                                           cd.Id,
                                           tc.Name,
                                           cd.DatePlayed
                                       };

                    foreach (var temp in matchHistory)
                    {
                        Match tempMatch = new Match();
                        tempMatch.GameNumber = temp.Id;
                        tempMatch.ClassName = temp.Name;
                        tempMatch.DatePlayed = temp.DatePlayed.ToShortDateString();
                        Add(tempMatch);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
