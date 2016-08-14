using ButiqueShops.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ButiqueShops.ViewModels
{
    public class ItemsViewModel
    {
        public ItemsViewModel()
        {
            DateAdded = DateTime.Now;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int TypeId { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string ImagePath { get; set; }
        public string SmallImagePath { get; set; }
        public Nullable<int> ShopId { get; set; }
        public System.DateTime DateAdded { get; set; }
        public int [] sizesIds { get; set; }
        public int [] colorsIds { get; set; }

        public virtual List<Colors> Colors { get; set; }
        public virtual ItemTypes ItemTypes { get; set; }
        public virtual IEnumerable<UserLikedItem> UserLikedItem { get; set; }
        public virtual IEnumerable<UserVistedItem> UserVistedItem { get; set; }
        public virtual Shops Shops { get; set; }
        public virtual List<Sizes> Sizes { get; set; }
    }

    public class ItemSpecification
    {
        public int? shopid { get; set; }
        public int? [] colorids { get; set; }
        public int? [] sizeids { get; set; }
    }
}