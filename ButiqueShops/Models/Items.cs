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
    
    public partial class Items
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Items()
        {
            this.UserLikedItem = new HashSet<UserLikedItem>();
            this.UserVistedItem = new HashSet<UserVistedItem>();
            this.Colors = new HashSet<Colors>();
            this.Sizes = new HashSet<Sizes>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public int TypeId { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string ImagePath { get; set; }
        public Nullable<int> ShopId { get; set; }
        public System.DateTime DateAdded { get; set; }
    
        public virtual ItemTypes ItemTypes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserLikedItem> UserLikedItem { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserVistedItem> UserVistedItem { get; set; }
        public virtual Shops Shops { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Colors> Colors { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sizes> Sizes { get; set; }
    }
}
