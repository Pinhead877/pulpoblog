//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ButiqueShops.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserVistedItem
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ItemId { get; set; }
        public System.DateTime VisitedOn { get; set; }
    
        public virtual Items Items { get; set; }
        public virtual AspNetUsers AspNetUsers { get; set; }
    }
}
