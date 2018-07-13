using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GEM.Models
{
    public class mission_member_measure_assesment
    {
        public int MissionAssesmentId { get; set; }
        public int MissionId { get; set; }
        public int MemberId { get; set; }
        public int MeasureId { get; set; }
        public int Assesment { get; set; }
    }
}