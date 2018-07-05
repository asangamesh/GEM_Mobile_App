using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gem.BusinessEntities;

namespace GEM.Models
{
    public class Mission_info
    {
        public List<Practice> PracticeList { get; set; }
        public team Teams { get; set; }
        public int JourneyId { get; set; }
        public string fluencyName { get; set; }
    }
}