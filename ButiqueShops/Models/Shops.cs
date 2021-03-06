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
    
    public partial class Shops
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Shops()
        {
            this.UserLikeShop = new HashSet<UserLikeShop>();
            this.UserVisitedShop = new HashSet<UserVisitedShop>();
            this.Items = new HashSet<Items>();
            this.ItemsToSubmit = new HashSet<ItemsToSubmit>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Website { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string OwnerId { get; set; }
        public string LogoPath { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public Nullable<double> Longitude { get; set; }
        public Nullable<double> Latitude { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserLikeShop> UserLikeShop { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserVisitedShop> UserVisitedShop { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Items> Items { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ItemsToSubmit> ItemsToSubmit { get; set; }
    }
}
