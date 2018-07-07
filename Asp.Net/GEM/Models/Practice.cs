using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GEM.Models
{
    public class Practice
    {
        public int PracticeId { get; set; }
        public int FluencyLevelId { get; set; }
        public string Name { get; set; }
        public int SequenceNum { get; set; }
        public int PrerequisiteNum { get; set; }
        public virtual fluency_level fluency_level { get; set; }
    }
}