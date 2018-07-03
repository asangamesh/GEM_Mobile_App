using Gem.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public List<team> GetTeam(int userid)
        {
            gemdb = new gemEntities1();

            var objteam = (from t in gemdb.teams join tm in gemdb.team_member on t.TeamId equals tm.TeamId where tm.MemberId == userid select t) .ToList();

            return objteam;
        }

        public List<team_member> GetTeamMembers(int TeamId)
        {
            gemdb = new gemEntities1();

            var objteamMember = (from tm in gemdb.team_member where tm.TeamId == TeamId select tm).ToList();

            return objteamMember;
        }

        public int AddorUpdateTeam(team team)
        {
            gemdb = new gemEntities1();
            gemdb.teams.Add(team);
            return gemdb.SaveChanges();
        }
    }
}
