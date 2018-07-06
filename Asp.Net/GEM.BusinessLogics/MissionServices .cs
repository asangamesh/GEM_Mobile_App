using Gem.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;

namespace GEM.BusinessLogics
{
    public class MissionServices
    {
        gemEntities1 gemdb = new gemEntities1();

        public List<practice> Getpractice(int fluencyId)
        {
            gemdb = new gemEntities1();

            var objpractices = (from p in gemdb.practices
                                join f in gemdb.fluency_level on p.FluencyLevelId equals f.FluencyLevelId
                                where f.FluencyLevelId == fluencyId
                                select p).ToList();

            return objpractices;
        }

        public team_journey GetTeam_journey(int teamJourneyId)
        {
            gemdb = new gemEntities1();

            var objTeamJourneys = (from t in gemdb.team_journey where t.TeamJourneyId == teamJourneyId select t).FirstOrDefault();

            return objTeamJourneys;
        }

        public mission GetMission(int teamJourneyId)
        {
            gemdb = new gemEntities1();

            var objmission = (from m in gemdb.missions where m.TeamJourneyId == teamJourneyId select m).FirstOrDefault();

            return objmission;
        }

        public object GetMissionName(int teamJourneyId)
        {

            gemdb = new gemEntities1();

            var objmission = (from tj in gemdb.team_journey
                              join t in gemdb.teams on tj.TeamId equals t.TeamId
                              join j in gemdb.journeys on tj.JourneyId equals j.JourneyId
                              join tf in gemdb.team_focus on j.TeamFocusId equals tf.TeamFocusId
                              where tj.TeamJourneyId == teamJourneyId
                              select  new { Name = tf.Name, TeamName =t.Name }).FirstOrDefault();

            return objmission;
        }

        public fluency_level GetFluency()
        {
            gemdb = new gemEntities1();

            var objfluency_level = (from f in gemdb.fluency_level select f).FirstOrDefault();

            return objfluency_level;
        }

        public int AddorUpdateMission(mission mission)
        {
            gemdb = new gemEntities1();
            gemdb.missions.AddOrUpdate(mission);
            return gemdb.SaveChanges();
        }

        public int AddorUpdateteamjourneypractice(team_journey_practice team_journey_practice)
        {
            gemdb = new gemEntities1();
            gemdb.team_journey_practice.AddOrUpdate(team_journey_practice);
            return gemdb.SaveChanges();
        }

        public int AddorUpdatemissionpractice(mission_practice mission_practice)
        {
            gemdb = new gemEntities1();
            gemdb.mission_practice.AddOrUpdate(mission_practice);
            return gemdb.SaveChanges();
        }

    }
}
