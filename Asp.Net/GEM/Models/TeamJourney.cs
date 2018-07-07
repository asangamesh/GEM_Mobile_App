using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GEM.Models
{
    public class TeamJourney
    {
        public List<JourneyInformation> JourneyList { get; set; }
        public List<Team_Journey> Teams { get; set; }
        public string InstanceTeamName { get; set; }
    }
}