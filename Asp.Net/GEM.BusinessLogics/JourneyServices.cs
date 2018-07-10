using Gem.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;

namespace GEM.BusinessLogics
{
    public class JourneyServices
    {
        gemEntities1 gemdb = new gemEntities1();

        public List<journey> GetJourney()
        {
            gemdb = new gemEntities1();

            var objJourneys = (from j in gemdb.journeys select j).ToList();

            return objJourneys;
        }

        public journey GetJourney(int JourneyId)
        {
            gemdb = new gemEntities1();

            var objJourney = (from j in gemdb.journeys where j.JourneyId == JourneyId select j).FirstOrDefault();

            return objJourney;
        }

        public journey GetJourney(int JourneyId, int memberId)
        {
            gemdb = new gemEntities1();

            var objJourney = (from j in gemdb.journeys join tj in gemdb.team_journey on j.JourneyId equals tj.JourneyId join tm in gemdb.team_journey_member on tj.TeamJourneyId equals tm.TeamJourneyId where j.JourneyId == JourneyId && tm.MemberId == memberId select j).FirstOrDefault();

            return objJourney;
        }

        public List<journey> GetJourneybyTeamId(int TeamId)
        {
            gemdb = new gemEntities1();

            var objJourneys = (from j in gemdb.journeys join tj in gemdb.team_journey on j.JourneyId equals tj.JourneyId where tj.TeamId == TeamId select j).ToList();

            return objJourneys;
        }

        public List<journey> GetJourneybyUserId(int MemberId)
        {
            gemdb = new gemEntities1();

            var objJourneys = (from j in gemdb.journeys join tj in gemdb.team_journey on j.JourneyId equals tj.JourneyId join tm in gemdb.team_journey_member on tj.TeamJourneyId equals tm.TeamJourneyId where tm.MemberId == MemberId select j).ToList();

            return objJourneys;
        }

        public List<team_journey> GetTeams(int journeyId, int memberId)
        {
            gemdb = new gemEntities1();

            var objJourneys = (from tj in gemdb.team_journey join tm in gemdb.team_journey_member on tj.TeamJourneyId equals tm.TeamJourneyId where tj.JourneyId == journeyId && tm.MemberId == memberId select tj).ToList();

            return objJourneys;
        }

        public List<team_journey> GetTeamMission(int memberId)
        {
            gemdb = new gemEntities1();

            var objJourneys = (from tj in gemdb.team_journey join tjm in gemdb.team_journey_member on tj.TeamJourneyId equals tjm.TeamJourneyId
                               join m in gemdb.missions on tj.TeamJourneyId equals m.TeamJourneyId 
                               where tjm.MemberId == memberId select tj).ToList();

            return objJourneys;
        }

        public team_journey GetTeamJourney(int TeamId, int JourneyId)
        {
            gemdb = new gemEntities1();

            var objJourneys = (from tj in gemdb.team_journey where tj.TeamId == TeamId && tj.JourneyId == JourneyId select tj).FirstOrDefault();

            return objJourneys;
        }

        public List<team_journey_member> GetTeamJourneyMember(int teamJourneyId)
        {
            gemdb = new gemEntities1();

            var objJourneys = (from tjm in gemdb.team_journey_member join tj in gemdb.team_journey on tjm.TeamJourneyId equals tj.TeamJourneyId where tj.TeamJourneyId == teamJourneyId select tjm).ToList();

            return objJourneys;
        }

        public int AddorUpdateJourney(journey journey)
        {
            gemdb = new gemEntities1();
            gemdb.journeys.AddOrUpdate(journey);
            return gemdb.SaveChanges();
        }

        public int AddorUpdatetTeamJourney(team_journey team_journey)
        {
            gemdb = new gemEntities1();
            gemdb.team_journey.AddOrUpdate(team_journey);
            return gemdb.SaveChanges();
        }

        public int AddorUpdatetteamjourneymember(team_journey_member team_journey_member)
        {
            gemdb = new gemEntities1();
            gemdb.team_journey_member.AddOrUpdate(team_journey_member);
            return gemdb.SaveChanges();
        }

        public mission GetTeamPractice(int teamJourneyId)
        {
            gemdb = new gemEntities1();

            var objMission = (from p in gemdb.practices join mp in gemdb.mission_practice on p.PracticeId equals mp.PracticeId join m in gemdb.missions on mp.MissionId equals m.MissionId join tj in gemdb.team_journey on m.TeamJourneyId equals tj.TeamJourneyId where tj.TeamJourneyId == teamJourneyId select m).FirstOrDefault();

            return objMission;
        }
    }
}
