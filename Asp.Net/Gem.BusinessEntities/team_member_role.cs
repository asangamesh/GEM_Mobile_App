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
    
    public partial class team_member_role
    {
        public team_member_role()
        {
            this.team_member = new HashSet<team_member>();
        }
    
        public int TeamMemberRoleId { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<team_member> team_member { get; set; }
    }
}
