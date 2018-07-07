using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GEM.Models
{
    public class mission_practice
    {
        public int MissionPracticeId { get; set; }
        public int MissionId { get; set; }
        public int PracticeId { get; set; }

        public virtual mission mission { get; set; }
        public virtual Practice practice { get; set; }
    }
}