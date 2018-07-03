using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GEM.Models
{
    public class ModelTeam
    {
        public int teamId { get; set; }
        public string teamName { get; set; }
        public string Desc { get; set; }
        public int createdby { get; set; }
        public DateTime createddate { get; set; }
        public int updatedby { get; set; }
        public DateTime updateddate { get; set; }
    }
}