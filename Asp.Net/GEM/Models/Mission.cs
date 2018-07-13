using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GEM.Models
{
    public class mission
    {
        public int MissionId { get; set; }
        public int TeamJourneyId { get; set; }
        public string Name { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public virtual List<mission_practice> MissionPractice { get; set; }
        public virtual team Team { get; set; }
        public virtual JourneyInformation Journey { get; set; }
    }
}