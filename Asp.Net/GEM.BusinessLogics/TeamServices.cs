using Gem.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;

namespace GEM.BusinessLogics
{
    public class TeamServices
    {
        gemEntities1 gemdb = new gemEntities1();

        public List<team> GetTeam()
        {
            gemdb = new gemEntities1();

            var objteam = (from t in gemdb.teams select t).ToList();

            return objteam;
        }

        public List<team> GetTeambyName(int journeyId, int memberId)
        {
            gemdb = new gemEntities1();

            var objteam = (from t1 in gemdb.teams join tj in gemdb.team_journey on t1.TeamId equals tj.TeamId join tm in gemdb.team_journey_member on tj.TeamJourneyId equals tm.TeamJourneyId where tj.JourneyId == journeyId && tm.MemberId == memberId && t1.Name.ToLower().StartsWith("team") select t1).ToList();

            return objteam;
        }

        public team GetTeamById(int teamId)
        {
            gemdb = new gemEntities1();

            var objteam = (from t in gemdb.teams where t.TeamId == teamId select t).FirstOrDefault();

            return objteam;
        }

        public List<team> GetTeam(team team)
        {
            gemdb = new gemEntities1();

            var objteam = (from t in gemdb.teams where t.Name == team.Name && t.CreatedBy == team.CreatedBy orderby t.CreatedDate descending select t).ToList();

            return objteam;
        }

        public team_journey_member GetTeamJourneyMember(int tjMemberId)
        {
            gemdb = new gemEntities1();

            var objteamMember = (from tm in gemdb.team_journey_member where tm.TeamJourneyMemberId == tjMemberId select tm).FirstOrDefault();

            return objteamMember;
        }

        public int AddorUpdateTeam(team team)
        {
            gemdb = new gemEntities1();
            gemdb.teams.AddOrUpdate(team);
            return gemdb.SaveChanges();
        }

        public int DeleteTeamMember(team_journey_member teamMember)
        {
            gemdb = new gemEntities1();
            var teamjourneymember = from u in gemdb.team_journey_member where u.TeamJourneyMemberId.Equals(teamMember.TeamJourneyMemberId) select u;
            gemdb.team_journey_member.RemoveRange(teamjourneymember);
            return gemdb.SaveChanges();
        }
        public int AddorUpdateTeamMemberMeasure(mission_member_measure_assesment missionMemberMeasureAssesment)
        {
            gemdb = new gemEntities1();
            gemdb.mission_member_measure_assesment.AddOrUpdate(missionMemberMeasureAssesment);
            return gemdb.SaveChanges();
        }
    }
}
