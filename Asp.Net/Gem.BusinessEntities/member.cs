//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gem.BusinessEntities
{
    using System;
    using System.Collections.Generic;
    
    public partial class member
    {
        public member()
        {
            this.journeys = new HashSet<journey>();
            this.teams = new HashSet<team>();
            this.team_member = new HashSet<team_member>();
            this.mission_member_measure_assesment = new HashSet<mission_member_measure_assesment>();
            this.team_journey_member = new HashSet<team_journey_member>();
            this.teams1 = new HashSet<team>();
            this.team_member1 = new HashSet<team_member>();
        }
    
        public int MemberId { get; set; }
        public string EmailAddress { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
    
        public virtual ICollection<journey> journeys { get; set; }
        public virtual ICollection<team> teams { get; set; }
        public virtual ICollection<team_member> team_member { get; set; }
        public virtual ICollection<mission_member_measure_assesment> mission_member_measure_assesment { get; set; }
        public virtual ICollection<team_journey_member> team_journey_member { get; set; }
        public virtual ICollection<team> teams1 { get; set; }
        public virtual ICollection<team_member> team_member1 { get; set; }
    }
}
