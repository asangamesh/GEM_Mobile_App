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
    
    public partial class team_journey_member
    {
        public int TeamJourneyMemberId { get; set; }
        public Nullable<int> TeamJourneyId { get; set; }
        public Nullable<int> MemberId { get; set; }
        public Nullable<int> TeamJourneyMemberRoleId { get; set; }
    
        public virtual member member { get; set; }
        public virtual team_journey team_journey { get; set; }
        public virtual team_journey_member_role team_journey_member_role { get; set; }
    }
}
