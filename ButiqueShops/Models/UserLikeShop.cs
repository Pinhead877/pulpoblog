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
    
    public partial class UserLikeShop
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ShopId { get; set; }
        public System.DateTime LikedOn { get; set; }
        public bool IsActive { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual Shops Shops { get; set; }
    }
}
