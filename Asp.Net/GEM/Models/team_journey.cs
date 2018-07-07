using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GEM.Models
{
    public class Team_Journey
    {
        public int TeamJourneyId { get; set; }
        public Nullable<int> TeamId { get; set; }
        public Nullable<int> JourneyId { get; set; }

        public team team { get; set; }
        public List<team_journey_member> team_journey_member { get; set; }
        public List<mission> missions { get; set; }
    }
}