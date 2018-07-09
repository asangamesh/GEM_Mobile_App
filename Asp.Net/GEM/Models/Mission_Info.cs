using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gem.BusinessEntities;

namespace GEM.Models
{
    public class Mission_Info
    {
        public List<Practice> practiceList { get; set; }
        public Team_Journey teams { get; set; }
        public int journeyid { get; set; }
        public string fluencyname { get; set; }
        public int teamjourneyid { get; set; }
    }
}