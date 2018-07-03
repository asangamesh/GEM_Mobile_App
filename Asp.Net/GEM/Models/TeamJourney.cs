using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gem.BusinessEntities;

namespace GEM.Models
{
    public class TeamJourney
    {
        public List<JourneyInformation> JourneyList { get; set; }
        public journey Journey { get; set; }
    }
}