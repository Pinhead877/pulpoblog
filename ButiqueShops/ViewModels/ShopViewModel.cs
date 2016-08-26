using ButiqueShops.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ButiqueShops.ViewModels
{
    public class ShopViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Website { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [Display(Name="Shop Owner")]
        public string OwnerId { get; set; }
        [Display(Name = "Logo Image")]
        public string LogoPath { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        [Required]
        public Nullable<double> Longitude { get; set; }
        [Required]
        public Nullable<double> Latitude { get; set; }

        [Display(Name = "Shop Owner")]
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual List<Items> Items { get; set; }
        public virtual List<UserLikeShop> UserLikeShop { get; set; }
        public virtual List<UserVisitedShop> UserVisitedShop { get; set; }
        public virtual List<ItemsToSubmit> ItemsToSubmit { get; set; }
    }
}