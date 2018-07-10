using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GEM.Models
{
    public class measure
    {
        public int MeasureId { get; set; }
        public int PracticeId { get; set; }
        public string Measure1 { get; set; }
        public string Description { get; set; }
        public string Why { get; set; }
        public string When { get; set; }
    }
}