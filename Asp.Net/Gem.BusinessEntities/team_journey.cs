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
    
    public partial class team_journey
    {
        public team_journey()
        {
            this.missions = new HashSet<mission>();
            this.team_journey_member = new HashSet<team_journey_member>();
            this.team_journey_practice = new HashSet<team_journey_practice>();
        }
    
        public int TeamJourneyId { get; set; }
        public Nullable<int> TeamId { get; set; }
        public Nullable<int> JourneyId { get; set; }
    
        public virtual journey journey { get; set; }
        public virtual ICollection<mission> missions { get; set; }
        public virtual team team { get; set; }
        public virtual ICollection<team_journey_member> team_journey_member { get; set; }
        public virtual ICollection<team_journey_practice> team_journey_practice { get; set; }
    }
}
