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
        [Display(Name = "Shop Name")]
        public string Name { get; set; }

        [Display(Name = "Website")]
        public string Website { get; set; }

        [Required]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Display(Name = "Adress")]
        public string AddressId { get; set; }

        [Required]
        [Display(Name = "Store Owner")]
        public string OwnerId { get; set; }

        [Display(Name = "Link To Store Logo")]
        public string LogoPath { get; set; }

        [Display(Name = "Date added in PulPo")]
        public DateTime DateAdded { get; set; }

        public virtual UserViewModel Owner { get; set; }
    }
}