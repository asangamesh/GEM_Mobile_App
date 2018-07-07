using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GEM.Models
{
    public class team_journey_member
    {
        public int TeamJourneyMemberId { get; set; }
        public Nullable<int> TeamJourneyId { get; set; }
        public Nullable<int> MemberId { get; set; }
        public Nullable<int> TeamJourneyMemberRoleId { get; set; }

        public member member { get; set; }
    }
}