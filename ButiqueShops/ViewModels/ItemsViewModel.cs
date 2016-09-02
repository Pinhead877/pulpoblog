using ButiqueShops.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ButiqueShops.ViewModels
{
    public class ItemsViewModel
    {
        public ItemsViewModel()
        {
            DateAdded = DateTime.Now;
            ImagePath = "/Images/defaultImage.jpg";
            SmallImagePath = "/Images/defaultSmaillImage.jpg";
            Colors = new List<Colors>();
            Sizes = new List<Sizes>();
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name="Type")]
        public int TypeId { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<int> Quantity { get; set; }
        [Display(Name = "Big Image")]
        public string ImagePath { get; set; }
        [Display(Name = "Small Image")]
        public string SmallImagePath { get; set; }
        [Required]
        [Display(Name = "Shop Name")]
        public Nullable<int> ShopId { get; set; }
        public System.DateTime DateAdded { get; set; }
        [Required]
        [Display(Name = "Sizes")]
        public int [] sizesIds { get; set; }
        [Required]
        [Display(Name = "Colors")]
        public int [] colorsIds { get; set; }
        [Display(Name = "Featured Item")]
        public bool IsFeatured { get; set; }

        [Display(Name = "Colors")]
        public virtual List<Colors> Colors { get; set; }
        [Display(Name = "Type")]
        public virtual ItemTypes ItemTypes { get; set; }
        public virtual IEnumerable<UserLikedItem> UserLikedItem { get; set; }
        public virtual IEnumerable<UserVistedItem> UserVistedItem { get; set; }
        public virtual Shops Shops { get; set; }
        [Display(Name = "Sizes")]
        public virtual List<Sizes> Sizes { get; set; }
    }

    public class ItemSpecification
    {
        public int? shopid { get; set; }
        public int? [] colorids { get; set; }
        public int? [] sizeids { get; set; }
        public int? itemTypeId { get; set; }
        public string ownerid { get; set; }
    }
}