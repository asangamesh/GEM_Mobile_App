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
    
    public partial class journey
    {
        public journey()
        {
            this.journey_practice = new HashSet<journey_practice>();
            this.team_journey = new HashSet<team_journey>();
        }
    
        public int JourneyId { get; set; }
        public Nullable<int> TeamFocusId { get; set; }
        public Nullable<int> SelectJourneyId { get; set; }
        public Nullable<int> Status { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
    
        public virtual member member { get; set; }
        public virtual ICollection<journey_practice> journey_practice { get; set; }
        public virtual ICollection<team_journey> team_journey { get; set; }
        public virtual team_focus team_focus { get; set; }
    }
}
