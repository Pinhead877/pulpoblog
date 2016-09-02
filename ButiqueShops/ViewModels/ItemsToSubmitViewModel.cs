using ButiqueShops.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ButiqueShops.ViewModels
{
    public class ItemsToSubmitViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Type")]
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
        [Required]
        [Display(Name = "Sizes")]
        public int[] sizesIds { get; set; }
        [Required]
        [Display(Name = "Colors")]
        public int[] colorsIds { get; set; }

        [Display(Name = "Colors")]
        public virtual List<Colors> Colors { get; set; }
        [Display(Name = "Type")]
        public virtual ItemTypes ItemTypes { get; set; }
        public virtual Shops Shops { get; set; }
        [Display(Name = "Sizes")]
        public virtual List<Sizes> Sizes { get; set; }
    }
}
